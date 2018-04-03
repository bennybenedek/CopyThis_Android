using Newtonsoft.Json;

namespace CopyThisServer.Model.Server.Data
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
