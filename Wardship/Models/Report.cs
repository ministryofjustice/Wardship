using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class Report
    {
        public List<WardshipRecord> WardshipRecordsList { get; set; }

        [Display(Name = "Begin Report From")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Begin date is required")]
        [DataType(DataType.Date)]
        public DateTime ReportBegin { get; set; }

        [Display(Name = "End Report On")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime ReportFinal { get; set; }

        public bool IsValidDateRange()
        {
            return ReportBegin <= ReportFinal;
        }
    }
}