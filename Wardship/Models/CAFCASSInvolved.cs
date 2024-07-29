using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class CAFCASSInvolved
    {
        /// <summary>
        /// Contains the values (Yes,No)
        /// </summary>

        [Key]
        public int CAFCASSInvolvedID { get; set; }

        [MaxLength(15), Display(Name = "CAFCASS Involved")]
        public string Detail { get; set; }



    }
}