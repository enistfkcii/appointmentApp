using System.ComponentModel.DataAnnotations;
namespace AppointmentSystem.Models
{
    public class Services
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}