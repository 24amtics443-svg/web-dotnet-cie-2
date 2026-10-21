// Namespace used for model classes in this application.
namespace Task_2.Models
{
    // Doctor class stores information about a doctor.
    public class Doctor
    {
        // Stores the unique ID of the doctor.
        public int DoctorId { get; set; }

        // Stores the name of the doctor.
        public string DoctorName { get; set; } = "";

        // Stores the medical specialization of the doctor.
        public string Specialization { get; set; } = "";

        // Stores the doctor's years of experience.
        public int Experience { get; set; }

        // Stores the doctor's consultation fee.
        public decimal ConsultationFee { get; set; }
    }
}