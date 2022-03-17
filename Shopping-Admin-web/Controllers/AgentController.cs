﻿using Newtonsoft.Json;
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
            var dict = new Dictionary<string, object>();
            if (payload == null)
            {
                result.Set(100, "params is required");
                return result.Stringify();
            }

            // 進庫撈使用者
            try {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("search_agent", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", payload.account);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            dict["id"] = r["f_id"];
                            dict["account"] = r["f_account"];
                            dict["pwd"] = r["f_pwd"];
                            dict["enabled"] = r["f_enabled"];
                            dict["createdDate"] = r["f_createdDate"];
                            dict["updatedDate"] = r["f_updatedDate"];
                            dict["role"] = r["f_role"];
                            dict["count"] = r["f_count"];
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

            // check status
            if (Convert.ToInt16(dict["enabled"]) == 0)
            {
                result.Set(108, "the account is disabled");
                return result.Stringify();
            }

            // check >= 3
            if (Convert.ToInt16(dict["count"]) >= 3) {
                result.Set(107, "error count meets the limit", new { count = dict["count"] });
            }
            else
            {
                //check pwd
                bool verify = SecurePasswordHasher.Verify(payload.pwd, dict["pwd"].ToString());
                if (verify) {
                    result.Set(200, "success", dict);
                }
                else
                {
                    // 密碼錯誤 count+1後寫進庫
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectString))
                        {
                            conn.Open();
                            using (SqlCommand cmd = new SqlCommand("update_t_agents", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@account", dict["account"]);
                                cmd.Parameters.AddWithValue("@enabled", dict["enabled"]);
                                cmd.Parameters.AddWithValue("@role", dict["role"]);
                                cmd.Parameters.AddWithValue("@count", Convert.ToInt16(dict["count"]) + 1);
                                SqlDataReader r = cmd.ExecuteReader();
                                if (r.Read())
                                {
                                    // 回傳錯誤次數
                                    result.Set(104, "wrong password", new { count = dict["count"] });
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
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ex: {ex}");
                        result.Set(101, "網路錯誤");
                    }
                }
            }
            Debug.WriteLine($"checkpointB=> {JsonConvert.SerializeObject(dict)}");

            
            return result.Stringify();
        }

        /// <summary>
        /// 解鎖後台帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/unLockAgent")]
        public string UnLockAgent([FromBody] GetAgent payload)
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "fail");
            //Debug.WriteLine("SerializeObject payload=> ", JsonConvert.SerializeObject(payload));
            if (payload == null)
            {
                result.Set(100, "params are required");
                return result.Stringify();
            }

            if (payload.account.Length == 0)
            {
                result.Set(106, "帳號不可為空字串");
                return result.Stringify();
            }

            // 檢查完參數再進庫撈使用者
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
                        if (r.Read())
                        {
                            dict["id"] = r["f_id"];
                            dict["account"] = r["f_account"];
                            dict["pwd"] = r["f_pwd"];
                            dict["enabled"] = r["f_enabled"];
                            dict["createdDate"] = r["f_createdDate"];
                            dict["updatedDate"] = r["f_updatedDate"];
                            dict["role"] = r["f_role"];
                            dict["count"] = r["f_count"];
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
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }

            // 更新f_count為0
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("update_t_agents", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", dict["account"]);
                        cmd.Parameters.AddWithValue("@enabled", dict["enabled"]);
                        cmd.Parameters.AddWithValue("@role", dict["role"]);
                        cmd.Parameters.AddWithValue("@count", 0);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.Read())
                        {
                            dict["count"] = r["f_count"];
                            result.Set(200, "success", new { count = dict["count"] });
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