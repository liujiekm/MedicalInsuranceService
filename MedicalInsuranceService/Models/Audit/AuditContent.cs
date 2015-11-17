using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;

namespace MedicalInsuranceService.Models.Audit
{

    /// <summary>
    /// Audit 服务基本信息内容
    /// </summary>
    public class AuditContent:Content
    {



        /*
        










        
        */

        /// <summary>
        /// visit_no
        /// 门诊挂号号/住院登记号
        /// </summary>
        [JilDirective(Name = "visit_no")]
        public String VisitNo { get; set; }


        /// <summary>
        /// visit_type
        /// 就诊类型（1住院、2门诊）
        /// </summary>
        [JilDirective(Name = "visit_type")]
        public String VisitType { get; set; }

        /// <summary>
        /// medicine_type
        /// 医疗类别，见附录一的编码
        /// </summary>
        [JilDirective(Name = "medicine_type")]
        public String MedicineType { get; set; }


        /// <summary>
        /// in_hosp_date
        /// 入院日期（就诊类型是住院的非空,格式：YYYYMMDD）
        /// </summary>
        [JilDirective(Name = "in_hosp_date")]
        public String InHospDate { get; set; }

        /// <summary>
        /// doctor_advice_no
        /// 处方流水号（医院内部唯一号）
        /// </summary>
        [JilDirective(Name = "doctor_advice_no")]
        public String DoctorAdviceNo { get; set; }


        /// <summary>
        /// diagnoses
        /// 诊断信息（多项）（内容说明参见Diagnoses（诊断信息说明））
        /// </summary>
        [JilDirective(Name = "diagnoses")]
        public List<Diagnose> Diagnoses { get; set; }

        /// <summary>
        /// advice_details
        /// 医嘱明细信息（多项）（内容说明参见advice_details(医嘱明细信息说明)）
        /// </summary>
        [JilDirective(Name = "advice_details")]
        public List<AdviceDetail> AdviceDetails { get; set; }
    }
}