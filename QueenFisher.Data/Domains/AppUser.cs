using Microsoft.AspNetCore.Identity;
using QueenFisher.Data.Enums;

namespace QueenFisher.Data.Domains
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public bool IsActive { get; set; }
        public string? PublicId { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string RefreshToken { get; set; } = String.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public bool IsDeleted { get; set; }

        public ICollection<TimeTable> TimeTables { get; set; }
        public ICollection<Recipe> Recipes { get; set; }

    }
}
