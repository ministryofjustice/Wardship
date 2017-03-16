using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Wardship.Models
{
    public enum AlertStatus
    {
        Off,
        Warning,
        High,
        Overdue
    }
    public class Alert
    {
        [Key]
        public int AlertID { get; set; }
        [Required]
        public bool Live { get; set; }
        [Required, UIHint("DateTimePicker")]
        public DateTime EventStart { get; set; }
        [Required]
        public int RaisedHours { get; set; }
        [Required, UIHint("DateTimePicker")]
        public DateTime WarnStart { get; set; }
        [Required, MaxLength(200), DataType(DataType.MultilineText), UIHint("TextAreaWithCountdown")]
        [AdditionalMetadata("maxLength", 200)]
        public string Message { get; set; }

        public string DisplayMessage 
        {    
            get
            {
                string result = Message;
                if (result != null)
                {
                    result = result.Replace("@ESDT", this.EventStart.ToString("d MMM yy \\a\\t H:mm"));
                    result = result.Replace("@ESD", this.EventStart.ToString("d MMM yy"));
                    result = result.Replace("@EST", this.EventStart.ToString("H:mm"));
                }
                return result;
            }
        }

        public AlertStatus Status
        {
            get
            {
                if (Live == true)
                {
                    if (DateTime.Now >= EventStart) return AlertStatus.Overdue;
                    if (DateTime.Now < WarnStart) return AlertStatus.Off;
                    if (DateTime.Now > EventStart.AddHours(-RaisedHours)) return AlertStatus.High;
                    return AlertStatus.Warning;
                }
                else
                {
                    return AlertStatus.Off;
                }
            }
        }
        public string DisplayClass
        {
            get
            {
                switch (Status)
                {
                    case AlertStatus.Warning:
                        return "comment";
                    case AlertStatus.High:
                        return "warning";
                    case AlertStatus.Overdue:
                        return "error";
                    case AlertStatus.Off:
                    default:
                        return "Inactive";
                }
            }
        }
    }
}