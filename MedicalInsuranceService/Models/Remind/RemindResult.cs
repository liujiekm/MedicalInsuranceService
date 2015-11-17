using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Jil;
namespace MedicalInsuranceService.Models
{

    /// <summary>
    ///提示服务返回数据结果
    /// </summary>
    public class RemindResult
    {

        /// <summary>
        /// serial_no	true	序号
        /// </summary>
        [JilDirective(Name = "serial_no")]
        public String  SerialNo { get; set; }


        /// <summary>
        /// remind_msg	true	提示信息
        /// </summary>
        [JilDirective(Name = "remind_msg")]
        public String  RemindMsg { get; set; }
        


            

    }
}