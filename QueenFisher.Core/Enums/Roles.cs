
using System.Text.Json.Serialization;

namespace QueenFisher.Data.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        SuperAdmin=0,
        Admin=1,
        Customer=2,
    }
}
