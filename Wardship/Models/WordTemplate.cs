using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Wardship.Logger;

namespace Wardship.Models
{

    public class TemplateEdit
    {
        public WordTemplate Template { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }
        public bool UploadSuccessful { get; set; }
        public string ErrorMessage { get; set; }
        public SelectList DiscriminatorType { get; set; }
        
        public TemplateEdit(int id) : this()
        {
                    using (SourceRepository db = new SQLRepository(new TelemetryLogger()))
                    {
                        Template = db.GetTemplateByID(id);
                    }
        }

         public TemplateEdit()
        {
            DiscriminatorType = new SelectList(
                  new List<Object>{ 
                        new { value = "" , text = "Please select a value"}, 
                        new { value = "All" , text = "All"  },
                        new { value = "Error" , text = "Error"},
                  },
                  "value",
                  "text", "");
        }
    }


    public class WordTemplate
    {
        [Key]
        public int templateID { get; set; }


        [Required, MaxLength(80), Display(Name = "Document Name")]
        public string templateName { get; set; }
        [Required]
        public string templateXML { get; set; }
        public bool active { get; set; }
        public DateTime? deactivated { get; set; }
        [MaxLength(50)]
        public string deactivatedBy { get; set; }
    }

    public class WordFile
    {
        public string fileName { get; set; }
        public string Path { get; set; }
        public string fullName { get; set; }
        public int WardshipDataID { get; set; }

        public WordFile() { } //empty constructor None variable

        public WordFile(WardshipRecord Wardshipdata, string serverPath) //Constructor with  2 variable
        {

            WardshipDataID = Wardshipdata.WardshipCaseID;
            Path = string.Format(serverPath + "{0}", Wardshipdata.WardshipCaseID);
            fileName = string.Format("SCD26Location-{0}.doc", Wardshipdata.WardshipCaseID);
            fullName = string.Format("{0}\\{1}", Path, fileName);
            //Ensure folder exists to create outoput
            //if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
        }

        public WordFile(WardshipRecord Wardshipdata, string serverPath, WordTemplate template) //Constructor with 3 variables
        {
            WardshipDataID = Wardshipdata.WardshipCaseID;
            Path = string.Format(serverPath + "{0}", Wardshipdata.WardshipCaseID);
            fileName = string.Format("{0}-{1}.doc", template.templateName, Wardshipdata.WardshipCaseID);
            fullName = string.Format("{0}\\{1}", Path, fileName);
            //Ensure folder exists to create outoput
            //if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);
        }

        public bool Exists
        {
            get
            {
                return System.IO.File.Exists(fullName);
            }
        }

        public string serverPath
        {
            get
            {
                return string.Format("~/Documents/{0}/{1}", WardshipDataID, fileName);
            }
        }

    }


    public class TemplateListVM
    {
        public IEnumerable<WordTemplate> WordTemplates { get; set; }
        public int WardshipCaseID { get; set; }
        public int templateID { get; set; }
    }

}