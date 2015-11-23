using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models
{

    /// <summary>
    /// 阳光医保服务类型
    /// </summary>
    public class PublicType
    {
        /// <summary>
        ///提醒服务
        /// </summary>
        public static readonly string Remind = "remind";

        /// <summary>
        /// 审核服务
        /// </summary>
        public static readonly string Audit = "audit";


        /// <summary>
        /// 反馈服务
        /// </summary>
        public static readonly string Feedback = "feedback";
    }
}