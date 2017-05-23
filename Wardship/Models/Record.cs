using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Record
    {
        /// <summary>
        ///Unknown values
        /// </summary>

        [Key]
        public int RecordID { get; set; }

        [MaxLength(5), Display(Name = "Record")]
        public string Detail { get; set; }

        public string Description { get; set; }

    }
}