using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Unilever.CDExcellent.API.Services.IService;
using Unilever.CDExcellent.API.Models.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace Unilever.CDExcellent.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // 1. Lấy danh sách thông báo của user
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetNotifications(int userId)
        {
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            return Ok(notifications);
        }

        // 2. Lấy danh sách thông báo chưa đọc
        [HttpGet("{userId}/unread")]
        public async Task<IActionResult> GetUnreadNotifications(int userId)
        {
            var notifications = await _notificationService.GetUnreadNotificationsAsync(userId);
            return Ok(notifications);
        }

        // 3. Tìm kiếm thông báo theo từ khóa
        [HttpGet("{userId}/search")]
        public async Task<IActionResult> SearchNotifications(int userId, [FromQuery] string keyword)
        {
            var notifications = await _notificationService.SearchNotificationsAsync(userId, keyword);
            return Ok(notifications);
        }

        // 4. Lấy chi tiết thông báo
        [HttpGet("detail/{notificationId}")]
        public async Task<IActionResult> GetNotificationDetail(int notificationId)
        {
            var notification = await _notificationService.GetNotificationByIdAsync(notificationId);
            if (notification == null) return NotFound("Notification not found");

            return Ok(notification);
        }

        // 5. Tạo thông báo mới
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationRequest request)
        {
            var senderId = int.Parse(User.FindFirstValue(JwtRegisteredClaimNames.Sub));
            await _notificationService.CreateNotificationAsync(senderId, request.ReceiverId, request.Title, request.Message);
            return Ok(new { message = "Notification sent successfully." });
        }

        // 6. Đánh dấu thông báo đã đọc
        [HttpPut("{notificationId}/read")]
        public async Task<IActionResult> MarkAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return NoContent();
        }
    }
}
