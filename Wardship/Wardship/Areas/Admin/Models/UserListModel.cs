using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wardship.Models;

namespace Wardship.Areas.Admin.Models
{
    public class UserList
    {
        public IEnumerable<ADGroup> Groups { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}