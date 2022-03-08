using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Shopping_Admin_web.Class
{
    public class Result
    {
        // TODO: private造成序列化出不來
        public int code;
        public string msg;
        public object data;

        public Result(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        public void set(int code, string msg, [Optional] object data) {
            this.code = code;
            this.msg = msg;
            this.data = data ?? null;
        }

        //public bool shouldserializecode() {
        //    return false;
        //}

        //public bool shouldserializemsg() { 
        //return true;
        //}
    }
}