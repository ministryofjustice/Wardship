using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class CWO
    {
        /// <summary>
        /// Contains the values (Yes,No)
        /// </summary>

        [Key]
        public int CWOID { get; set; }

        [MaxLength(15), Display(Name = "C.W.O.Involved")]
        public string Detail { get; set; }



    }
}