using System.Text.Json.Serialization;

namespace QueenFisher.Data.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MealType
    {
        Breakfast = 0,
        Lunch = 1,
        Dinner= 2
    }
}
