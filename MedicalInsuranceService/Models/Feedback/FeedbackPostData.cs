using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;
namespace MedicalInsuranceService.Models.Feedback
{

    /// <summary>
    /// 反馈服务的请求数据
    /// </summary>
    public class FeedbackPostData:PostBaseJson
    {

        /// <summary>
        /// 反馈服务的请求数据内容主体
        /// </summary>
        [JilDirective(Name ="content")]
        public FeedbackContent Content { get; set; }

    }
}