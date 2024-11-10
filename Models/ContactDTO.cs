namespace PRWebAPI.Models
{
    public class ContactDTO
    {

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Designation { get; set; }
        public string Priority { get; set; }
        public string IsAIM { get; set; }
        public string Relation { get; set; }
        public string ContactAddedBy { get; set; }
        public string? ContactOwnership { get; set; }
        public string? Status { get; set; }
    }
}
