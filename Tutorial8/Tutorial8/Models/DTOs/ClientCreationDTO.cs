using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;

    public class ClientCreationDTO
    {
        [Required]
        [StringLength(120, ErrorMessage = "FirstName cannot exceed 120 characters.")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(120, ErrorMessage = "LastName cannot exceed 120 characters.")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? Telephone { get; set; }
        
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Pesel must be exactly 11 digits.")]
        public string? Pesel { get; set; }
    }
