using QueenFisher.Data.Enums;

namespace QueenFisher.Data.Domains
{
    public class TimeTable: BaseEntity
    {
        public MealType MealType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        //Navigational mapping
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public ICollection<Meal> Meal { get; set; }
    }
}