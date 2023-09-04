namespace IsoTreatmentProcessSupportAPI.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool IsTaken { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
