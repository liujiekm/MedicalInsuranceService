using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;
namespace MedicalInsuranceService.Models.Audit
{

    /// <summary>
    /// Audit 返回数据字段Reason
    /// </summary>
    public class AuditReason
    {

        /// <summary>
        /// reason_no
        /// 原因编号，在反馈时上传使用，无需在前台显示
        /// </summary>
        [JilDirective(Name = "reason_no")]
        public String ReasonNo { get; set; }


        /// <summary>
        /// reason_desc
        /// 原因说明，可供医师选择的原因文字，需要前台显示
        /// </summary>
        [JilDirective(Name = "reason_desc")]
        public String ReasonDesc { get; set; }

    }
}