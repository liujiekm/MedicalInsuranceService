using MedicalInsuranceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.Configuration;
using MedicalInsuranceService.Formatter;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using MedicalInsuranceService.Models.Feedback;
using MedicalInsuranceService.Models.Audit;
using System.Web;
using System.Web.Caching;

using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using Oracle.Web.Caching;
using System.Runtime.InteropServices;
using System.Text;
using Jil;
using MedicalInsuranceService.Code;
using System.ComponentModel;
using System.Web.Http.Description;

namespace MedicalInsuranceService.Controllers
{
    [RoutePrefix("MIS")]
    public class MedicalInsuranceController : ApiController
    {
        private readonly string baseUrl = ConfigurationManager.AppSettings["baseUrl"];
        private readonly string authToken = ConfigurationManager.AppSettings["auth_token"];
        private readonly string timeOut = ConfigurationManager.AppSettings["timeout"];


        private String connectionString;
        private Options option;
        //private HttpClient client { set; get; }

        public MedicalInsuranceController()
        {
            //client = new HttpClient((HttpMessageHandler)new HttpClientHandler(),disposeHandler:false);
            //client.Timeout = TimeSpan.FromSeconds(Int32.Parse(timeOut));
            //参数为数据库的用户名
            connectionString = OracleConnectionData.ConnectionString;
            option = new Options(excludeNulls: true, includeInherited: true);

        }

        #region 通过HTTPClient POST方式调用（平安不支持）

        ///// <summary>
        /////调用阳光医保提示服务
        ///// </summary>
        //[Route("Remind")]
        //[HttpPost]
        //public  RemindResponseData Remind(Content content)
        //{
        //        var postContent = new RemindPostData() {
        //             AuthToken=authToken,
        //             PublicType= PublicType.Remind,
        //             Content=content
        //        };
        //    RemindResponseData remindResponseData = new RemindResponseData();

        //    var response = client.PostAsync<RemindPostData>(baseUrl, postContent, new JilFormatter()).Result;
        //    //response.EnsureSuccessStatusCode();
        //    if(response.IsSuccessStatusCode)
        //    {
        //        remindResponseData = response.Content.ReadAsAsync<RemindResponseData>(
        //        new MediaTypeFormatter[] { new JilFormatter() }).Result;
        //    }
        //    else//错误 或者超时
        //    {
        //        remindResponseData.Success = "F";
        //        remindResponseData.ErrorCode = "";
        //        remindResponseData.ErrorMsg = "";
        //    }
        //    return remindResponseData;
        //}
        ///// <summary>
        ///// 测试PB调用POST方法
        ///// </summary>
        ///// <param name="postData">传递的数据</param>
        ///// <returns></returns>
        //[Route("PT")]
        //[HttpPost]
        //public ResponseBaseJson PBPostTest(PostBaseJson postData)
        //{
        //    return
        //        new ResponseBaseJson()
        //        {
        //            Success = "T",
        //            ErrorCode = "null"
        //        };
        //}


        ///// <summary>
        /////调用阳光医保审核服务
        ///// </summary>
        //[Route("Audit")]
        //[HttpPost]
        //public AuditResponseData Feedback(AuditContent content)
        //{
        //    var postContent = new AuditPostData()
        //    {
        //        AuthToken = authToken,
        //        PublicType = PublicType.Feedback,
        //        Content = content
        //    };
        //    AuditResponseData auditResponseData = new AuditResponseData();
        //    var response = client.PostAsync<AuditPostData>(baseUrl, postContent, new JilFormatter()).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        auditResponseData = response.Content.ReadAsAsync<AuditResponseData>(
        //        new MediaTypeFormatter[] { new JilFormatter() }).Result;
        //    }
        //    else//错误 或者超时
        //    {
        //        auditResponseData.Success = "F";
        //        auditResponseData.ErrorCode = "";
        //        auditResponseData.ErrorMsg = "";
        //    }
        //    return auditResponseData;
        //}

        ///// <summary>
        /////调用阳光医保反馈服务
        ///// </summary>
        //[Route("Feedback")]
        //[HttpPost]
        //public FeedbackResponseData Feedback(FeedbackContent content)
        //{
        //    var postContent = new FeedbackPostData()
        //    {
        //        AuthToken = authToken,
        //        PublicType = PublicType.Feedback,
        //        Content = content
        //    };
        //    FeedbackResponseData feedbackResponseData = new FeedbackResponseData();

        //    var response = client.PostAsync<FeedbackPostData>(baseUrl, postContent, new JilFormatter()).Result;

        //    if (response.IsSuccessStatusCode)
        //    {
        //        feedbackResponseData = response.Content.ReadAsAsync<FeedbackResponseData>(
        //        new MediaTypeFormatter[] { new JilFormatter() }).Result;
        //    }
        //    else//错误 或者超时
        //    {
        //        feedbackResponseData.Success = "F";
        //        feedbackResponseData.ErrorCode = "";
        //        feedbackResponseData.ErrorMsg = "";
        //    }
        //    return feedbackResponseData;
        //}



        #endregion




        [DllImport("siaudit.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern int Dlink(StringBuilder auth_token, StringBuilder public_type, StringBuilder content, StringBuilder myapi);



        [DllImport("siaudit.dll", CharSet = CharSet.None, ExactSpelling = false)]
        private static extern int Dlink(StringBuilder auth_token, StringBuilder public_type, StringBuilder content, StringBuilder myapi, Int32 timeout);


        /// <summary>
        /// 创建缓存依赖项
        /// </summary>
        /// <param name="query">查询命令</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public CacheDependency GetOracleCacheDependency(string query)
        {
            OracleConnection con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand queryCommand = new OracleCommand(query);
            OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
            queryCommand.ExecuteReader();
            return dependency;
        }

        /// <summary>
        /// 通过NCache类库方式设置缓存依赖项
        /// </summary>
        //public void GetNCacheDependency()
        //{
        //    var cache = NCache.InitializeCache("NCache");
        //    String connection = "";
        //    String qureyCommand = "";
        //    CacheDependency orclSync = new OracleCacheDependency(connection, qureyCommand);

        //    cache.Insert("", "", orclSync, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, Alachisoft.NCache.Runtime.CacheItemPriority.Normal);
        //}




        /// <summary>
        /// 调用阳光医保提示（Remind）服务
        /// </summary>
        /// <param name="content">提示服务请求参数，详见《平安智慧医保智能审核系统事前审核接口文档》</param>
        /// <returns>提示服务返回参数，详见《平安智慧医保智能审核系统事前审核接口文档》</returns>
        [Route("Remind")]
        [HttpPost]
        public RemindResponseData RemindThroughDll(Content content)
        {
            var exception = new Exception();
            RemindResponseData remindResponseData = new RemindResponseData();
            var postContent = new StringBuilder(JSON.Serialize(content, option));
            try
            {
                Dlink(new StringBuilder(authToken), new StringBuilder(PublicType.Remind), postContent, new StringBuilder(baseUrl), Int32.Parse(timeOut)*1000);
                remindResponseData = JSON.Deserialize<RemindResponseData>(postContent.ToString(),option);
            }
            catch (Win32Exception ex)
            {
                //exception = ex;
                remindResponseData.Success = "F";
                remindResponseData.ErrorCode = "";
                remindResponseData.ErrorMsg = ex.Message;
            }
            return remindResponseData;
        }

        /// <summary>
        /// 修改医保相关字段
        /// </summary>
        /// <param name="auditContent"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public AuditContent Mapping(AuditContent auditContent)
        {
            //科室医保编号   PB端处理
            //var deptNOs = GetMapping("deptNOs", "SELECT ZKID,SBKSDM FROM CW_YB_KSDZ");
            //auditContent.MedicalDeptCode = deptNOs[auditContent.MedicalDeptCode];
            var diagnoses = GetMapping("diagnoses", "SELECT JBID,SBBM FROM YLGZ3.YL_ZD");
            //诊断代码
            foreach (var diagnose in auditContent.Diagnoses)
            {
                if (diagnoses.ContainsKey(diagnose.DiagnoseCode))
                {
                    diagnose.DiagnoseCode = diagnoses[diagnose.DiagnoseCode];
                }
            }
            //医疗类别
            var medicalTypes = GetMapping("medicalTypes", "SELECT distinct BDDM,SBDM FROM CW_YBDZ_YGYB WHERE DZLB='yllb' AND ZTBZ=1");
            if (medicalTypes.ContainsKey(auditContent.MedicineType))
            {
                auditContent.MedicineType = medicalTypes[auditContent.MedicineType];   
            }

            //剂型类别
            var doseForms = GetMapping("doseForms", "SELECT distinct BDDM,SBDM FROM CW_YBDZ_YGYB WHERE DZLB='jxlb' AND ZTBZ=1 ");

            //剂量单位
            var singleDoseUnits = GetMapping("singleDoseUnits", "SELECT distinct BDDM,SBDM FROM CW_YBDZ_YGYB WHERE DZLB='jldw' AND ZTBZ=1 ");

            //给药途径
            var deliverWays = GetMapping("deliverWays", "SELECT distinct BDDM,SBDM FROM CW_YBDZ_YGYB WHERE DZLB='gytj' AND ZTBZ=1 ");

            //药品使用频次
            var takeFrequences = GetMapping("takeFrequences", "SELECT distinct BDDM,SBDM FROM CW_YBDZ_YGYB WHERE DZLB='ypsypc' AND ZTBZ=1 ");
            foreach (var adviceDetail in auditContent.AdviceDetails)
            {
                //if (!String.IsNullOrEmpty(adviceDetail.DoseForm))
                if (doseForms.ContainsKey(adviceDetail.DoseForm))
                {
                    adviceDetail.DoseForm = doseForms[adviceDetail.DoseForm];
                }

                if (singleDoseUnits.ContainsKey(adviceDetail.SingleDoseUnit))
                {
                    adviceDetail.SingleDoseUnit = singleDoseUnits[adviceDetail.SingleDoseUnit];
                }

                if (deliverWays.ContainsKey(adviceDetail.DeliverWay))
                {
                    adviceDetail.DeliverWay = deliverWays[adviceDetail.DeliverWay];
                }

                if (takeFrequences.ContainsKey(adviceDetail.TakeFrequence))
                {
                    adviceDetail.TakeFrequence = takeFrequences[adviceDetail.TakeFrequence];
                }
            }
            return auditContent;
        }







        /// <summary>
        /// 获取通过查询命令command获取医保编码映射关系
        /// 通过配置缓存依赖，保证缓存的实时更新
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public Dictionary<string, string> GetMapping(string cacheKey, string command)
        {
            var cache = HttpContext.Current.Cache;
            var mapping = new Dictionary<string, string>();
            if (cache.Get(cacheKey) != null)
            {
                mapping = (Dictionary<string, string>)cache.Get(cacheKey);
            }
            else
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();
                    OracleCommand queryCommand = new OracleCommand(command, con);
                    OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
                    var reader = queryCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        mapping.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                    cache.Add(cacheKey, mapping, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
            }
            return mapping;
        }




        /// <summary>
        /// 调用阳光医保审核（Audit）服务
        /// </summary>
        /// <param name="content">审核服务请求参数，详见《平安智慧医保智能审核系统事前审核接口文档》</param>
        /// <returns>审核服务返回参数，详见《平安智慧医保智能审核系统事前审核接口文档》</returns>
        [Route("Audit")]
        [HttpPost]
        public AuditResponseData FeedbackThroughDll(AuditContent content)
        {
            var exception = new Exception();
            AuditResponseData auditResponseData = new AuditResponseData();
            try
            {
                var postContent = new StringBuilder(JSON.Serialize(Mapping(content), option));
                Dlink(new StringBuilder(authToken), new StringBuilder(PublicType.Audit), postContent, new StringBuilder(baseUrl), Int32.Parse(timeOut) * 1000);
                auditResponseData = JSON.Deserialize<AuditResponseData>(postContent.ToString(), option);
            }
            catch (Win32Exception ex)
            {
                //exception = ex;
                auditResponseData.Success = "F";
                auditResponseData.ErrorCode = "";
                auditResponseData.ErrorMsg = ex.Message;
            }

            return auditResponseData;
        }



        /// <summary>
        /// 调用阳光医保反馈（Feedback）服务
        /// </summary>
        /// <param name="content">反馈服务请求参数，详见《平安智慧医保智能审核系统事前审核接口文档》</param>
        /// <returns>反馈服务返回参数，详见《平安智慧医保智能审核系统事前审核接口文档》</returns>
        [Route("Feedback")]
        [HttpPost]
        public FeedbackResponseData FeedbackThroughDll(FeedbackContent content)
        {

            var exception = new Exception();
            FeedbackResponseData feedbackResponseData = new FeedbackResponseData();
            var postContent = new StringBuilder(JSON.Serialize(content, option));
            try
            {
                Dlink(new StringBuilder(authToken), new StringBuilder(PublicType.Feedback), postContent, new StringBuilder(baseUrl), Int32.Parse(timeOut) * 1000);
                feedbackResponseData = JSON.Deserialize<FeedbackResponseData>(postContent.ToString(),option);
            }
            catch (Win32Exception ex)
            {
                //exception = ex;
                feedbackResponseData.Success = "F";
                feedbackResponseData.ErrorCode = "";
                feedbackResponseData.ErrorMsg = ex.Message;
            }
            return feedbackResponseData;
        }
    }
}
