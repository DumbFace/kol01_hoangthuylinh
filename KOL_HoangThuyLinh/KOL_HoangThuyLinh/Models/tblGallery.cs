using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KOL_HoangThuyLinh.Models
{
    public class tblGallery
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreateDate { get; set; }
        public string AvatarCover { get; set; }
        public List<tblGalleryDT> Albums { get; set; }
    }
}