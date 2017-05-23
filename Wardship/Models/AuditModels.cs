using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Wardship.Models
{
    public class AuditEventViewModel
    {
        public PagedList.IPagedList<AuditEvent> AuditEvents { get; set; }
        public string auditType { get; set; }
        public string itemID { get; set; }
    }

    public class AuditEvent
    {
        [Key]
        public int idAuditEvent { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required, MaxLength(40)]
        public string UserID { get; set; }
        [Required]
        public string idAuditEventDescription { get; set; }
        //[Required, MaxLength(256)]
        //public string RecordChanged { get; set; }

        [MaxLength(100)]
        public string ChildSurname { get; set; }

        [MaxLength(100)]
        public string ChildForenames { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ChildDateofBirth { get; set; }

        public virtual string ChildOutputName // Full Name 
        {
            get { return string.Format("{0} {1}", ChildForenames, ChildSurname); }
        }

        //public int? RecordAddedTo { get; set; }
        //public virtual AuditEventDescription auditEventDescription { get; set; }
        //public virtual ICollection<AuditEventDataRow> AuditEventDataRows { get; set; }
        //[NotMapped]
        //public string EventDescription { get; set; }



    }

    public class AuditEventDescription
    {
        [Key]
        public int idAuditEventDescription { get; set; }
        [Required, MaxLength(40)]
        public string AuditDescription { get; set; }
        public virtual ICollection<AuditEvent> AuditEvents { get; set; }
    }

    public class AuditEventDataRow
    {
        [Key]
        public int idAuditData { get; set; }
        [Required]
        public int idAuditEvent { get; set; }
        [Required, MaxLength(200)]
        public string ColumnName { get; set; }
        [Required, MaxLength(200)]
        public string Was { get; set; }
        [Required, MaxLength(200)]
        public string Now { get; set; }

        public virtual AuditEvent auditEvent { get; set; }
    }
}