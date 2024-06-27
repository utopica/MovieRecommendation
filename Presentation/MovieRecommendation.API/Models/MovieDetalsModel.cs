namespace MovieRecommendation.API.Models
{
    public class MovieDetalsModel
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTimeOffset ReleaseDate { get; set; }
        public string Language { get; set; }
        public double AverageRating { get; set; }
        public int UserRating { get; set; }
        public string UserNote { get; set; }
    }
}
