namespace Entities
{
    public class Patient
    {
        public int Id_Patient { get; set; }
        public string Pat_Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Relations

        public Appointment Appointment { get; set; }
        
    }
}
