namespace IsoTreatmentProcessSupportAPI.Entities
{
    public class Reminder
    {
        public int Id { get; set; }
        public TimeOnly Time { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
