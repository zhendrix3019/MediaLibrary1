public class MovieFile
{
    private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

    public string filePath { get; set; }
    public List<Movie> Movies { get; set; }

    public MovieFile(string movieFilePath)
    {
        filePath = movieFilePath;
        Movies = new List<Movie>();

        try
        {
            StreamReader sr = new StreamReader(filePath);
            
            // Assuming that there are no headers in the scrubbed file
            while (!sr.EndOfStream)
            {
                Movie movie = new Movie();
                string line = sr.ReadLine();

                // Parse the file data here as per the actual file structure

                Movies.Add(movie);
            }
            sr.Close();
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
    }

    public void AddMovie(Movie movie)
    {
        try
        {
            movie.mediaId = Movies.Max(m => m.mediaId) + 1;
            StreamWriter sw = new StreamWriter(filePath, true);
            sw.WriteLine($"{movie.mediaId},{movie.title},{string.Join("|", movie.genres)},{movie.director},{movie.runningTime}");
            sw.Close();
            Movies.Add(movie);
            logger.Info("Media id {Id} added", movie.mediaId);
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
    }
}