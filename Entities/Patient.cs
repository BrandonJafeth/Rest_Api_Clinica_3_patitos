namespace Entities
{
    public class Patient
    {
        public int Id_Patient { get; set; }
        public string Pat_Name { get; set; }
        public string Phone { get; set; }

        // Relations
        public int Id_Appoitment { get; set; }
        public Appointment Appointment { get; set; }

        public User User { get; set; }

        public string Email => User?.Email;

    }
}
