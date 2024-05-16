namespace Entities
{
    public class Patient
    {
        public int PatientId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        // Relación
        public ICollection<Appointment> Appointments { get; set; }
    }
}
