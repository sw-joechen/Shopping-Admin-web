using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping_Admin_web.Class
{
    public class SearchAgent
    {
        public string id { get; set; }
        public string account { get; set; }
        public int enabled { get; set; }
        public string createdDate { get; set; }
        public string updatedDate { get; set; }
        public string role { get; set; }
        public string pwd { get; set; }
        public string count { get; set; }
    }
}