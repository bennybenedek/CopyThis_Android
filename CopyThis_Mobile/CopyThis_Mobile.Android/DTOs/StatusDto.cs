using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CopyThisServer.Model.Server.Data
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
