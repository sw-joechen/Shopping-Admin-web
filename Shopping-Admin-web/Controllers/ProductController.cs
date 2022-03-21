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

            try
            {
                string name = httpRequest.Params["name"];
                string price = httpRequest.Params["price"];
                string amount = httpRequest.Params["amount"];
                string description = httpRequest.Params["description"];
                string type = httpRequest.Params["type"];
                string picture;

                Debug.WriteLine($"name=> {name}");
                Debug.WriteLine($"price=> {price}");
                Debug.WriteLine($"amount=> {amount}");
                Debug.WriteLine($"description=> {description}");
                Debug.WriteLine($"type=> {type}");

                // 檢查價格不可為負數
                if (Convert.ToInt32(price) < 0)
                {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 檢查數量不可為負數
                if (Convert.ToInt32(amount) < 0)
                {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 名稱不可為空字串
                if (name.Length == 0)
                {
                    result.Set(113, "商品名稱不可為空");
                    return result.Stringify();
                }

                var postedFile = httpRequest.Files[0];

                // 串上時戳_原始檔名
                var filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}");
                postedFile.SaveAs(filePath);

                picture = $"/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}";                

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    var sqlResponse = 0;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_addProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@picture", picture);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@type", type);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            sqlResponse = (int)r["result"];
                        }
                    }

                    if (sqlResponse == 200)
                    {
                        result.Set(200, "success");
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
            string spName = payload == null ? "pro_saw_getProductList" : "pro_saw_getProductByID";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (payload != null && payload.id != null) 
                            cmd.Parameters.AddWithValue("@id", payload.id);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                int status = Convert.ToInt16(r["f_enabled"]);
                                var httpRequest = HttpContext.Current.Request;
                                productList.Add(new Product
                                {
                                    id = r["f_id"].ToString(),
                                    name = r["f_name"].ToString(),
                                    description = r["f_description"].ToString(),
                                    price = Convert.ToInt16(r["f_price"]),
                                    picture = $"{httpRequest.Url.Scheme}://{httpRequest.Url.Authority}/Uploads{r["f_picture"].ToString()}",
                                    amount = Convert.ToInt16(r["f_amount"]),
                                    type = r["f_type"].ToString(),
                                    enabled = Convert.ToBoolean(status),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
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
            Result result = new Result(100, "缺少參數");

            if (!Request.Content.IsMimeMultipartContent())
            {
                result.Set(110, "上傳格式錯誤");
                return result.Stringify();
            }

            try
            {
                var httpRequest = HttpContext.Current.Request;

                int enabled = Convert.ToInt32(Convert.ToBoolean(httpRequest.Params["enabled"]));
                int id = Convert.ToInt32(httpRequest.Params["id"]);
                string name = httpRequest.Params["name"];
                string price = httpRequest.Params["price"];
                string amount = httpRequest.Params["amount"];
                string description = httpRequest.Params["description"];
                string type = httpRequest.Params["type"];
                string picture = $"/{Path.GetFileName(httpRequest.Params["picture"])}";

                Debug.WriteLine($"id=> {id}");
                Debug.WriteLine($"enabled=> {enabled}");
                Debug.WriteLine($"name=> {name}");
                Debug.WriteLine($"price=> {price}");
                Debug.WriteLine($"amount=> {amount}");
                Debug.WriteLine($"description=> {description}");
                Debug.WriteLine($"type=> {type}");
                Debug.WriteLine($"picture=> {picture}");

                // TODO: 改用變數存的話 怎麼檢查null, 只帶key沒帶value的情形
                // 檢查價格不可為負數
                if (Convert.ToInt32(price) < 0)
                {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 檢查數量不可為負數
                if (Convert.ToInt32(amount) < 0)
                {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 名稱不可為空字串
                if (name.Length == 0) {
                    result.Set(113, "商品名稱不可為空");
                    return result.Stringify();
                }

                if (httpRequest.Files.Count >= 1)
                {
                    var postedFile = httpRequest.Files[0];
                    Debug.WriteLine($"postedFile: {postedFile.FileName}");

                    // 串上時戳_原始檔名
                    var filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}");
                    postedFile.SaveAs(filePath);

                    picture = $"/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}";
                }

                // 檢查完參數再寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_editProduct", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@enabled", enabled);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@picture", picture);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@type", type);
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
