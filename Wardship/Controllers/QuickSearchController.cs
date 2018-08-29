using System;
using System.Linq;
using System.Web.Mvc;
using Wardship.Models;
using PagedList;
using System.Xml;
using System.Text;
using TPLibrary.Logger;

namespace Wardship.Controllers
{
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class QuickSearchController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public QuickSearchController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
        }


        // GET: /QuickSearch/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PoPupDetails(int id)
        {
            WardshipRecord model = db.GetWardshipRecordByID(id);
            return PartialView("_PopupDetails", model);

        }

        //go to Wardship index
        public ActionResult Homeindex()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Index(QuickSearch model)
        {
            try
            {
                if (model != null && model.isValid())//if not equal to null build list of results 
                {
                    #region New search type

                    model.page = model.page ?? 1;

                    if (model.FileNumber != null)// search criteria 
                    {
                        model.results = db.QuickSearchByNumber(model.FileNumber).ToPagedList(model.page ?? 1, 15);
                    }
                    if (model.ChildSurname != null)
                    {
                        model.results = db.QuickSearchSurname(model.ChildSurname).ToPagedList(model.page ?? 1, 15);
                    }
                    if (model.ChildForenames != null)
                    {
                        model.results = db.QuickSearchByForename(model.ChildForenames).ToPagedList(model.page ?? 1, 15);
                    }
                    if (model.ChildDateofBirth != null)
                    {
                        model.results = db.QuickSearchByDOB(model.ChildDateofBirth).ToPagedList(model.page ?? 1, 15);
                    }


                    //Adding Audit for new record 
                    var Audit = new AuditEvent();

                    Audit.EventDate = DateTime.Now;
                    Audit.UserID = (User as Wardship.ICurrentUser).DisplayName;
                    Audit.idAuditEventDescription = "New Search Made";
                    Audit.ChildForenames = model.ChildForenames;
                    Audit.ChildSurname = model.ChildSurname;
                    Audit.ChildDateofBirth = model.ChildDateofBirth;

                    //Audit.RecordChanged = model.WardshipCaseID.ToString();

                    db.AddAuditEvent(Audit);
                    //


                    return View("Results", model);
                    //return View(model);


                    #endregion

                }
                //return View(model);
                return View("Results", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in QuickSearchController in Index method, for user {User.Identity.Name}");
                return View("Error");
            }
         }




        public ActionResult CompleteForPrinting(int id, QuickSearch model)
        {   
            int WardshipCaseID;
            //1st Pass still using temp data to pass through "WardshipCaseID" 
            WardshipCaseID = id; //so creating a new instance of the ID if it exists

            TempData["WardshipCaseID"] = WardshipCaseID;    //putting all field values into Tempdata
                                                            //Then sending the data to my Print action
    
            return View();
        }



        [HttpPost]
        public ActionResult Print(QuickSearch model) ///int WardshipCaseID
        {
            int RefNum = 0;
            try
            {
                int WardshipCaseID = (int)TempData["WardshipCaseID"]; // setting the ID fron 0 = no ID  0< = found ID 

                TemplateListVM Amodel = new TemplateListVM();
                
                if (WardshipCaseID == 0) //Print Not found letter
                {
                    Amodel.templateID = 1;//template id
                }
                else if (WardshipCaseID > 0) // Print Search found letter
                {
                    Amodel.templateID = 2;//template id
                }
                RefNum = Amodel.templateID;
                //Load The WardshipCase object's 
                WardshipRecord WardshipRecord = db.GetWardshipRecordByID(WardshipCaseID);

                //Load The Template object
                WordTemplate template = db.GetTemplateByID(RefNum);

                //Create XML object for Template & put data in
                XmlDocument xDoc = new XmlDocument();

                //Set XML data of doument from template object
                xDoc.InnerXml = template.templateXML;

                //Applicant address
                xDoc.InnerXml = xDoc.InnerXml.Replace("||ADDRESS||", model.printAddressMultiLine);
                //References
                xDoc.InnerXml = xDoc.InnerXml.Replace("||DATE||", DateTime.Today.ToShortDateString());

                if (WardshipCaseID != 0)
                {
                  
               
                    //some child names can be blank
                    if (WardshipRecord.ChildForenames != null && WardshipRecord.ChildSurname != null)
                    {
                        xDoc.InnerXml = xDoc.InnerXml.Replace("||CHILDFULLNAME||", WardshipRecord.ChildOutputName.ToString());
                    }
                    if (WardshipRecord.ChildForenames == null && WardshipRecord.ChildSurname != null)
                    {
                        xDoc.InnerXml = xDoc.InnerXml.Replace("||CHILDFULLNAME||", WardshipRecord.ChildSurname.ToString());
                    }
                    if (WardshipRecord.ChildForenames != null && WardshipRecord.ChildSurname == null)
                    {
                        xDoc.InnerXml = xDoc.InnerXml.Replace("||CHILDFULLNAME||", WardshipRecord.ChildForenames.ToString());
                    }

                }

                else
                {
                    AuditEvent Auditmodel = new AuditEvent();
                    Auditmodel = db.AuditEventsGetAll().LastOrDefault();

                    if (Auditmodel.ChildForenames != null && Auditmodel.ChildSurname != null)
                    {
                        xDoc.InnerXml = xDoc.InnerXml.Replace("||CHILDFULLNAME||", Auditmodel.ChildOutputName.ToString());
                    }
                    if (Auditmodel.ChildForenames == null && Auditmodel.ChildSurname != null)
                    {
                        xDoc.InnerXml = xDoc.InnerXml.Replace("||CHILDFULLNAME||", Auditmodel.ChildSurname.ToString());
                    }
                    if (Auditmodel.ChildForenames != null && Auditmodel.ChildSurname == null)
                    {
                        xDoc.InnerXml = xDoc.InnerXml.Replace("||CHILDFULLNAME||", Auditmodel.ChildForenames.ToString());
                    }

                }
               

                //USERNAME
                xDoc.InnerXml = xDoc.InnerXml.Replace("||USERNAME||", (User as Wardship.ICurrentUser).DisplayName);

                //Return saved document (to the screen with the data embedded....)
                return File(ConvertToBytes(xDoc), "application/msword", template.templateName + ".doc"); //return byte version

            }
            catch (Exception ex)
            {
                // redirect to error page
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("Could not load SearchTemplate {0}", RefNum);
                TempData["ErrorModel"] = errModel;
                _logger.LogError(ex, $"Exception in QuickSearchController in Print method, for user {User.Identity.Name}");
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }


        }



        public static byte[] ConvertToBytes(XmlDocument doc)
        {
            Encoding encoding = Encoding.UTF8;
            byte[] docAsBytes = encoding.GetBytes(doc.OuterXml);
            return docAsBytes;
        }

  
    }
}
