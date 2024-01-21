using IsoTreatmentProcessSupportAPI.Converters;
using System.Text.Json.Serialization;

namespace IsoTreatmentProcessSupportAPI.Models
{
    public class ReminderDto
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonTimeOnlyConverter))]
        public TimeOnly Time { get; set; }
    }
}
