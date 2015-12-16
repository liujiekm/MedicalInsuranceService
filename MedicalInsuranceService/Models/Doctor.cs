using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models
{
    /// <summary>
    /// 社保医生
    /// </summary>
    public class Doctor
    {
        /// <summary>
        /// 社保医生编号
        /// </summary>
        public string DoctorCode { get; set; }

        /// <summary>
        /// 本地医生ID
        /// </summary>
        public string DoctorID { get; set; }

        /// <summary>
        /// 接口代码，xcjm_bj：居民医保 xczg_bj：职工医保_本级 xczg_sn：职工医保_市内 xczg_yd：职工医保_异地等

        /// </summary>
        public string InterfaceCode { get; set; }
    }
}