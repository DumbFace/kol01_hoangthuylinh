﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KOL_HoangThuyLinh.Models
{
    public class tblCategory
    {
        public int Id { get; set; }
        public int IdZone { get; set; }
        public string ZoneName { get; set; }
        public string CategoryName { get; set; }
    }
}