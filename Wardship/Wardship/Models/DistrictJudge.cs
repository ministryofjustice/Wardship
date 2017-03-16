using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class DistrictJudge
    {

        /// <summary>
        /// Contains the Names of the DistrictJudge
        /// </summary>

        [Key]
        public int DistrictJudgeID { get; set; }

        [Display(Name = "Name")] //District Judge
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string Name { get; set; }

       


    }
}