using Unilever.CDExcellent.API.Data;
using Unilever.CDExcellent.API.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardDto> GetDashboardDataAsync(int userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new UserInfoDto
            {
                UserId = u.Id,
                UserName = u.FullName, 
                ProfilePicture = "" 
            })
            .FirstOrDefaultAsync();

        var pendingTasks = await _context.UserTasks
            .Where(t => t.AssigneeId == userId)
            .CountAsync(); 

        var upcomingVisits = await _context.VisitPlans
            .Where(vp => vp.ActorId == userId && vp.VisitDate >= DateTime.UtcNow)
            .CountAsync(); 

        var recentNotifications = await _context.Notifications
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.CreatedAt)
            .Take(5)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                Message = n.Message,
                CreatedAt = n.CreatedAt,
                IsRead = n.IsRead
            })
            .ToListAsync();

        return new DashboardDto
        {
            UserInfo = user,
            PendingTasks = pendingTasks,
            UpcomingVisits = upcomingVisits,
            RecentNotifications = recentNotifications
        };
    }
}
