using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Jil;

namespace MedicalInsuranceService.Models
{


    /// <summary>
    ///医保返回数据基类
    /// </summary>
    public class ResponseBaseJson
    {
        /// <summary>
        /// 请求是否成功。 T代表成功，F代表失败
        /// </summary>
        //[JsonProperty(PropertyName = "success")]
        [JilDirective(Name = "success")]
        public string Success { set; get; }

        /// <summary>
        /// 错误编码
        /// </summary>
        [JilDirective(Name = "error_code")]
        public string ErrorCode { set; get; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JilDirective(Name = "error_msg")]
        public string ErrorMsg { set; get; }

        /// <summary>
        /// 交易流水号（唯一主键）
        /// </summary>
        [JilDirective(Name = "tran_serial_no")]
        public string TranSerialNo { set; get; }
    }
}