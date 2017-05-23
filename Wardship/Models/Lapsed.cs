using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Lapsed
    {
        /// <summary>
        /// Contains the values (Y, N) for wardship and abduction
        /// </summary>

        [Key]
        public int LapsedID { get; set; }

        [MaxLength(3), Display(Name = "Case Lapsed")]
        public string Detail { get; set; }


    }
}