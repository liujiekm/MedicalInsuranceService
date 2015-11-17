using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;

namespace MedicalInsuranceService.Models.Audit
{

    /// <summary>
    /// 审核请求数据
    /// </summary>
    public class AuditPostData:PostBaseJson
    {
        /// <summary>
        /// Audit请求内容主体
        /// </summary>
        [JilDirective(Name ="content")]
        public AuditContent Content { get; set; }

    }
}