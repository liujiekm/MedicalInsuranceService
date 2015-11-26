using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MedicalInsuranceService.Models.Maintenance;
using MedicalInsuranceService.Code;
using Oracle.ManagedDataAccess.Client;
//using Oracle.Web.Caching;
using System.Web.Caching;
using CacheDependency = System.Web.Caching.CacheDependency;
using Oracle.Web.Caching;

namespace MedicalInsuranceService.Controllers
{
    [RoutePrefix("Maintenance")]
    public class MaintenanceController : ApiController
    {
        private String connectionString;
        //private HttpClient client { set; get; }

        public MaintenanceController()
        {
            //client = new HttpClient((HttpMessageHandler)new HttpClientHandler(),disposeHandler:false);
            //client.Timeout = TimeSpan.FromSeconds(Int32.Parse(timeOut));

            //参数为数据库的用户名
            connectionString = OracleConnectionData.ConnectionString;

        }

        #region 维护方法

        /// <summary>
        /// 获取所有对照数据
        /// </summary>
        /// <returns></returns>
        [Route("GetControl")]
        [HttpGet]
        public List<Maintenance> GetControl()
        {
            List<Maintenance> list = new List<Maintenance>();
            String command = "SELECT dzid,sbdm,bddm,ywmz,zwmz,dzlb,jtms FROM CW_YBdz_ygyb where ztbz=1 order by dzid desc";
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleCommand queryCommand = new OracleCommand(command, con);
                //OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
                var reader = queryCommand.ExecuteReader();
                while (reader.Read())
                {
                    Maintenance maintenance = new Maintenance();
                    maintenance.ControlID = reader.GetInt32(0);
                    maintenance.SocialSecurityCode = reader.GetString(1);
                    maintenance.LocalCode = reader.GetString(2);
                    maintenance.EnglishName = reader.GetString(3);
                    maintenance.ChineseName = reader.GetString(4);
                    maintenance.Type = reader.GetString(5);
                    maintenance.Description = reader.GetString(6);
                    list.Add(maintenance);
                }
            }
            return list;
        }

        /// <summary>
        /// 根据对照ID删除对照
        /// </summary>
        /// <param name="controlID">对照ID</param>
        /// <returns></returns>
        [Route("DeleteControl/{controlID}")]
        [HttpPost]
        public int DeleteControl(int controlID)
        {
            int result = 0;
            String command = "update cw_ybdz_ygyb set ztbz=0 where dzid=" + controlID;
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleCommand queryCommand = new OracleCommand(command, con);
                //OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
                result = queryCommand.ExecuteNonQuery();
            }
            return result;
        }

        /// <summary>
        /// 保存对照，分新增和修改，如果对照id为0或者空，则为新增
        /// </summary>
        /// <param name="maintenance">对照对象</param>
        /// <returns></returns>
        [Route("SaveControl")]
        [HttpPost]
        public int SaveControl(Maintenance maintenance)
        {
            int result = 0;
            int controlID = GetControlID();
            String command = "";
            if (maintenance.ControlID == 0 || maintenance.ControlID == null)
            {
                command = @"insert into cw_ybdz_ygyb(dzid,sbdm,bddm,ywmz,zwmz,dzlb,jtms,ztbz) values(" + controlID + ",'"
                             + maintenance.SocialSecurityCode + "','" + maintenance.LocalCode + "','" + maintenance.EnglishName + "','" +
                             maintenance.ChineseName + "','" + maintenance.Type + "','" + maintenance.Description + "',1)";
            }
            else
            {
                command = @"update cw_ybdz_ygyb set sbdm='" + maintenance.SocialSecurityCode + "',bddm='" + maintenance.LocalCode
                                                    + "',ywmz='" + maintenance.EnglishName + "',zwmz='" + maintenance.ChineseName + "',dzlb='" +
                                                    maintenance.Type + "',jtms='" + maintenance.Description + "' where dzid=" + maintenance.ControlID;
            }
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleCommand updateCommand = new OracleCommand(command, con);
                result = updateCommand.ExecuteNonQuery();
            }

            if (result != 0 && (maintenance.ControlID == 0 || maintenance.ControlID == null))
            {
                result = controlID;
            }
            return result;
        }

        /// <summary>
        /// 获取对照序列ID
        /// </summary>
        /// <returns></returns>
        public int GetControlID()
        {
            int result = 0;
            String command = "select seq_cw_ybdz_ygyb.nextval from dual";
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                con.Open();
                OracleCommand queryCommand = new OracleCommand(command, con);
                //OracleCacheDependency dependency = new OracleCacheDependency(queryCommand);
                result = int.Parse(queryCommand.ExecuteScalar().ToString());
            }
            return result;
        }
        #endregion

        [Route("test")]
        [HttpGet]
        public Dictionary<string, string> test()
        {
            string cacheKey = "deptNOs";
            string command = "SELECT ZKID,SBKSDM FROM CW_YB_KSDZ";
            var cache = System.Web.HttpContext.Current.Cache;
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
                    //CacheDependency cd = GetOracleCacheDependency(command);
                    OracleCommand queryCommand = new OracleCommand(command, con);
                    OracleCacheDependency cd = new OracleCacheDependency(queryCommand);
                    var reader = queryCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        mapping.Add(reader.GetValue(0).ToString(), reader.GetString(1));
                    }
                    cache.Add(cacheKey, mapping, cd, System.Web.Caching.Cache.NoAbsoluteExpiration, System.Web.Caching.Cache.NoSlidingExpiration, CacheItemPriority.Normal, null);
                }
            }
            return mapping;
        }

        public CacheDependency GetOracleCacheDependency(string query)
        {
            OracleCacheDependency dependency;
            //using (OracleConnection con = new OracleConnection(connectionString))
            //{
            OracleConnection con = new OracleConnection(connectionString);
            con.Open();
            OracleCommand queryCommand = new OracleCommand(query, con);
            dependency = new OracleCacheDependency(queryCommand);
            queryCommand.ExecuteReader();
            //}
            return dependency;
        }
    }


}