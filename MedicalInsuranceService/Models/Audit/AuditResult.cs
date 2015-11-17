using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;

namespace MedicalInsuranceService.Models.Audit
{

    /// <summary>
    /// Audit 返回数据内容说明
    /// </summary>
    public class AuditResult
    {
        /// <summary>
        /// rule_name
        /// 规则名称
        /// </summary>
        [JilDirective(Name = "rule_name")]
        public String RuleName { get; set; }


        /// <summary>
        /// result_desc
        /// 异常级别描述（违规，高度可疑，可疑）
        /// </summary>
        [JilDirective(Name = "result_desc")]
        public String ResultDesc { get; set; }


        /// <summary>
        /// project_code
        /// 项目编号
        /// </summary>
        [JilDirective(Name = "project_code")]
        public String ProjectCode { get; set; }

        /// <summary>
        /// project_name	
        /// 项目名称
        /// </summary>
        [JilDirective(Name = "project_name")]
        public String ProjectName { get; set; }

        /// <summary>
        /// ex_desc
        /// 异常描述
        /// </summary>
        [JilDirective(Name = "ex_desc")]
        public String ExDesc { get; set; }


    }
}