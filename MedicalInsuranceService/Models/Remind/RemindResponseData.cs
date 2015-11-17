using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jil;

namespace MedicalInsuranceService.Models
{


    /// <summary>
    /// 提醒服务返回数据
    /// </summary>
    public class RemindResponseData:ResponseBaseJson
    {
        [JilDirective(Name ="result")]
        public List<RemindResult> RemindResults { get; set; }
    }
}