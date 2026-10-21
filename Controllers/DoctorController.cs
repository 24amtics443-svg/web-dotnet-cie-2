// Imports the extension method CalculateTotalCharge().
using Task_2.Extensions;

// Imports Doctor and Post models.
using Task_2.Models;

// Imports MVC functionality.
using Microsoft.AspNetCore.Mvc;

// Imports memory cache functionality.
using Microsoft.Extensions.Caching.Memory;

// Imports JSON functionality.
using System.Text.Json;

// Namespace for controllers.
namespace Task_2.Controllers
{
    // DoctorController manages doctors and appointments.
    public class DoctorController : Controller
    {
        // Stores the memory cache object.
        private readonly IMemoryCache _cache;

        // Stores the HttpClient object.
        private readonly HttpClient _httpClient;

        // Constructor receives required services using Dependency Injection.
        public DoctorController(
            IMemoryCache cache,
            IHttpClientFactory httpClientFactory)
        {
            // Stores the memory cache service.
            _cache = cache;

            // Creates an HttpClient.
            _httpClient = httpClientFactory.CreateClient();
        }

        // Displays the list of doctors.
        public IActionResult Index()
        {
            // Creates a key for the doctor list cache.
            string cacheKey = "doctorList";

            // Checks whether doctor data already exists in cache.
            if (!_cache.TryGetValue(
                cacheKey,
                out List<Doctor>? doctors))
            {
                // Creates doctor records when cache is empty.
                doctors = new List<Doctor>
                {
                    // First doctor.
                    new Doctor
                    {
                        // Doctor ID.
                        DoctorId = 1,

                        // Doctor name.
                        DoctorName = "Dr. Raj Patel",

                        // Doctor specialization.
                        Specialization = "Cardiologist",

                        // Years of experience.
                        Experience = 10,

                        // Consultation fee.
                        ConsultationFee = 800
                    },

                    // Second doctor.
                    new Doctor
                    {
                        // Doctor ID.
                        DoctorId = 2,

                        // Doctor name.
                        DoctorName = "Dr. Priya Shah",

                        // Doctor specialization.
                        Specialization = "Dermatologist",

                        // Years of experience.
                        Experience = 7,

                        // Consultation fee.
                        ConsultationFee = 600
                    },

                    // Third doctor.
                    new Doctor
                    {
                        // Doctor ID.
                        DoctorId = 3,

                        // Doctor name.
                        DoctorName = "Dr. Amit Mehta",

                        // Doctor specialization.
                        Specialization = "Neurologist",

                        // Years of experience.
                        Experience = 12,

                        // Consultation fee.
                        ConsultationFee = 1000
                    }
                };

                // Creates cache settings.
                var cacheOptions = new MemoryCacheEntryOptions()
                    // Sets cache expiration to 3 minutes.
                    .SetAbsoluteExpiration(
                        TimeSpan.FromMinutes(3));

                // Stores doctor list in memory cache.
                _cache.Set(
                    cacheKey,
                    doctors,
                    cacheOptions);
            }

            // Sends doctor list to Index.cshtml.
            return View(doctors);
        }

        // Caches the response of the Details page for 60 seconds.
        [ResponseCache(
            Duration = 60,
            Location = ResponseCacheLocation.Any)]
        public IActionResult Details(int DoctorId)
        {
            // Uses the doctor list cache key.
            string cacheKey = "doctorList";

            // Retrieves doctor list from memory cache.
            if (!_cache.TryGetValue(
                cacheKey,
                out List<Doctor>? doctors))
            {
                // Returns Not Found if the cache is empty.
                return NotFound();
            }

            // Finds doctor using DoctorId.
            var doctor = doctors?.FirstOrDefault(
                d => d.DoctorId == DoctorId);

            // Checks whether doctor exists.
            if (doctor == null)
            {
                // Returns 404 Not Found.
                return NotFound();
            }

            // Stores DoctorId in ViewBag.
            ViewBag.DoctorId = DoctorId;

            // Stores specialization in Session.
            HttpContext.Session.SetString(
                "Specialization",
                doctor.Specialization);

            // Reads patient name from Cookie.
            string patientName =
                Request.Cookies["PatientName"]
                ?? "Guest";

            // Sends patient name to the View.
            ViewBag.PatientName = patientName;

            // Calculates total consultation charge.
            ViewBag.TotalCharge =
                doctor.CalculateTotalCharge();

            // Sends doctor object to Details.cshtml.
            return View(doctor);
        }

        // Handles appointment form submission.
        [HttpPost]
        public IActionResult Appointment(
            int DoctorId,
            string PatientName,
            string Specialization)
        {
            // Creates a Cookie to store patient name.
            Response.Cookies.Append(
                "PatientName",
                PatientName,
                new CookieOptions
                {
                    // Cookie expires after one day.
                    Expires = DateTimeOffset.Now.AddDays(1)
                });

            // Stores specialization in Session.
            HttpContext.Session.SetString(
                "Specialization",
                Specialization);

            // Creates a temporary success message.
            TempData["Message"] =
                "Appointment request submitted successfully.";

            // Redirects to Doctor Details page.
            // DoctorId is passed using Query String.
            return RedirectToAction(
                "Details",
                new { DoctorId = DoctorId });
        }

        // Gets posts asynchronously from JSONPlaceholder API.
        public async Task<IActionResult> Posts()
        {
            // Sends an asynchronous GET request.
            var response = await _httpClient.GetAsync(
                "https://jsonplaceholder.typicode.com/posts");

            // Checks whether the HTTP request was successful.
            response.EnsureSuccessStatusCode();

            // Reads API response asynchronously.
            var json =
                await response.Content.ReadAsStringAsync();

            // Converts JSON data into Post objects.
            var posts =
                JsonSerializer.Deserialize<List<Post>>(
                    json,
                    new JsonSerializerOptions
                    {
                        // Allows case-insensitive property matching.
                        PropertyNameCaseInsensitive = true
                    });

            // Sends posts to Posts.cshtml.
            return View(posts);
        }
    }
}