using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentSystem.Models
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime AppointmentDate { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int ServiceId { get; set; }

        [ForeignKey("ServiceId")]
        public virtual Services Service { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
