using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS1_Core.Models
{
    public class tblUser
    {
        private string _Role;
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int IdRole { get; set; }
        public int IsDelete { get; set; }
        public string Role
        {
            get => IdRole == 1 ? "Admin" : "User";
            set { _Role = value; }
        }
    }
}