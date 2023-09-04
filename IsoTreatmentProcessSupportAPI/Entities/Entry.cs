namespace IsoTreatmentProcessSupportAPI.Entities
{
    public class Entry
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
