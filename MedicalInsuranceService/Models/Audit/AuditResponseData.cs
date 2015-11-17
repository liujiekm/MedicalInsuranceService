using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;

namespace MedicalInsuranceService.Models.Audit
{
    /// <summary>
    /// Audit服务返回的整体数据
    /// </summary>
    public class AuditResponseData:ResponseBaseJson
    {

        [JilDirective(Name ="result")]
        public List<AuditResult> Results { get; set; }


        [JilDirective(Name = "reasons")]
        public List<AuditReason> Reasons { get; set; }
    }
}