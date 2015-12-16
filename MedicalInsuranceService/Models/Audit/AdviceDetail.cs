using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;

namespace MedicalInsuranceService.Models.Audit
{
    /// <summary>
    /// 医嘱明细信息说明
    /// </summary>
    public class AdviceDetail
    {



        /// <summary>
        /// project_code	
        /// 项目编码(使用医保三目录项目编码)，正常情况必填，特殊情况，如生化全套等没有医保编码，可放空。
        /// </summary>
        [JilDirective(Name = "project_code")]
        public String ProjectCode { get; set; }

        /// <summary>
        /// hospital_code
        /// 院内项目编码
        /// </summary>
        [JilDirective(Name = "hospital_code")]
        public String HospitalCode { get; set; }

        /// <summary>
        /// project_name
        /// 项目名称
        /// </summary>
        [JilDirective(Name = "project_name")]
        public String ProjectName { get; set; }

        /// <summary>
        /// 项目类别，1：药品，2：诊疗
        /// </summary>
        [JilDirective(Name="xmlb")]
        public string ProjectType { get; set; }

        /// <summary>
        /// recipe_no
        /// 处方号
        /// </summary>
        [JilDirective(Name = "recipe_no")]
        public String RecipeNo { get; set; }

        /// <summary>
        /// invoice_project
        /// 发票项目编码（使用医保发票项目编码）
        /// </summary>
        [JilDirective(Name = "invoice_project")]
        public String InvoiceProject { get; set; }



        /// <summary>
        /// dose_form
        /// 剂型, 具体可使用剂型详见附录五的编码
        /// </summary>
        [JilDirective(Name = "dose_form")]
        public String DoseForm { get; set; }

        /// <summary>
        /// medical_specification
        /// 规格
        /// </summary>
        [JilDirective(Name = "medical_specification")]
        public String MedicalSpecification { get; set; }



        /// <summary>
        /// price
        /// 单价，必须使用数值型，如”12.5”
        /// </summary>
        [JilDirective(Name = "price")]
        public float Price { get; set; }

        /// <summary>
        /// medical_number	
        /// 数量，必须使用数值型，如”10”
        /// </summary>
        [JilDirective(Name = "medical_number")]
        public float MedicalNumber { get; set; }


        /// <summary>
        /// dose_unit
        /// 单位
        /// </summary>
        [JilDirective(Name = "dose_unit")]
        public String DoseUnit { get; set; }


        /// <summary>
        /// amount
        /// 金额，必须使用数值型，如”50.5”
        /// </summary>
        [JilDirective(Name = "amount")]
        public float Amount { get; set; }


        /// <summary>
        /// use_day
        /// 用药天数（项目为药品时非空），医嘱服用该药品天数，必须使用数值型，如”15”
        /// </summary>
        [JilDirective(Name = "use_day")]
        public float UseDay { get; set; }


        /// <summary>
        /// single_dose_number
        /// 单次用药量（项目为药品时非空），配合下一个字段的单位，如 50 mg，必须使用数值型，如”50”
        /// </summary>
        [JilDirective(Name = "single_dose_number")]
        public float SingleDoseNumber { get; set; }


        /// <summary>
        /// single_dose_unit
        /// 单次用药量剂量单位（项目为药品时非空），具体可使用剂量单位详见附录四的编码
        /// </summary>
        [JilDirective(Name = "single_dose_unit")]
        public String SingleDoseUnit { get; set; }


        /// <summary>
        /// take_medical_number	
        /// 取药总量（项目为药品时非空），配合下一个字段的单位，如 500 mg，必须使用数值型，如”500”
        /// </summary>
        [JilDirective(Name = "take_medical_number")]
        public float TakeMedicalNumber { get; set; }

        /// <summary>
        /// take_medical_unit
        /// 取药总量剂量单位（项目为药品时非空），具体可使用剂量单位附录四的编码
        /// </summary>
        [JilDirective(Name = "take_medical_unit")]
        public String TakeMedicalUnit { get; set; }

        /// <summary>
        /// dose_day
        /// 药量天数（项目为药品时非空），依据医嘱服药要求，所配药品患者可以服用的天数，必须使用数值型，如”5”
        /// </summary>
        [JilDirective(Name = "dose_day")]
        public float DoseDay { get; set; }


        /// <summary>
        /// deliver_way
        /// 给药途径，具体给药途径详见附录三的编码
        /// </summary>
        [JilDirective(Name = "deliver_way")]
        public String DeliverWay { get; set; }


        /// <summary>
        /// take_frequence
        /// 服用频次，具体可用服用频次详见附录二的编码
        /// </summary>
        [JilDirective(Name = "take_frequence")]
        public String TakeFrequence { get; set; }

    }
}