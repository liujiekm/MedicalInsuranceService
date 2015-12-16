using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models
{
    /// <summary>
    /// 收费项目
    /// </summary>
    public class FeesProject
    {
        /// <summary>
        /// 对照代码
        /// </summary>
        public string ControlCode { get; set; }

        /// <summary>
        /// 结算类型
        /// </summary>
        public string SettlementType { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// 项目代码
        /// </summary>
        public string ProjectCode { get; set; }
    }
}