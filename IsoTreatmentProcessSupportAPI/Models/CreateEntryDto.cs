namespace IsoTreatmentProcessSupportAPI.Models
{
    public class CreateEntryDto
    {
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
    }
}
