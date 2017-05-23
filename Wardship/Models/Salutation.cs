using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Salutation
    {
        [Key]
        public int SalutationID { get; set; }

        [Required, MaxLength(10), Display(Name = "Title")]
        public string Detail { get; set; }
        [Display(Name = "Active")]
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }
}