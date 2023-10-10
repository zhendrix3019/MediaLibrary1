using System;
using System.Globalization;
using NLog;

namespace MediaLibrary
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");

        static void Main(string[] args)
        {
            logger.Info("Program started");

            // Use the scrubbed file, obtained from the FileScrubber
            string filePath = FileScrubber.ScrubMovies("movies.csv");

            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
            {
                logger.Error("Scrubbed file not found or couldn't be created: {File}", filePath);
                Console.WriteLine("File not found. Please ensure the movies.csv file is in the correct directory.");
                return;
            }

            MovieFile movieFile = new MovieFile(filePath);

            string choice;
            do
            {
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("Press ENTER to quit");
                choice = Console.ReadLine();

                logger.Info($"User choice: {choice}");

                if (choice == "1")
                {
                    Movie movie = new Movie();
                    
                    Console.WriteLine("Enter movie title");
                    movie.title = Console.ReadLine();

                    string genre;
                    do
                    {
                        Console.WriteLine("Enter genre (or done to quit)");
                        genre = Console.ReadLine();
                        if (genre.ToLower() != "done" && !string.IsNullOrWhiteSpace(genre))
                        {
                            movie.genres.Add(genre);
                        }
                    } while (genre.ToLower() != "done");

                    Console.WriteLine("Enter movie director");
                    movie.director = Console.ReadLine();

                    Console.WriteLine("Enter running time (h:m:s)");
                    // Using CultureInfo.InvariantCulture to ensure consistent format, especially for parsing TimeSpan
                    movie.runningTime = TimeSpan.Parse(Console.ReadLine(), CultureInfo.InvariantCulture);

                    movieFile.AddMovie(movie);

                    // Save the movie directly to the scrubbed file
                    movieFile = new MovieFile(filePath);
                }
                else if (choice == "2")
                {
                    // Refresh the movieFile object to get the latest data from the file before displaying movies
                    movieFile = new MovieFile(filePath);
                    
                    foreach (var movie in movieFile.Movies)
                    {
                        Console.WriteLine($"Id: {movie.mediaId}");
                        Console.WriteLine($"Title: {movie.title}");
                        Console.WriteLine($"Director: {movie.director}");
                        Console.WriteLine($"Run time: {movie.runningTime}");
                        Console.WriteLine($"Genres: {string.Join(", ", movie.genres)}");
                        Console.WriteLine();
                    }
                }
            } while (choice == "1" || choice == "2");

            logger.Info("Program ended");
        }
    }
}
