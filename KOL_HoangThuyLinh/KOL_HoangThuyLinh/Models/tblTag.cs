﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KOL_HoangThuyLinh.Models
{
    public class tblTag
    {
        public int Id { get; set; }
        public string  TagName{ get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}