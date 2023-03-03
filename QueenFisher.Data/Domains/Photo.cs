namespace QueenFisher.Data.Domains
{
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        //Navigational mapping
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

    }
}