using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KOL_HoangThuyLinh.Models
{
    public class tblGalleryDT
    {
        public int Id { get; set; }
        public int IdGallery { get; set; }
        public string Avatar { get; set; }
        public DateTime CreateDate { get; set; }
        public string Title { get; set; }
    }
}