using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tutorial8.Models.DTOs;
    public class TripDTO
    {
        [Required]
        public int IdTrip { get; set; }

        [Required]
        [StringLength(120)]
        public string Name { get; set; }

        [StringLength(220)]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateFrom { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTo { get; set; }

        [Range(1, int.MaxValue)]
        public int MaxPeople { get; set; }

        [Required]
        public List<string> Countries { get; set; } = new();
    }
