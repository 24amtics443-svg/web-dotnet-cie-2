// Imports the Doctor model.
using Task_2.Models;

// Namespace for extension methods.
namespace Task_2.Extensions
{
    // Static class is required for extension methods.
    public static class DoctorExtensions
    {
        // Extension method calculates the total consultation charge.
        public static decimal CalculateTotalCharge(this Doctor doctor)
        {
            // Adds ₹100 service charge to the consultation fee.
            decimal totalCharge = doctor.ConsultationFee + 100;

            // Returns the final amount.
            return totalCharge;
        }
    }
}