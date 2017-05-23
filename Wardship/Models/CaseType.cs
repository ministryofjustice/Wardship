using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class CaseType
    {
        /// <summary>
        /// Contains the values (W, A) for wardship and abduction
        /// </summary>

        [Key]
        public int CaseTypeID { get; set; }

        [MaxLength(2), Display(Name = "Case Type")]
        public string Detail { get; set; }

        public string Description { get; set; }



    }
}