using Jil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models
{


    /// <summary>
    /// 请求内容基类
    /// </summary>
    public class Content
    {


        /// <summary>
        /// card_no		 
        /// 社保卡号
        /// </summary>
        [JilDirective(Name = "card_no")]
        public String CardNo { get; set; }

        /// <summary>
        /// medical_dept_code		 
        /// 科室编码（使用医院科室编号）
        /// </summary>
        [JilDirective(Name = "medical_dept_code")]
        public String  MedicalDeptCode { get; set; }


        /// <summary>
        /// medical_dept_name		 
        /// 科室名称
        /// </summary>
        [JilDirective(Name = "medical_dept_name")]
        public String  MedicalDeptName { get; set; }


        /// <summary>
        /// doctor_code		 
        /// 医师编号（使用医院医生编号）
        /// </summary>
        [JilDirective(Name = "doctor_code")]
        public String  DoctorCode { get; set; }



        /// <summary>
        /// doctor_name		 
        /// 医师姓名
        /// </summary>
        [JilDirective(Name = "doctor_name")]
        public String  DoctorName { get; set; }

    }
}