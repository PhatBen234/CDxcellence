namespace Unilever.CDExcellent.API.Models.Dto
{
    public class DashboardDto
    {
        public UserInfoDto UserInfo { get; set; }
        public int PendingTasks { get; set; }
        public int UpcomingVisits { get; set; }
        public List<NotificationDto> RecentNotifications { get; set; }
    }

}
