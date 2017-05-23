using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Gender
    { /// <summary>
        /// Contains the values (M,F,,,,,,,,)
        /// </summary>

        [Key]
        public int GenderID { get; set; }

        [MaxLength(1), Display(Name = "Gender")]
        public string Detail { get; set; }

       

    }
}