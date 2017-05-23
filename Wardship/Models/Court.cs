using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Security;
using System.Web.Mvc;

namespace Wardship.Models
{
    public class Court
    {
        [Key]
        public int CourtID { get; set; }
        [Required, MaxLength(100), Display(Name="Court name")]
        public string CourtName { get; set; }
        [MaxLength(50), Display(Name="Address Line 1")]
        public string AddressLine1 { get; set; }
        [MaxLength(50), Display(Name="Address Line 2")]
        public string AddressLine2 { get; set; }
        [MaxLength(50), Display(Name="Address Line 3")]
        public string AddressLine3 { get; set; }
        [MaxLength(50), Display(Name="Address Line 4")]
        public string AddressLine4 { get; set; }
        [MaxLength(30), Display(Name="Town")]
        public string Town { get; set; }
        [MaxLength(30), Display(Name="County")]
        public string County { get; set; }
        [MaxLength(20), Display(Name="Country")]
        public string Country { get; set; }
        [MaxLength(8), Display(Name="Postcode")]
        public string Postcode { get; set; }
        [MaxLength(60), Display(Name="DX Address")]
        public string DX { get; set; }
        [Display(Name = "Active")]
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
      
        
        
        /// <summary>
        /// AddressSingleLine returns all populated address lines separated with a comma
        /// </summary>
        [Display(Name = "Address")]
        public virtual string AddressSingleLine
        {
            get
            {
                List<string> popLines = populatedLines;
                string result = string.Join(", ", popLines.ToArray());
                return result;
            }
        }
        
        /// <summary>
        /// ScreenAddressMultiLine return all populated address lines separated with &lt;br/> tags and should be displayed on screen 
        /// <br /> with @Html.Raw(item.ScreenAddressMultiLine) NOT @Html.DisplayFor(modelItem=>item.ScreenAddressMultiLine)
        /// </summary>
        [Display(Name = "Address")]
        public virtual string ScreenAddressMultiLine
        {
            get
            {
                List<string> popLines = populatedLines;
                return string.Join("<br />", popLines.ToArray());
            }
        }

        /// <summary>
        /// PrintAddressMultiLine return all populated address lines separated with &lt;w:br/> tags for export to XML documents
        /// </summary>
        [Display(Name = "Address")]
        public virtual string PrintAddressMultiLine
        {
            get
            {
                List<string> popLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    popLines.Add(SecurityElement.Escape(line));
                }
                return string.Join("<w:br/>", popLines.ToArray());
            }
        }

        private List<string> populatedLines
        {
            get
            {
                List<string> outputAddress = new List<string>();
                outputAddress.Add(AddressLine1);
                if (AddressLine2 != null) outputAddress.Add(AddressLine2);
                if (AddressLine3 != null) outputAddress.Add(AddressLine3);
                if (AddressLine4 != null) outputAddress.Add(AddressLine4);
                if (Town != null) outputAddress.Add(Town);
                if (County != null) outputAddress.Add(County);
                if (Country != null) outputAddress.Add(Country);
                if (Postcode != null) outputAddress.Add(Postcode);
                return outputAddress;
            }
        }
  
    
    
    
    
    }
    public class DeleteCourtVM
    {
        public int CourtID { get; set; }
        [Display(Name = "Reason for Deletion")]
        public int deletedReasonID { get; set; }
        public SelectList DeletedReasonList { get; set; }
        public string DisplayIdentifier { get; set; }



        public int WardshipRecordID { get; set; }
        public virtual WardshipRecord WardshipRecord { get; set; }


    }
}