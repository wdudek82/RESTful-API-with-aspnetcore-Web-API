using System.ComponentModel.DataAnnotations;

namespace ParkyAPIApp.Models.Dtos
{
    public class TrailUpdateDto
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public Trail.DifficultyType Difficulty { get; set; }

        public int NationalParkId { get; set; }
    }
}