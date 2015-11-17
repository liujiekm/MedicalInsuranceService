using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;

namespace MedicalInsuranceService.Models.Audit
{
    /// <summary>
    /// 诊断信息说明
    /// </summary>
    public class Diagnose
    {
        /// <summary>
        /// diagnose_code	true	char	诊断代码（使用医保下发诊断代码（ICD-10））
        /// </summary>
        /// 
        [JilDirective(Name = "diagnose_code")]
        public String DiagnoseCode { get; set; }


        /// <summary>
        /// diagnose_desc	true	char 诊断描述
        /// </summary>
        [JilDirective(Name = "diagnose_desc")]
        public String DiagnoseDesc { get; set; }

    }
}