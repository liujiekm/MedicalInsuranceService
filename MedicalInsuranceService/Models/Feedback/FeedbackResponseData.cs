using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;
namespace MedicalInsuranceService.Models.Feedback
{

    /// <summary>
    /// 反馈服务返回的数据
    /// </summary>
    public class FeedbackResponseData:ResponseBaseJson
    {
        /// <summary>
        /// result 字段
        /// </summary>
        [JilDirective(Name ="result")]
        public String  Result { get; set; }
        
    }
}