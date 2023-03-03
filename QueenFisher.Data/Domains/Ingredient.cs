namespace QueenFisher.Data.Domains
{
    public class Ingredient : BaseEntity
    {
        public string Name { get; set; }

        //Navigational mapping
        public string RecipeId { get; set; }
        public Recipe Recipe { get; set; }

    }
}