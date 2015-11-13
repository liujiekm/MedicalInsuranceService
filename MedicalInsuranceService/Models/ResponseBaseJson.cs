using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace MedicalInsuranceService.Models
{
    public class ResponseBaseJson
    {
        [JsonProperty(PropertyName = "success")]
        public string success { set; get; }

        [JsonProperty(PropertyName = "error_code")]
        public string error_code { set; get; }

        [JsonProperty(PropertyName = "error_msg")]
        public string error_msg { set; get; }

        [JsonProperty(PropertyName = "tran_serial_no")]
        public string tran_serial_no { set; get; }
    }
}