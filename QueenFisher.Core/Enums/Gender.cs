using System.Text.Json.Serialization;

namespace QueenFisher.Core.Enums
{

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        Male = 0,
        Female = 1,
    }
}
