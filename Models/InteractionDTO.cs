namespace PRWebAPI.Models
{
    public class InteractionDTO
    { 
    public DateTime InteractionDate { get; set; }
    public DateTime MeetingDate { get; set; }
    public string? Reason { get; set; }
    public string? Comment { get; set; }
    public int ContactDetailsID { get; set; }
    }
}
