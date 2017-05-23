using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class CAFCASS
    {
        /// <summary>
        /// Contains the values (Official solicitor,,,,,,,,,)
        /// </summary>

        [Key]
        public int CAFCASSID { get; set; }

        [MaxLength(10), Display(Name = "CAFCASS")]
        public string Detail { get; set; }

        public string Description { get; set; }

    }
}