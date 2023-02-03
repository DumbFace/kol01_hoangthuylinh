using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KOL_HoangThuyLinh.Models
{
    public class tblHome
    {
        public int Id { get; set; }
        public int IdZone { get; set; }
        public string Title { get; set; }
        public string UrlLink { get; set; }
        public string CategoryName { get; set; }
        public string UserName { get; set; }
        public string Teaser { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Avatar2x1 { get; set; }
        public string Avatar1x2 { get; set; }
        public string Avatar1x1 { get; set; }
        public string AvatarThumb { get; set; }
        public string Body { get; set; }
        public string ZoneName { get; set; }
        public int IdType { get; set; }

    }
}