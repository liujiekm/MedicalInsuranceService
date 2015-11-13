using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models
{
    public class PostBaseJson
    {
        [JsonProperty(PropertyName = "auth_token")]
        public string auth_token { set; get; }

        [JsonProperty(PropertyName = "public_type")]
        public string public_type { set; get; }
    }
}