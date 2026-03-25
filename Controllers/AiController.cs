using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace DeeplearningwithCapybara.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public AiController(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
           
            _apiKey = configuration["GeminiApiKey"];
        }
        [HttpPost("schedule-advise")]
        public IActionResult ScheduleAdvise([FromBody] ScheduleRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Subject))
                return BadRequest(new { error = "Vui lòng nhập tên môn học." });

            string subject = request.Subject.Trim();
            var deadline = request.Deadline;

            if (subject.Contains("- deadline", StringComparison.OrdinalIgnoreCase))
            {
                var index = subject.IndexOf("- deadline", StringComparison.OrdinalIgnoreCase);
                var datePart = subject.Substring(index + 10).Trim();
                subject = subject.Substring(0, index).Trim();

                if (DateTime.TryParseExact(datePart, new[] { "dd/MM", "d/M", "dd/MM/yyyy", "d/M/yyyy" },
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                {
                    // Nếu người dùng chỉ nhập ngày tháng (không có năm), dùng năm hiện tại
                    if (!datePart.Contains("/")) parsedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(7); // fallback
                    else if (datePart.Length <= 5) 
                    {
                        parsedDate = new DateTime(DateTime.Now.Year, parsedDate.Month, parsedDate.Day);
                        if (parsedDate < DateTime.Now) parsedDate = parsedDate.AddYears(1);
                    }
                    deadline = parsedDate;
                }
            }

            deadline = deadline ?? DateTime.Now.AddDays(7);
            var daysLeft = (int)(deadline.Value - DateTime.Now).TotalDays;
            if (daysLeft < 1) daysLeft = 1;

            // Mock AI schedule generation
            var slots = new List<object>();
            var startDays = new[] { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7" };
            var times = new[] { "07:00 - 09:00", "09:00 - 11:00", "14:00 - 16:00", "19:00 - 21:00" };

            int sessions = Math.Min(daysLeft * 2, 8);
            for (int i = 0; i < sessions; i++)
            {
                slots.Add(new
                {
                    day = startDays[i % startDays.Length],
                    time = times[i % times.Length],
                    subject = subject,
                    duration = "2 giờ"
                });
            }

            return Ok(new
            {
                subject = subject,
                deadline = deadline.Value.ToString("dd/MM/yyyy"),
                daysLeft,
                totalSessions = slots.Count,
                suggestions = slots,
                tip = daysLeft <= 3
                    ? "⚠️ Deadline gần rồi! Hãy tập trung tối đa."
                    : "✅ Bạn còn đủ thời gian. Học đều đặn mỗi ngày nhé!"
            });
        }
    }

    public class ScheduleRequest
    {
        [System.Text.Json.Serialization.JsonPropertyName("subject")]
        public string Subject { get; set; } = string.Empty;

        [System.Text.Json.Serialization.JsonPropertyName("deadline")]
        public DateTime? Deadline { get; set; }
    }
}
