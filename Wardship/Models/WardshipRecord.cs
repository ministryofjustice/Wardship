using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PagedList;
using System.Web.Mvc;

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

        [Display(Name = "Custom Status Reason")]
        public string CustomStatusReason { get; set; }

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

        [Display(Name = "CAFCASS Involved")]
        public int? CAFCASSInvolvedID { get; set; } //used for setting the relationship in the DB
        public virtual CAFCASSInvolved CAFCASSInvolved { get; set; }//used for the status dropdown

        [Display(Name = "Lapse Letter Sent")]
        [DataType(DataType.Date)]
        public DateTime? LapseLetterSent { get; set; }

        [Display(Name = "First Appointment Date")]
        [DataType(DataType.Date)]
        public DateTime? FirstAppointmentDate { get; set; }

        [Display(Name = "Hearing Date")]
        [DataType(DataType.Date)]
        public DateTime? HearingDate { get; set; }

        public string Username { get; set; }

        public bool? Deleted { get; set; }
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
        public CAFCASSInvolved CAFCASSInvolved { get; set; }
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
    
}