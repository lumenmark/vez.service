using System.Text.Json;
using System.Text.Json.Serialization;

namespace UsaWeb.Service.Helper
{
    public class IgnoreNullPropertiesConverter<T> : JsonConverter<T> where T : class
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(ref reader, options);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetValue(value) != null) // Exclude null properties
                .ToDictionary(p => p.Name, p => p.GetValue(value));

            JsonSerializer.Serialize(writer, properties, options);
        }
    }
}
