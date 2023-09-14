using IsoTreatmentProcessSupportAPI.Converters;
using System.Text.Json.Serialization;

namespace IsoTreatmentProcessSupportAPI.Models
{
    public class CreateReminderDto
    {
        [JsonConverter(typeof(JsonTimeOnlyConverter))]
        public TimeOnly Time { get; set; }
        public int UserId { get; set; }
    }
}
