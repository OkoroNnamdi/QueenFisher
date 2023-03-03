namespace QueenFisher.Data.Domains
{
    public class Meal: BaseEntity
    {
        public string Name { get; set; }

        //Navigational mapping
        public string TimeTableId { get; set; }
        public TimeTable TimeTable { get; set; }

    }
}