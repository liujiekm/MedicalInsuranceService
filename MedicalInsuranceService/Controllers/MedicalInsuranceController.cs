using MedicalInsuranceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MedicalInsuranceService.Controllers
{
    [RoutePrefix("MI")]
    public class MedicalInsuranceController : ApiController
    {
        /// <summary>
        ///调用阳光医保提示服务
        /// </summary>
        [Route("NR")]
        [HttpPost]
        public void NoticeRemind()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:9000");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var postData = new PostBaseJson() {
                };
                //var response = await client.PostAsJsonAsync("",)
            }
        }

        [Route("PT")]
        [HttpPost]
        public ResponseBaseJson PBPostTest(PostBaseJson postData)
        {
            return
                new ResponseBaseJson()
                {
                    success = "T",
                    error_code = "null"
                };

            
        }


    }
}
