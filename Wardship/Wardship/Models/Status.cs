using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Status
    {
        /// <summary>
        /// Contains the values (Dismissed item?,,,,,,,)
        /// </summary>

        [Key]
        public int StatusID { get; set; }

        [MaxLength(15), Display(Name = "Status")]
        public string Detail { get; set; }

  

    }
}