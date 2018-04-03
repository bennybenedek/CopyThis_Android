using Newtonsoft.Json;

namespace CopyThisServer.Model.Server.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClipboardDto
    {
        [JsonProperty("content")]
        public string Content { get; set; }
    }
}
