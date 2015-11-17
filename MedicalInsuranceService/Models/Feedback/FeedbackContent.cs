using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;
namespace MedicalInsuranceService.Models.Feedback
{

    /// <summary>
    /// 反馈服务的请求数据内容主体
    /// </summary>
    public class FeedbackContent
    {
        /// <summary>
        ///  tran_serial_no
        /// 审核接口返回的交易流水号
        /// </summary>
        [JilDirective(Name = "tran_serial_no")]
        public String TranSerialNo { get; set; }


        /// <summary>
        ///  reason_no
        /// 医师根据原因清单选择的原因编号。若手工填写，则为空
        /// </summary>
        [JilDirective(Name = "reason_no")]
        public String ReasonNo { get; set; }


        /// <summary>
        ///  reason_other
        /// 若未填写原因编号，则将手工填写的原因说明上传。最长200个字符。
        /// </summary>
        [JilDirective(Name = "reason_other")]
        public String ReasonOther { get; set; }




        


    }
}