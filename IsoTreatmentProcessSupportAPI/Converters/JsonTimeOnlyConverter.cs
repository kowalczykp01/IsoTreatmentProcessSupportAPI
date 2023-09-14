﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace IsoTreatmentProcessSupportAPI.Converters
{
    public class JsonTimeOnlyConverter : JsonConverter<TimeOnly>
    {
        public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String && TimeSpan.TryParse(reader.GetString(), out var timeSpan))
            {
                return TimeOnly.FromTimeSpan(timeSpan);
            }
            throw new JsonException($"Unable to parse TimeOnly from JSON.");
        }

        public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
