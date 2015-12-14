using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models.Maintenance
{
    /// <summary>
    /// 对照数据维护类
    /// </summary>
    public class Maintenance
    {
        /// <summary>
        /// 对照ID
        /// </summary>
        public int? ControlID { get; set; }

        /// <summary>
        /// 社保代码
        /// </summary>
        public string SocialSecurityCode { get; set; }

        /// <summary>
        /// 本地代码
        /// </summary>
        public string LocalCode { get; set; }

        /// <summary>
        /// 英文名称
        /// </summary>
        public string EnglishName { get; set; }

        /// <summary>
        /// 中文名称
        /// </summary>
        public string ChineseName { get; set; }

        /// <summary>
        /// 类别
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 具体描述
        /// </summary>
        public string Description { get; set; }
    }
}