using System;
using System.Text.Json.Serialization;

namespace MusicPlayerApplication.Dtos
{
    public class LogDto
    {
        [JsonPropertyName("@timestamp")]
        public string Timestamp => DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.fff'Z'");
        [JsonPropertyName("fields")]
        public Fields Fields { get; set; }
    }

    public class Fields
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("level")]
        public string Level { get; set; }
        [JsonPropertyName("environnement")]
        public string Environnement { get; set; }
        [JsonPropertyName("application")]
        public string Application { get; set; }
    }
}
