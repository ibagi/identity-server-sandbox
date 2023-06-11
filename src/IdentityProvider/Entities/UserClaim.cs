using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Entities
{
    public class UserClaim
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
