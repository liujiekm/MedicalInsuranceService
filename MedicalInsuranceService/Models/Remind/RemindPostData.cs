using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Models
{

    /// <summary>
    /// 提醒接口提交数据
    /// </summary>
    public class RemindPostData:PostBaseJson
    {
        [Jil.JilDirective(Name ="content")]
        public Content Content { get; set; }
    }
}