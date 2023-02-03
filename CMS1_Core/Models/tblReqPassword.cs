using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS1_Core.Models
{
    public class tblReqPassword
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string OldPass { get; set; }
        public string NewPass { get; set; }

    }
}