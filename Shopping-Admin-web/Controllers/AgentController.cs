using Newtonsoft.Json;
using Shopping_Admin_web.Class;
using Shopping_Admin_web.PwdHasher;
using Shopping_Admin_web.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web.Http;

namespace Shopping_Admin_web.Controllers
{
    public class AgentController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// <summary>
        /// 註冊管理帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/registerAgent")]
        public string RegisterAgent([FromBody] Agent payload)
        {
            Result result = new Result(100, "params required");
            if (payload == null)
            {
                return result.Stringify();
            }

            if (payload.account.Length == 0 && payload.pwd.Length == 0)
            {
                result.Set(102, "empty account or pwd");
                return result.Stringify();
            }

            AccountValidator accountValidator = new AccountValidator(payload.account);
            PwdValidator pwdValidator = new PwdValidator(payload.pwd);

            if (accountValidator.IsAccountValid() && pwdValidator.IsPwdValid())
            {
                try {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        // Hash password
                        string hashedPwd = SecurePasswordHasher.Hash(payload.pwd);

                        int sqlResponse = 0;

                        // 開啟連線
                        conn.Open();

                        using (SqlCommand cmd = new SqlCommand("register_t_agent", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@account", payload.account);
                            cmd.Parameters.AddWithValue("@pwd", hashedPwd);
                            SqlDataReader r = cmd.ExecuteReader();
                            if (r.Read())
                            {
                                sqlResponse = (int)r["result"];
                            }
                            // 關閉連線
                            conn.Close();
                        }

                        if (sqlResponse == 200) {
                            result.Set(200, "success", new { payload.account});
                        } 
                        else
                        {
                            result.Set(101, "網路錯誤");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ex: {ex}");
                    result.Set(101, "網路錯誤");
                }                
            }
            else
            {
                result.Set(103, "account, pwd not valid");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 登入後台帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/loginAgent")]
        public string LoginAgent([FromBody] Agent payload)
        {
            Result result = new Result(100, "fail");
            if (payload == null)
            {
                result.Set(100, "params is required");
                return result.Stringify();
            }
            try {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("login_t_agents", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", payload.account);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            // Verify
                            bool verify = SecurePasswordHasher.Verify(payload.pwd, r["f_pwd"].ToString());

                            if (verify)
                            {
                                object obj = new
                                {
                                    account = r["f_account"],
                                    role = r["f_role"],
                                };
                                result.Set(200, "success", obj);
                            }
                            else
                            {
                                result.Set(104, "wrong password");
                            }
                        }
                        else
                        {
                            result.Set(105, "member not found");
                        }
                        // 關閉連線
                        conn.Close();
                    }
                }
            }
            catch (Exception ex) {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }

            return result.Stringify();
        }

        /// <summary>
        /// 更新後台帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/updateAgent")]
        public string UpdateAgent([FromBody] AgentUpdate payload)
        {
            Result result = new Result(100, "fail");
            //Debug.WriteLine("SerializeObject payload=> ", JsonConvert.SerializeObject(payload));
            if (payload == null)
            {
                result.Set(100, "params are required");
                return result.Stringify();
            }

            if (payload.account.Length == 0 )
            {
                result.Set(106, "帳號不可為空字串");
                return result.Stringify();
            }

            // 檢查完參數再進到db
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("update_t_agents", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", payload.account);
                        cmd.Parameters.AddWithValue("@enabled", payload.enabled);
                        cmd.Parameters.AddWithValue("@role", payload.role);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.Read())
                        {
                            object obj = new
                            {
                                account = r["f_account"],
                                role = r["f_role"],
                                enabled = r["f_enabled"]
                            };
                            result.Set(200, "success", obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }          
            return result.Stringify();
        }

        /// <summary>
        /// 取得後台帳號清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getAgentsList")]
        public string GetAgentsList([FromBody] GetAgent payload)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(payload));
            Result result = new Result(100, "fail");
            // 搜尋指定agent
            if (payload != null && payload.account!= null && payload.account.Length !=0)
            {
                List<SearchAgent> searchAgentList = new List<SearchAgent> { };
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("search_agent", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@account", payload.account);
                            SqlDataReader r = cmd.ExecuteReader();

                            if (r.HasRows)
                            {
                                while (r.Read())
                                {
                                    searchAgentList.Add(new SearchAgent
                                    {
                                        id = r["f_id"].ToString(),
                                        account = r["f_account"].ToString(),
                                        enabled = Convert.ToInt16(r["f_enabled"]),
                                        createdDate = r["f_createdDate"].ToString(),
                                        updatedDate = r["f_updatedDate"].ToString(),
                                        role = r["f_role"].ToString(),
                                        pwd = r["f_pwd"].ToString()
                                    });
                                }
                                result.Set(200, "success", searchAgentList);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ex: {ex}");
                    result.Set(101, "網路錯誤");
                }
            }
            else
            {
                List<AgentsList> agentList = new List<AgentsList> { };
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectString))
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("get_agentList", conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataReader r = cmd.ExecuteReader();

                            if (r.HasRows)
                            {
                                while (r.Read())
                                {
                                    agentList.Add(new AgentsList
                                    {
                                        id = r["f_id"].ToString(),
                                        account = r["f_account"].ToString(),
                                        enabled = Convert.ToInt16(r["f_enabled"]),
                                        createdDate = r["f_createdDate"].ToString(),
                                        updatedDate = r["f_updatedDate"].ToString(),
                                        role = r["f_role"].ToString()
                                    });
                                }
                                result.Set(200, "success", agentList);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"ex: {ex}");
                    result.Set(100, "網路錯誤");
                }

            }

            return result.Stringify();

        }
    }
}