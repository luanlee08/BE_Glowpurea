namespace BE_Glowpurea.Dtos.Dashboard
{
    public class UserStatsDto
    {
        public int TotalUsers { get; set; }
        public int NewUsersThisMonth { get; set; }

        public List<int> UserByMonth { get; set; } = new();

        public List<int> UserByWeekThisWeek { get; set; } = new();
        public List<int> UserByWeekLastWeek { get; set; } = new();
    }
}
