using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Instrumentation;
using System.Collections.Specialized;
using System;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;

namespace MedicalInsuranceService.Data.Configuration
{
    /// <summary>
    /// Describes a <see cref="OracleDatabase"/> instance, aggregating information from a
    /// <see cref="ConnectionStringSettings"/>.
    /// </summary>
    public class OracleDatabaseData : DatabaseData
    {
         /// <summary>
        /// Initializes a new instance of the <see cref="OracleDatabase"/> class with a connection string and a configuration
        /// source.
        ///</summary>
        ///<param name="connectionStringSettings">The <see cref="ConnectionStringSettings"/> for the represented database.</param>
        ///<param name="configurationSource">The <see cref="IConfigurationSource"/> from which additional information can 
        /// be retrieved if necessary.</param>
        public OracleDatabaseData(ConnectionStringSettings connectionStringSettings,
                                 IConfigurationSource configurationSource)
            : base(connectionStringSettings, configurationSource)
        {
            var settings = (OracleConnectionSettings)
                           configurationSource.GetSection(OracleConnectionSettings.SectionName);

            if (settings != null)
            {
                ConnectionData = settings.OracleConnectionsData.Get(connectionStringSettings.Name);
            }
        }

        ///<summary>
        /// Gets the Oracle package mappings for the represented database.
        ///</summary>
        public IEnumerable<OraclePackageData> PackageMappings
        {
            get
            {
                return ConnectionData != null
                           ? (IEnumerable<OraclePackageData>)ConnectionData.Packages
                           : new OraclePackageData[0];
            }
        }
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                NameValueCollection app = System.Configuration.ConfigurationManager.AppSettings;
                String ls_db = app["dbservername"];
                String ls_user = app["predbuser"];
                String ls_password = app["predbpwd"];
                String userName = app["userName"];
                String connectionString = GenConnectString(ls_db, ls_user, ls_password);
                List<User> list_user = new List<User>();
                OracleDatabase db = new OracleDatabase(connectionString);
                #region 获取数据
                using (DbConnection dbConnection = db.CreateConnection())
                {
                    dbConnection.Open();
                    string connStr = @"select dbusername,dbuserpasswd from vi_dbuser";
                    DbCommand cmd = db.GetSqlStringCommand(connStr);
                    //db.AddInParameter(cmd, ":id", DbType.String, "12");

                    #region 12

                    //List<DbParameter> ListPara;
                    //DbParameter para = factory.CreateParameter();
                    //para.ParameterName = "@xm";
                    //para.Size = 10;
                    //para.DbType = DbType.String;
                    //para.Value = "郑成城"; 

                    #endregion

                    using (IDataReader reader = db.ExecuteReader(cmd))
                    {
                        while (reader.Read())
                        {
                            User user = new User();
                            user.UserName = f_passwd2(reader.GetString(0), 2).ToLower().Trim();
                            user.UserPassword = f_passwd2(reader.GetString(1), 2);
                            list_user.Add(user);
                        }
                    }
                }
                #endregion

                foreach (User user in list_user)
                {
                    if (user.UserName == userName)
                    {
                        return GenConnectString(ls_db, user.UserName, user.UserPassword);
                    }
                }
                return "";
         }
        }
        private static string GenConnectString(string as_ds, string as_dbuser, string as_password)
        {
            return "Data Source=" + as_ds + ";User ID=" + as_dbuser + ";Password=" + as_password + ";Pooling=true;";
        }

        #region 加密处理
        #region 加密函数
        /// <summary>
        /// 按固定数\随机数加密或解密
        /// </summary>
        /// <param name="as_pass"></param>
        /// <param name="ai_mod">ai_mod = 0 固定数加密,ai_mod = 1 随机数加密,ai_mod = 2 解密</param>
        /// <returns>加／解密后的字符串</returns>
        public static string f_passwd2(string as_pass, int ai_mod)
        {
            int li_i, li_len, li_pos;
            string ls_cpu = "", ls_tmp = "", ls_ret = "";
            string[,] ls_pass = new string[14, 2];
            if (ai_mod == 0 || ai_mod == 1)
            {
                if (string.IsNullOrEmpty(as_pass)) return "";
                as_pass = f_left(as_pass, 10);
                if (ai_mod == 1)
                {
                    ls_cpu = DateTime.Now.ToString("mmss");
                }
                else
                {
                    ls_cpu = f_left(System.Convert.ToString(System.Convert.ToDouble(f_length(as_pass)) / 0.000412), 4);     //不能改变
                }
                li_len = f_length(as_pass);
                ls_ret = as_pass + ls_cpu;
                for (li_i = 1; li_i <= 4; li_i++)
                {
                    li_pos = System.Convert.ToInt32(f_mid(ls_cpu, li_i, 1));
                    li_pos = li_pos % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_tmp = ls_tmp + f_mid(ls_ret, li_pos, 1);
                    ls_pass[li_pos - 1, 1] = "1";
                    li_pos = (9 - li_pos) % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_pass[li_pos - 1, 1] = "1";
                    ls_tmp = ls_tmp + f_mid(ls_ret, li_pos, 1);
                }
                for (li_i = 1; li_i <= li_len + 4; li_i++)
                {
                    if (ls_pass[li_i - 1, 1] == "1") continue;
                    ls_tmp = ls_tmp + f_mid(ls_ret, li_i, 1);
                }
                ls_ret = System.Convert.ToString(li_len - 1) + ls_cpu + ls_tmp;
                return ls_ret;
            }
            else if (ai_mod == 2)
            {
                if (string.IsNullOrEmpty(as_pass)) return "";
                li_len = System.Convert.ToInt32(f_mid(as_pass, 1, 1)) + 1;
                ls_cpu = f_mid(as_pass, 2, 4);
                ls_ret = f_mid(as_pass, 6, f_length(as_pass));
                for (li_i = 1; li_i <= 4; li_i++)
                {
                    li_pos = System.Convert.ToInt32(f_mid(ls_cpu, li_i, 1));
                    li_pos = li_pos % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_pass[li_pos - 1, 1] = "1";
                    ls_pass[li_pos - 1, 0] = f_mid(ls_ret, 2 * li_i - 1, 1);
                    li_pos = (9 - li_pos) % (li_len + 4);
                    li_pos = li_pos % (li_len + 4);
                    if (li_pos == 0) li_pos = 1;
                    ls_pass[li_pos - 1, 1] = "1";
                    ls_pass[li_pos - 1, 0] = f_mid(ls_ret, 2 * li_i, 1);
                }
                li_pos = 9;
                for (li_i = 1; li_i <= li_len; li_i++)
                {
                    if (ls_pass[li_i - 1, 1] == "1")
                    {
                        //null 
                    }
                    else
                    {
                        ls_pass[li_i - 1, 0] = f_mid(ls_ret, li_pos, 1);
                        li_pos++;
                    }
                    ls_tmp = ls_tmp + ls_pass[li_i - 1, 0];
                }
                return ls_tmp;
            }
            else
            {
                return as_pass;
            }
        }
        #endregion
        #region 加密内部处理函数
        /// <summary>
        /// 从指定文字开始取len长的字符串
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="start">开始的位置</param>
        /// <param name="len">指定长度</param>
        /// <returns>字符串</returns>
        public static string f_mid(string yourstring, int start, int len)
        {
            if (len <= 0 || start < 0) return string.Empty;
            if (string.IsNullOrEmpty(yourstring)) return string.Empty;
            if (yourstring.Length < start) return string.Empty;
            if (yourstring.Length <= start + len - 1)
                return yourstring.Substring(start - 1);
            else
                return yourstring.Substring(start - 1, len);
        }

        /// <summary>
        /// 从指定文字开始取len长的字符串
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <param name="start">开始的位置</param>
        /// <param name="len">指定长度</param>
        /// <returns>字符串</returns>
        public static string f_mid(string yourstring, int start)
        {
            int len = yourstring.Length;
            return f_mid(yourstring, start, len);
        }

        public static string f_left(string yourstring, int ai_len)
        {
            if (ai_len <= 0) return string.Empty;
            if (string.IsNullOrEmpty(yourstring)) return string.Empty;
            int length = f_length(yourstring);
            if (length <= ai_len)
                return yourstring;
            else
            {
                int tmp = 0;
                int len = 0;
                int okLen = 0;
                int li_asc;
                string ls_return = string.Empty;
                for (int i = 0; i < length; i++)
                {
                    //获取asc码
                    li_asc = Asc(yourstring.Substring(i, 1));
                    if (li_asc > 127)
                        tmp += 2;
                    else
                        len += 1;
                    okLen += 1;
                    if (tmp + len == ai_len)
                    {
                        ls_return = yourstring.Substring(0, okLen);
                        break;
                    }
                    else if (tmp + len > ai_len)
                    {
                        ls_return = yourstring.Substring(0, okLen - 1);
                        break;
                    }
                }
                return ls_return;
            }
        }

        /// <summary>
        /// 返回字符串的长度（包括汉字），汉字长度为2
        /// </summary>
        /// <param name="yourstring">源字符串</param>
        /// <returns>字符串</returns>
        public static int f_length(string yourstring)
        {
            if (string.IsNullOrEmpty(yourstring)) return 0;
            int len = yourstring.Length;
            byte[] sarr = System.Text.Encoding.Default.GetBytes(yourstring);
            return sarr.Length;
        }
        /// <summary>
        /// 将字符转换为ＡＳＣ码
        /// </summary>
        /// <param name="asciiCode">字符</param>
        /// <returns>对应的ＡＳＣ码</returns>
        public static int Asc(string character)
        {
            if (character.Length == 1)
            {
                byte[] bytes = System.Text.Encoding.Default.GetBytes(character);
                int intAsciiCode = (int)bytes[0];
                return (intAsciiCode);
            }
            else
            {
                throw new Exception("字符的长度不对");
            }

        }
        #endregion
        #endregion
        private OracleConnectionData ConnectionData { get; set; }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the database represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a database.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations()
        {
            yield return new TypeRegistration<Database>(
                () => new OracleDatabase(
                    ConnectionString,
                    from opd in PackageMappings select (IOraclePackage)opd,
                    Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Container.Resolved<IDataInstrumentationProvider>(Name)))
                {
                    Name = Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }

    }

    public class User
    {
        private string _userName;
        private string _userPassword;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }

        public string UserPassword
        {
            get { return _userPassword; }
            set { _userPassword = value; }
        }
    }
}
