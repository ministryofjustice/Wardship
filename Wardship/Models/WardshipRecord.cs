﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;

namespace Wardship.Models
{
    public class WardshipRecord
    {
        [Key]
        public int WardshipCaseID { get; set; }

        //Minor Information 
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
        [DataType(DataType.Date)]
        public DateTime? ChildDateofBirth { get; set; }

        [Display(Name = "Date Issued")]
        [DataType(DataType.Date)]
        public DateTime? DateOfOS { get; set; }

        [MaxLength(15), Display(Name = "File Number")]
        [DataType(DataType.Text)]
        public string FileNumber { get; set; }

        [MaxLength(10), Display(Name = "File Duplicate")]
        [DataType(DataType.Text)]
        public string FileDuplicate { get; set; }

        [MaxLength(150), Display(Name = "Xreg")]  // free text? ask daniel
        [DataType(DataType.Text)]
        public string Xreg { get; set; }


        [Display(Name = "Type")]
        public int? TypeID { get; set; } //used for setting the relationship in the DB
        public virtual Type Type { get; set; }//used for the status dropdown

        [Display(Name = "Court")]
        public int? CourtID { get; set; } //used for setting the relationship in the DB
        public virtual Court Court { get; set; }//used for the status dropdown

        [Display(Name = "Status")]
        public int? StatusID { get; set; } //used for setting the relationship in the DB
        public virtual Status Status { get; set; }//used for the status dropdown

        [Display(Name = "Gender")]
        public int? GenderID { get; set; } //used for setting the relationship in the DB
        public virtual Gender Gender { get; set; }//used for the status dropdown

        [Display(Name = "Record")]
        public int? RecordID { get; set; } //used for setting the relationship in the DB
        public virtual Record Record { get; set; }//used for the status dropdown

        [Display(Name = "Lapsed")]
        public int? LapsedID { get; set; } //used for setting the relationship in the DB
        public virtual Lapsed Lapsed { get; set; }//used for the status dropdown

        [Display(Name = "CWO")]
        public int? CWOID { get; set; } //used for setting the relationship in the DB
        public virtual CWO CWO { get; set; }//used for the status dropdown

        [Display(Name = "DistrictJudge")]
        public int? DistrictJudgeID { get; set; } //used for setting the relationship in the DB
        public virtual DistrictJudge DistrictJudge { get; set; }//used for the status dropdown

        [Display(Name = "CaseType")]
        public int? CaseTypeID { get; set; } //used for setting the relationship in the DB
        public virtual CaseType CaseType { get; set; }//used for the status dropdown

        [Display(Name = "CAFCASS")]
        public int? CAFCASSID { get; set; } //used for setting the relationship in the DB
        public virtual CAFCASS CAFCASS { get; set; }//used for the status dropdown


        [Display(Name = "Lapse Letter Sent")]
        [DataType(DataType.Date)]
        public DateTime? LapseLetterSent { get; set; }

        [Display(Name = "First Appointment Date")]
        [DataType(DataType.Date)]
        public DateTime? FirstAppointmentDate { get; set; }

        [Display(Name = "Hearing Date")]
        [DataType(DataType.Date)]
        public DateTime? HearingDate { get; set; }



        //Username
        public string Username { get; set; }
    }



    public class WardshipRecordVM
    {
        public int WardshipCaseID { get; set; }
        public WardshipRecord WardshipRecord { get; set; }
        public CaseType CaseType { get; set; } // A collection of CaseType's dropdown, etc.
        public Court Locations { get; set; }
        public Type Types { get; set; }
        public Gender Gender { get; set; }
        public DistrictJudge DistrictJudge { get; set; }
        public Record Record { get; set; }
        public Lapsed Lapsed { get; set; }
        public Status Status { get; set; }
        public CWO CWO { get; set; }
        public CAFCASS CAFCASS { get; set; }

    }



    public class WardshipRecordVMlistView : ListViewModel
    {
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public bool RecordsFound { get; set; }

        public WardshipRecordVMlistView()
        {
            RecordsFound = false;
        }

        public IPagedList<WardshipRecord> WardshipResults { get; set; }

    }

    public class WardshipRecordCreationModel
{
    [Required(ErrorMessage = "Child Surname is required.")]
    [Display(Name = "Child Surname")]
    [MaxLength(100)]
    public string ChildSurname { get; set; }

    [Required(ErrorMessage = "Child Forename is required.")]
    [Display(Name = "Child Forename")]
    [MaxLength(100)]
    public string ChildForenames { get; set; }

    [Required(ErrorMessage = "Child Date of Birth is required.")]
    [Display(Name = "Child Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? ChildDateOfBirth { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    [Display(Name = "Gender")]
    public int? GenderID { get; set; }

    // Dropdown list items for Gender
    public List<SelectListItem> GenderList { get; set; }

    // Constructor to initialize the dropdown list
    public CreateWardshipRecordViewModel()
    {
        // Initialize the Gender dropdown list
        GenderList = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Male" },
            new SelectListItem { Value = "2", Text = "Female" },
        };
    }

    [Required(ErrorMessage = "District Judge is required.")]
    [Display(Name = "District Judge")]
    public int? DistrictJudgeID { get; set; }

    // Dropdown list items for District Judges
    public List<SelectListItem> DistrictJudgeList { get; set; }

    // Constructor to initialize the dropdown list
    public CreateWardshipRecordViewModel(List<DistrictJudge> districtJudges)
    {
        // Initialize the District Judge dropdown list from the database
        DistrictJudgeList = districtJudges.Select(d => new SelectListItem
        {
            Value = d.DistrictJudgeID.ToString(),
            Text = d.Name
        }).ToList();
    }

    [Required(ErrorMessage = "Date Issued is required.")]
    [Display(Name = "Date Issued")]
    [DataType(DataType.Date)]
    public DateTime? DateOfOS { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    [Display(Name = "Type")]
    public int? TypeID { get; set; }

    [Required(ErrorMessage = "Court is required.")]
    [Display(Name = "Court")]
    public int? CourtID { get; set; }
}


    // public class WardshipRecordCreationModel
    // {
    //     public string ChildSurname { get; set; }
    //     public string ChildForenames { get; set; }
    //     public string Court { get; set; }
    //     public DateTime ChildDateOfBirth { get; set; }
    //     public string Gender { get; set; }
    //     public DateTime DateIssued { get; set; }
    //     public string DistrictJudge { get; set; }
    // }

    // public class WardshipRecordEditModel
    // {
    //     public string ChildSurname { get; set; }
    //     public string ChildForenames { get; set; }
    //     public string Court { get; set; }
    //     public DateTime ChildDateOfBirth { get; set; }
    //     public string Gender { get; set; }
    //     public DateTime DateIssued { get; set; }
    //     public string DistrictJudge { get; set; }
    // }

}