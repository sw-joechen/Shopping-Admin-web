﻿using Newtonsoft.Json;

namespace Shopping_Admin_web.Class
{
    public class Result
    {
        public int code;
        public string msg;
        public object data = null;

        public Result(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public void Set(int code, string msg, object data = null) {
            this.code = code;
            this.msg = msg;
            this.data = data;
        }
        public string Stringify()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}