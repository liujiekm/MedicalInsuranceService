using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;
using System.Runtime.Serialization;

namespace MedicalInsuranceService.Models
{

    /// <summary>
    /// 请求数据基类
    /// </summary>
    public class PostBaseJson
    {

        /// <summary>
        /// 阳光医保给各医院分配的Token
        /// </summary>
        //[JsonProperty(PropertyName = "auth_token")]
        [JilDirective(Name = "auth_token")]
        public string AuthToken { set; get; }



        /// <summary>
        /// 接口类型
        /// notice通知，remind提醒、audit审核、feedback反馈
        /// </summary>
        //[JsonProperty(PropertyName = "public_type")]
        [JilDirective(Name = "public_type")]
        public string PublicType { set; get; }
    }
}