using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using DeeplearningwithCapybara.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace DeeplearningwithCapybara.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly DLWcapybara _context;
        private readonly UserManager<Users> _userManager;

        public AiController(IConfiguration configuration, DLWcapybara context, UserManager<Users> userManager)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["GeminiApiKey"];
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("schedule-advise")]
        public async Task<IActionResult> ScheduleAdvise([FromBody] ScheduleRequestDTO request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Subject))
                return BadRequest(new { error = "Vui lòng nhập tên môn học." });

            // Tính toán khoảng thời gian thực tế
            var startDate = request.StartDate ?? DateTime.Now;
            var deadline = request.Deadline;
            if (deadline <= startDate) deadline = startDate.AddDays(7);

            var totalDays = (int)(deadline - startDate).TotalDays;
            if (totalDays < 1) totalDays = 1;

       
            string prompt = $@"
                Bạn là chuyên gia lập kế hoạch học tập AI. 
                Hãy lập lộ trình học môn '{request.Subject}' từ ngày {startDate:dd/MM/yyyy} đến {deadline:dd/MM/yyyy}.
                Thông tin người học: 
                - Trình độ: {request.Level}
                - Mức độ ưu tiên: {request.Priority}/3 (1:Thấp, 2:Trung bình, 3:Cao)
                - Thời gian học: {request.StudyHoursPerDay} giờ/buổi
                - Phong cách: {request.StudyStyle}

                RÀNG BUỘC QUAN TRỌNG: 
                1. Dàn trải lịch học dựa trên {totalDays} ngày khả dụng. 
                2. Nếu Priority là 3, hãy xếp lịch dày hơn (4-5 buổi/tuần). Nếu Priority 1, xếp 2 buổi/tuần.
                3. Nội dung task phải cụ thể, tăng dần độ khó.
                
                TRẢ VỀ DUY NHẤT một JSON array cấu trúc:
                [
                    {{ 
                        ""taskName"": ""Tên nhiệm vụ cụ thể"", 
                        ""startDate"": ""YYYY-MM-DDTHH:mm:ss (ngày bắt đầu buổi học)"", 
                        ""endDate"": ""YYYY-MM-DDTHH:mm:ss (ngày kết thúc buổi học, thêm {request.StudyHoursPerDay} giờ)"", 
                        ""duration"": ""{request.StudyHoursPerDay} giờ"",
                        ""priority"": {request.Priority}
                    }}
                ]";

            if (string.IsNullOrEmpty(_apiKey))
                return Ok(GenerateMockData(request.Subject, deadline, totalDays));

            try
            {
                var requestBody = new
                {
                    contents = new[] { new { parts = new[] { new { text = prompt } } } },
                    generationConfig = new { responseMimeType = "application/json" }
                };

                var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}", content);

                if (response.IsSuccessStatusCode)
                {
                    var resultString = await response.Content.ReadAsStringAsync();
                    using var doc = JsonDocument.Parse(resultString);
                    var text = doc.RootElement.GetProperty("candidates")[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();

                    var tasks = JsonSerializer.Deserialize<List<AiTaskResponse>>(text);
                    return Ok(new
                    {
                        subject = request.Subject,
                        courseId = request.CourseId,
                        deadline = deadline.ToString("dd/MM/yyyy"),
                        tasks = tasks
                    });
                }
            }
            catch (Exception ex) { Console.WriteLine("AI Error: " + ex.Message); }

            return Ok(GenerateMockData(request.Subject, deadline, totalDays));
        }

        [Authorize]
        [HttpPost("save-schedule")]
        public async Task<IActionResult> SaveSchedule([FromBody] SaveScheduleRequest request)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // 1. Tạo Plan
            var studyPlan = new StudyPlans
            {
                UserId = user.Id,
                CourseId = request.CourseId > 0 ? request.CourseId : (_context.Courses.FirstOrDefault()?.CourseId ?? 1),
                ExamDate = request.Deadline,
                Note = "Lộ trình AI: " + request.Subject
            };

            _context.StudyPlans.Add(studyPlan);
            await _context.SaveChangesAsync();

            // 2. Lưu danh sách Task
            foreach (var t in request.Tasks)
            {
                DateTime startTime = DateTime.TryParse(t.StartDate, out var s) ? s : DateTime.Now;
                DateTime endTime = DateTime.TryParse(t.EndDate, out var e) ? e : startTime.AddHours(2);

                _context.Tasks.Add(new DeeplearningwithCapybara.Models.Tasks
                {
                    PlanId = studyPlan.PlanId,
                    TaskName = t.TaskName,
                    StartDate = startTime,
                    EndDate = endTime,
                    Status = false,
                    Priority = t.Priority,
                    CourseId = studyPlan.CourseId
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { success = true });
        }

        private object GenerateMockData(string subject, DateTime deadline, int totalDays)
        {
            // Mock data thông minh hơn: Tạo 1 task mỗi 2 ngày
            var slots = new List<AiTaskResponse>();
            for (int i = 0; i < totalDays; i += 2)
            {
                var start = DateTime.Now.AddDays(i);
                slots.Add(new AiTaskResponse
                {
                    TaskName = $"Học {subject} - Giai đoạn {i / 2 + 1}",
                    StartDate = start.ToString("yyyy-MM-ddT08:00:00"),
                    EndDate = start.ToString("yyyy-MM-ddT10:00:00"),
                    Duration = "2 giờ",
                    Priority = 2
                });
            }
            return new { subject, deadline = deadline.ToString("dd/MM/yyyy"), tasks = slots };
        }
    }


    public class ScheduleRequestDTO
    {
        public int CourseId { get; set; }
        public string Subject { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public int Priority { get; set; }
        public string Level { get; set; }
        public double StudyHoursPerDay { get; set; }
        public string StudyStyle { get; set; }
    }

    public class AiTaskResponse
    {
        [JsonPropertyName("taskName")] public string TaskName { get; set; } = string.Empty;
        [JsonPropertyName("startDate")] public string? StartDate { get; set; }
        [JsonPropertyName("endDate")] public string? EndDate { get; set; }
        [JsonPropertyName("duration")] public string Duration { get; set; } = string.Empty;
        [JsonPropertyName("priority")] public int Priority { get; set; }
    }

    public class SaveScheduleRequest
    {
        public int CourseId { get; set; }
        public string Subject { get; set; }
        public DateTime Deadline { get; set; }
        public List<AiTaskResponse> Tasks { get; set; } = new List<AiTaskResponse>();
    }
}