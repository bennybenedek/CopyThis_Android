using System;
using System.Net;
using Newtonsoft.Json;

namespace CopyThis_Mobile.DTOs
{
    [JsonObject(MemberSerialization.OptIn)]
    internal class StatusDto
    {
        public StatusDto() { }

        public StatusDto(HttpStatusCode code)
            : this(code, Enum.GetName(typeof(HttpStatusCode), code)) { }

        public StatusDto(HttpStatusCode code, string status)
        {
            StatusCode = (int) code;
            Status = status;
        }

        [JsonProperty("code")]
        public int StatusCode { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }


        public static StatusDto OK => new StatusDto(HttpStatusCode.OK);
        public static StatusDto Unauthorized => new StatusDto(HttpStatusCode.Unauthorized);
    }
}
