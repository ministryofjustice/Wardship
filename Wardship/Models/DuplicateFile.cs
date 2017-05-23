using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class DuplicateFile
    {
        /// <summary>
        /// Contains the values (Y, N) for Yes and No
        /// </summary>

        [Key]
        public int DuplicateFileID { get; set; }

        [MaxLength(1), Display(Name = "Duplicate File")]
        public string Detail { get; set; }

        public string Description { get; set; }
    }
}