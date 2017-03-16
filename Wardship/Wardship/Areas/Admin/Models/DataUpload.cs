using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wardship.Models
{
    public class DataUpload
    {
        public int DataUploadID { get; set; }
        public DateTime UploadStarted { get; set; }
        public string UploadedBy { get; set; }
        public string FileName { get; set; }
        public string FullPathandName { get; set; }
        public int FileSize { get; set; }
        public DateTime? UploadCompleted { get; set; }
        public int NumberofRows { get; set; }
        public int NumberOfErrs { get; set; }

        public DataUpload()
        {
            UploadStarted = DateTime.Now;
            NumberOfErrs = 0;
            NumberofRows = 0;
        }

        public TimeSpan TimeTaken
        {
            get
            {
                if (UploadCompleted != null)
                {
                    return (DateTime)UploadCompleted - UploadStarted;
                }
                return TimeSpan.Zero;
            }
        }
    }
}