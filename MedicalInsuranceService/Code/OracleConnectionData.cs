﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace MedicalInsuranceService.Code
{
    public class OracleConnectionData
    {
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
                using (OracleConnection conn = new OracleConnection(connectionString))
                {
                    conn.Open();
                    string commandStr = @"select dbusername,dbuserpasswd from vi_dbuser";
                    OracleCommand command = new OracleCommand(commandStr);
                    using (var reader = command.ExecuteReader())
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