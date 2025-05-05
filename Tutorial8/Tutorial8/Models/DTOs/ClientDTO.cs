using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;
    public class ClientDTO
    {
        [Required]
        public int IdClient { get; set; }

        [Required]
        [StringLength(120)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(120)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string? Telephone { get; set; }

        [RegularExpression(@"^\d{11}$")]
        public string? Pesel { get; set; }
    }
