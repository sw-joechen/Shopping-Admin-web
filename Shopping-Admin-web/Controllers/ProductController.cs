using Newtonsoft.Json;
using Shopping_Admin_web.Class;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Shopping_Admin_web.Controllers
{
    public class ProductController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// <summary>
        /// 新增商品
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/addProduct")]
        public string AddProduct()
        {
            Result result = new Result(100, "缺少參數");
            var dict = new Dictionary<string, object>();

            if (!Request.Content.IsMimeMultipartContent())
            {
                result.Set(110, "上傳格式錯誤");
                return result.Stringify();
            }

            var httpRequest = HttpContext.Current.Request;

            // 沒上傳圖檔
            if (httpRequest.Files.Count < 1)
            {
                result.Set(111, "未上傳檔案");
                return result.Stringify();
            }

            dict["name"] = httpRequest.Params["name"];
            dict["price"] = httpRequest.Params["price"];
            dict["amount"] = httpRequest.Params["amount"];
            dict["description"] = httpRequest.Params["description"];
            dict["type"] = httpRequest.Params["type"];

            // 檢查參數
            foreach (KeyValuePair<string, object> item in dict)
            {
                if (item.Value == null)
                {
                    result.Set(100, "缺少參數");
                    return result.Stringify();
                }
                else
                {
                    // 檢查價格, 數量 不可為負數
                    if (item.Key == "price" || item.Key == "amount") {
                        if (Convert.ToInt16(item.Value) < 0) {
                            result.Set(114, "價格與數量不可為負");
                            return result.Stringify();
                        }
                    }
                    // 檢查name不可為空
                    if (item.Key == "name" && item.Value.ToString().Length == 0)
                    {
                        result.Set(113, "商品名稱不可為空");
                        return result.Stringify();
                    }
                }
            }

            try
            {
                var postedFile = httpRequest.Files[0];

                // 串上時戳_原始檔名
                var filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{DateTimeOffset.Now.ToUnixTimeSeconds()}_" + postedFile.FileName);
                postedFile.SaveAs(filePath);

                // TODO: 存進庫的路徑疑似有問題
                dict["picture"] = $"/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}";

                Debug.WriteLine($"dict: {JsonConvert.SerializeObject(dict)}");

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    var sqlResponse = 0;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("create_product", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.Parameters.AddWithValue("@name", dict["name"]);
                        cmd.Parameters.AddWithValue("@price", dict["price"]);
                        cmd.Parameters.AddWithValue("@picture", dict["picture"]);
                        cmd.Parameters.AddWithValue("@amount", dict["amount"]);
                        cmd.Parameters.AddWithValue("@description", dict["description"]);
                        cmd.Parameters.AddWithValue("@type", dict["type"]);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            sqlResponse = (int)r["result"];
                        }
                    }

                    if (sqlResponse == 200)
                    {
                        result.Set(200, "success", dict);
                    }
                    else
                    {
                        result.Set(101, "網路錯誤");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return $"exception=> {e}";
            }
            return result.Stringify();
        }

        /// <summary>
        /// 取得商品清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getProductsList")]
        public string GetProductsList([FromBody] Product payload)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(payload));
            Result result = new Result(100, "缺少參數");
            List<Product> productList = new List<Product> { };

            if (payload == null)
            {
                return result.Stringify();
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    string spName = payload.id == null ? "get_productList" : "search_product";
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (payload.id != null) 
                            cmd.Parameters.AddWithValue("@id", payload.id);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                int status = Convert.ToInt16(r["f_enabled"]);
                                productList.Add(new Product
                                {
                                    id = r["f_id"].ToString(),
                                    enabled = Convert.ToBoolean(status),
                                    name = r["f_name"].ToString(),
                                    price = Convert.ToInt16(r["f_price"]),
                                    picture = r["f_picture"].ToString(),
                                    amount = Convert.ToInt16(r["f_amount"]),
                                    description = r["f_description"].ToString(),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                    type = r["f_type"].ToString()
                                });
                            }
                            result.Set(200, "success", productList);
                        }
                        else
                        {
                            result.Set(112, "找不到該商品");
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
        /// 更新商品
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/updateProduct")]
        public string UpdateProduct()
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "缺少參數");

            if (!Request.Content.IsMimeMultipartContent())
            {
                result.Set(110, "上傳格式錯誤");
                return result.Stringify();
            }

            var httpRequest = HttpContext.Current.Request;

            // 沒上傳圖檔
            if (httpRequest.Files.Count < 1)
            {
                result.Set(111, "未上傳檔案");
                return result.Stringify();
            }

            dict["id"] = httpRequest.Params["id"];
            dict["enabled"] = httpRequest.Params["enabled"];
            dict["name"] = httpRequest.Params["name"];
            dict["price"] = httpRequest.Params["price"];
            dict["picture"] = httpRequest.Params["picture"];
            dict["amount"] = httpRequest.Params["amount"];
            dict["description"] = httpRequest.Params["description"];
            dict["type"] = httpRequest.Params["type"];
            Debug.WriteLine("SerializeObject payload=> ", JsonConvert.SerializeObject(dict));

            // 檢查參數
            foreach (KeyValuePair<string, object> item in dict)
            {
                if (item.Value == null)
                {
                    return result.Stringify();
                }
            }

            // 名稱不可為空字串
            if (dict["name"].ToString().Length == 0) {
                result.Set(113, "商品名稱不可為空");
                return result.Stringify();
            }

            // 價格, 數量不可為負數
            if (Convert.ToInt16(dict["price"]) < 0 || Convert.ToInt16(dict["amount"]) < 0)
            {
                result.Set(114, "價格與數量不可為負");
                return result.Stringify();
            }

            // 檢查完參數再寫進庫
            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("update_product", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", dict["id"]);
                        cmd.Parameters.AddWithValue("@enabled", dict["enabled"]);
                        cmd.Parameters.AddWithValue("@name", dict["name"]);
                        cmd.Parameters.AddWithValue("@price", dict["price"]);
                        cmd.Parameters.AddWithValue("@picture", dict["picture"]);
                        cmd.Parameters.AddWithValue("@amount", dict["amount"]);
                        cmd.Parameters.AddWithValue("@description", dict["description"]);
                        cmd.Parameters.AddWithValue("@type", dict["type"]);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.Read())
                        {
                            result.Set(200, "success");
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

        // GET: api/Product/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Product
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Product/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Product/5
        public void Delete(int id)
        {
        }
    }
}
