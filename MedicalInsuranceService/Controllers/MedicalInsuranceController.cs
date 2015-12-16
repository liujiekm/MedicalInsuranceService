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
using System.Web.Helpers;
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
            try
            {
                var postContent = new StringBuilder(JSON.Serialize(Mapping(content), option));
                Dlink(new StringBuilder(authToken), new StringBuilder(PublicType.Remind), postContent, new StringBuilder(baseUrl), Int32.Parse(timeOut) * 1000);
                remindResponseData = JSON.Deserialize<RemindResponseData>(postContent.ToString(), option);
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
                feedbackResponseData = JSON.Deserialize<FeedbackResponseData>(postContent.ToString(), option);
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

        [ApiExplorerSettings(IgnoreApi = true)]
        public Content Mapping(Content content)
        {
            //单位代码
            string unitCode = "";
            //外部代码
            string externalCode = "";

            string sql = "Select dwdm,wbbh from cw_khxx where brbh ='" + content.CardNo + "'";
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleCommand command = new OracleCommand(sql, con);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    unitCode = reader.GetValue(0).ToString();
                    externalCode = reader.GetValue(1).ToString();
                }
            }
            //把本地医生编号转换为社保医生编号
            content.DoctorCode = GetDoctorCode(content.DoctorCode, unitCode, externalCode);
            //将科室转换为社保科室代码
            var deptNOs = GetMapping("deptNOs", "SELECT ZKID,SBKSDM FROM CW_YB_KSDZ");
            if (deptNOs.ContainsKey(content.MedicalDeptCode))
            {
                content.MedicalDeptCode = deptNOs[content.MedicalDeptCode];
            }
            return content;
        }

        /// <summary>
        /// 修改医保相关字段
        /// </summary>
        /// <param name="auditContent"></param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public AuditContent Mapping(AuditContent auditContent)
        {
            //单位代码
            string unitCode = "";
            //外部代码
            string externalCode = "";
            //结算类型
            string settlementType = "";

            string sql = "Select dwdm,wbbh from cw_khxx where brbh ='" + auditContent.CardNo + "'";
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleCommand command = new OracleCommand(sql, con);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    unitCode = reader.GetValue(0).ToString();
                    externalCode = reader.GetValue(1).ToString();
                }
            }
            //把本地医生编号转换为社保医生编号
            auditContent.DoctorCode = GetDoctorCode(auditContent.DoctorCode, unitCode, externalCode);

            //科室医保编号   PB端处理
            var deptNOs = GetMapping("deptNOs", "SELECT ZKID,SBKSDM FROM CW_YB_KSDZ");
            if (deptNOs.ContainsKey(auditContent.MedicalDeptCode))
            {
                auditContent.MedicalDeptCode = deptNOs[auditContent.MedicalDeptCode];
            }
            var diagnoses = GetMapping("diagnoses", "SELECT ICD,SBBM FROM YLGZ3.YL_ZD");
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

            if (unitCode.Substring(0,1)=="A")
            {
                settlementType = "02";
            }
            else
            {
                settlementType = "01";
            }

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
        /// 根据本地医生id，以及就诊卡号，获取医生社保代码
        /// </summary>
        /// <param name="doctorCode">本地医生id</param>
        /// <param name="unitCode">单位代码</param>
        /// <param name="externalCode">外部编码</param>
        /// <returns></returns>
        public string GetDoctorCode(string doctorCode, string unitCode, string externalCode)
        {
            //接口代码
            string interfaceCode = "";

            //如果单位代码以A打头，则表示居民医保;如果是Y打头，则表示职工医保
            if (unitCode.Substring(0, 1) == "A")
            {
                interfaceCode = "xcjm";
            }
            else if (unitCode.Substring(0, 1) == "Y")
            {
                interfaceCode = "xczg";
            }

            //如果外部编号不包含“-”，则表示本级,如果包含，则表示市内或者异地，如果是3306打头的是sn，否则认为是yd
            if (externalCode.IndexOf('-') < 0)
            {
                interfaceCode += "_bj";
            }
            else if (externalCode.Substring(0, 4) == "3306")
            {
                interfaceCode += "_sn";
            }
            else
            {
                interfaceCode += "_yd";
            }

            string cacheKey = "doctorCode";
            var cache = HttpContext.Current.Cache;
            var mapping = new List<Doctor>();
            if (cache.Get(cacheKey) != null)
            {
                mapping = (List<Doctor>)cache.Get(cacheKey);
            }
            else
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();
                    string sql = "Select sbysdm,ysid,jkdm from cw_yb_ysxx";
                    OracleCommand queryCommand = new OracleCommand(sql, con);
                    OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
                    var reader = queryCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        Doctor doctor = new Doctor();
                        doctor.DoctorCode = reader.GetValue(0).ToString();
                        doctor.DoctorID = reader.GetValue(1).ToString();
                        doctor.InterfaceCode = reader.GetValue(2).ToString();
                        mapping.Add(doctor);
                    }
                    cache.Add(cacheKey, mapping, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
            }

            Doctor currentDoctor = mapping.Find(p => p.DoctorID == doctorCode && p.InterfaceCode == interfaceCode);
            if (currentDoctor != null)
            {
                doctorCode = currentDoctor.DoctorCode;
            }
            return doctorCode;
        }

        /// <summary>
        /// 获取收费项目对应的社保项目编号
        /// </summary>
        /// <param name="hospitalCode">医院内部编码</param>
        /// <param name="settlementType">结算类型</param>
        /// <param name="projectType">项目类型</param>
        /// <returns></returns>
        public string GetProject(string hospitalCode, string settlementType,string projectType)
        {
            string controlCode = "";
            string cacheKey = "projectCode";
            var cache = HttpContext.Current.Cache;
            var mapping = new List<FeesProject>();
            if (cache.Get(cacheKey) != null)
            {
                mapping = (List<FeesProject>)cache.Get(cacheKey);
            }
            else
            {
                using (OracleConnection con = new OracleConnection(connectionString))
                {
                    con.Open();
                    string sql = "Select dzdm,jslx,lb,yydm from cw_yb_tydzb";
                    OracleCommand queryCommand = new OracleCommand(sql, con);
                    OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
                    var reader = queryCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        FeesProject feesProject = new FeesProject();
                        feesProject.ControlCode = reader.GetValue(0).ToString();
                        feesProject.SettlementType = reader.GetValue(1).ToString();
                        feesProject.ProjectType = reader.GetValue(2).ToString();
                        feesProject.ProjectCode = reader.GetValue(3).ToString();
                        mapping.Add(feesProject);
                    }
                    cache.Add(cacheKey, mapping, dependency, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
            }

            FeesProject project = mapping.Find(p => p.ProjectType == projectType && p.ProjectCode == hospitalCode&&p.SettlementType==settlementType);
            if (project!=null)
            {
                controlCode = project.ControlCode;
            }
            return controlCode;
        }
    }
}
