using System.ComponentModel.DataAnnotations;

namespace IdentityProvider.Entities
{
    public class UserSecret
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Secret { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }
    }
}
