using Microsoft.AspNetCore.Mvc;

namespace DeeplearningwithCapybara.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        // POST /api/ai/schedule-advise
        // Body: { "subject": "Deep Learning", "deadline": "2026-03-25T23:59:00" }
        [HttpPost("schedule-advise")]
        public IActionResult ScheduleAdvise([FromBody] ScheduleRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Subject))
                return BadRequest(new { error = "Vui lòng nhập tên môn học." });

            var deadline = request.Deadline ?? DateTime.Now.AddDays(7);
            var daysLeft = (int)(deadline - DateTime.Now).TotalDays;
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
                    subject = request.Subject,
                    duration = "2 giờ"
                });
            }

            return Ok(new
            {
                subject = request.Subject,
                deadline = deadline.ToString("dd/MM/yyyy"),
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
        public string Subject { get; set; } = string.Empty;
        public DateTime? Deadline { get; set; }
    }
}
