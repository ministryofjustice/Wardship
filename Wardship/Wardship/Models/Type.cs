using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Type
    {
        /// <summary>
        /// At present we do not know what these values are
        /// </summary>

        [Key]
        public int TypeID { get; set; }

        [MaxLength(20), Display(Name = "Type")]
        public string Detail { get; set; }
        public string Description { get; set; }



    }
}