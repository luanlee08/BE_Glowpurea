namespace BE_Glowpurea.Dtos.Account
{
    public class UpdateCustomerStatusRequest
    {
        public string Status { get; set; } = "Active"; // Blocked | Active
    }
}
