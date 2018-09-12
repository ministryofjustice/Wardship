using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Security;


namespace Wardship.Models
{
    public class QuickSearch : ListViewModel
    {

        // applicant address details 
        #region Applicant Search Details

        [Display(Name = "Applicant Name")]
        [MaxLength(8), DataType(DataType.Text)]
        public string ApplicantName { get; set; }

        [Display(Name = "Address Line 1")]
        [MaxLength(100), DataType(DataType.Text)]
        public string ApplicantAddr1 { get; set; }

        [Display(Name = "Address Line 2")]
        [MaxLength(100), DataType(DataType.Text)]
        public string ApplicantAddr2 { get; set; }

        [Display(Name = "Address Line 3")]
        [MaxLength(100), DataType(DataType.Text)]
        public string ApplicantAddr3 { get; set; }

        [Display(Name = "Address Line 4")]
        [MaxLength(100), DataType(DataType.Text)]
        public string ApplicantAddr4 { get; set; }

        [Display(Name = "Postcode")]
        [MaxLength(8), DataType(DataType.Text)]
        public string ApplicantPostcode { get; set; }

        #endregion

        //builds the address with out spaces
        public virtual List<string> populatedLines
        {
            get
            {
                List<string> outputAddress = new List<string>();
                outputAddress.Add(ApplicantName);
                outputAddress.Add(ApplicantAddr1);
                if (ApplicantAddr2 != null) outputAddress.Add(ApplicantAddr2);
                if (ApplicantAddr3 != null) outputAddress.Add(ApplicantAddr3);
                if (ApplicantAddr4 != null) outputAddress.Add(ApplicantAddr4);
                if (ApplicantPostcode != null) outputAddress.Add(ApplicantPostcode);

                return outputAddress;
            }
        }

        //Security element replaces invalid xml with equivilant when using the address block above
        public virtual string printAddressMultiLine
        {
            get
            {
                List<string> PopulateLines = new List<string>();
                foreach (var line in populatedLines)
                {
                    PopulateLines.Add(SecurityElement.Escape(line));
                }
                return string.Join("<w:br/>", PopulateLines.ToArray());
            }
        }





//search details 



        //Key](NONE)
        public int WardshipCaseID { get; set; }

        [Display(Name = "File Number")]
        [DataType(DataType.Text)]
        public string FileNumber { get; set; }

        [Display(Name = "Child Surname")]
        [DataType(DataType.Text)]
        [MaxLength(100)]
        public string ChildSurname { get; set; }

        [Display(Name = "Child Forename")]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        public string ChildForenames { get; set; }


        public virtual string ChildOutputName // Full Name 
        {
            get { return string.Format("{0} {1}", ChildForenames, ChildSurname); }
        }


        [Display(Name = "Child Date Of Birth")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "0:d", ApplyFormatInEditMode = true), UIHint("DatePicker")]
        public DateTime? ChildDateofBirth { get; set; }

  

        public Boolean isValid()
        {
            //check each variable for validity, if not valid - set result to true
            if (FileNumber != null) { return true; }
            if (ChildSurname != null) { return true; }
            if (ChildForenames != null) { return true; }
            if (ChildDateofBirth != null) { return true; }
          

            //if no criteria is valid, return false as default
            return false;
        }

         public IPagedList<Wardship.Models.WardshipRecord> results { get; set; }
        // results object for a list of data which we will send to my controllor and view






    }







}