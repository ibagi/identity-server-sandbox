using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(200)]
        [Required]
        public string Subject { get; set; }

        [MaxLength(200)]
        public string UserName { get; set; }

        [MaxLength(200)]
        public string Password { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public string PasswordSalt { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        public ICollection<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public ICollection<UserSecret> Secrets { get; set; } = new List<UserSecret>();
    }
}
