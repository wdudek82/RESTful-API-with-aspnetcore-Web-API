using System.ComponentModel.DataAnnotations;
using static ParkyAPIApp.Models.Trail;

namespace ParkyAPIApp.Models.Dtos
{
    public class TrailDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        [Required]
        public double Elevation { get; set; }

        public DifficultyType Difficulty { get; set; }

        public int NationalParkId { get; set; }

        public NationalParkDto NationalPark { get; set; }

        // public DateTime DateCreated { get; set; }
    }
}
