using System;
using Newtonsoft.Json;

namespace CopyThis_Mobile.DTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class PictureDto
    {
        [JsonProperty("content")]
        private string content;

        [JsonProperty("filename")]
        public string Name { get; set; }

        public byte[] Content
        {
            get => Convert.FromBase64String(content);
            set => content = Convert.ToBase64String(value);
        }
    }
}
