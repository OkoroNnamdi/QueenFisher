using QueenFisher.Data.Enums;

namespace QueenFisher.Data.Domains
{
    public class Recipe : BaseEntity
    {
        public string Name { get; set; }
        public MealType MealType { get; set; }
        public string Procedure { get; set; }
        public string Summary { get; set; }

        //Navigational mapping
        public ICollection<Ingredient> Ingredients { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Video> Videos { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}