using Newtonsoft.Json;
using Shopping_Admin_web.Class;
using Shopping_Admin_web.PwdHasher;
using Shopping_Admin_web.Validators;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shopping_Admin_web.Controllers
{
    public class AgentController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// 註冊管理帳號
        [HttpPost]
        [Route("api/{controller}/registerAgent")]
        public string RegisterAgent([FromBody] Agent payload)
        {
            Result result = new Result(100, "fail");
            if (payload == null)
            {
                return JsonConvert.SerializeObject(result);
            }

            // TODO: 以下的商業邏輯要拆分去哪裡?
            AccountValidator accountValidator = new AccountValidator(payload.account);
            PwdValidator pwdValidator = new PwdValidator(payload.pwd);

            if (accountValidator.IsAccountValid() && pwdValidator.IsPwdValid())
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    // Hash password
                    string hashedPwd = SecurePasswordHasher.Hash(payload.pwd);

                    // 開啟連線
                    conn.Open();

                    // 從庫撈資料出來檢查帳號是否重複創建
                    SqlCommand cmd = new SqlCommand($"SELECT f_account, f_enabled, f_pwd, f_createdDate, f_updatedDate, f_role FROM t_agents WHERE f_account = '{payload.account}'", conn);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // 從庫撈到資料代表帳號重複
                            result.set(100, "fail");
                        }
                        else
                        {   
                            // 創新帳號寫進庫
                            reader.Close();

                            cmd = new SqlCommand($"INSERT INTO t_agents(f_account, f_pwd) VALUES('{payload.account}', '{hashedPwd}');", conn);

                            cmd.ExecuteNonQuery();

                            result.set(200, "success");
                        }
                    }

                    // 關閉連線
                    conn.Close();
                }
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        // 登入後台帳號
        [HttpPost]
        [Route("api/{controller}/loginAgent")]
        public string LoginAgent([FromBody] Agent payload)
        {
            Result result = new Result(100, "fail");
            if (payload == null)
            {
                result.set(100, "params is required");
                return JsonConvert.SerializeObject(result);
            }

            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT f_account, f_enabled, f_pwd, f_createdDate, f_updatedDate, f_role FROM t_agents WHERE f_account = '{payload.account}'", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Verify
                        bool verify = SecurePasswordHasher.Verify(payload.pwd, reader["f_pwd"].ToString());

                        if (verify)
                        {
                            object obj = new {
                                account = reader["f_account"],
                                role = reader["f_role"],
                            };
                            result.set(200, "success", obj);
                        }
                        else
                        {
                            result.set(100, "wrong password");
                        }
                    }
                    else
                    {
                        result.set(100, "member not found");
                    }
                    conn.Close();
                }
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        // 取得後台帳號清單
        [HttpPost]
        [Route("api/{controller}/getAgentsList")]
        public string GetAgentsList()
        {
            Result result = new Result(100, "fail");
            List<AgentsList> agentList = new List<AgentsList> { };

            using (SqlConnection conn = new SqlConnection(connectString))
            using (SqlCommand cmd = new SqlCommand($"SELECT f_account, f_enabled, f_pwd, f_createdDate, f_updatedDate, f_role FROM t_agents", conn))
            {
                conn.Open();
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.HasRows)
                    {
                        while (r.Read())
                        {
                            agentList.Add(new AgentsList
                            {
                                account = r["f_account"].ToString(),
                                enabled = Convert.ToInt16(r["f_enabled"]),
                                createdDate = r["f_createdDate"].ToString(),
                                updatedDate = r["f_updatedDate"].ToString(),
                                role = r["f_role"].ToString()
                            });
                            //Debug.WriteLine($"{r["f_account"]}, {r["f_enabled"]}, {r["f_pwd"]}, {r["f_createdDate"]}");
                        }
                        result.set(200, "success", agentList);
                    }
                }
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }
    }
}