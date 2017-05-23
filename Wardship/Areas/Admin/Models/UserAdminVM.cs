using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wardship.Models;
using System.Web.Mvc;

namespace Wardship.Areas.Admin.Models
{
    public class UserAdminVM
    {
        public User User { get; set; }
        public SelectList Roles { get; set; }
    }
}