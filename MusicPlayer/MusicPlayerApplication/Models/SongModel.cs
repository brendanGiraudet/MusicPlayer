using System.Text.Json.Serialization;

namespace MusicPlayerApplication.Models
{
    public class SongModel
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        public string Path { get; set; }
        [JsonPropertyName("artist")]
        public string Artist { get; set; }
        public string ImagePath { get; set; }
        public string FileName { get; set; }
    }
}
