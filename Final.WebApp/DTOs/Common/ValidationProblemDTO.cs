using System.Text.Json.Serialization;

namespace Final.WebApp.DTOs.Common
{
    public class ValidationProblemDTO
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("errors")]
        public Dictionary<string, List<string>> Errors { get; set; } = [];
    }
}
