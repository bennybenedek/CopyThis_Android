using Newtonsoft.Json;

namespace CopyThis_Mobile.DTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class NoteDto
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
