﻿using System;
using System.Web.Mvc;
using Wardship.Models;
using System.IO;
using System.Xml;
using System.Web.ModelBinding;
using TPLibrary.Logger;

namespace Wardship.Areas.Admin.Controllers
{
    [AuthorizeRedirect(Roles = "Admin")]
    [Authorize]
    [ValidateAntiForgeryTokenOnAllPosts]
    public class TemplatesController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public TemplatesController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
        }
      
        // GET: /Admin/Template/
        public ActionResult Index()
        {
            var model = db.GetAllTemplates();
            return View(model);
        }

        public ActionResult Open(int id)
        {
            WordTemplate WordTemplate = db.GetTemplateByID(id);
            XmlDocument xDoc = new XmlDocument();
            xDoc.InnerXml = WordTemplate.templateXML;
            return File(genericFunctions.ConvertToBytes(xDoc), "application/msword", WordTemplate.templateName + ".xml"); 
        }
        public ActionResult Create()
        {
            TemplateEdit model = new TemplateEdit();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TemplateEdit model)
        {
            var xml = string.Empty;
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("xml")) { throw new NotUploaded("Please select an XML file to upload"); }
                    if (model.uploadFile.ContentLength == 0) { throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try"); }
                    //Upload
                    var fileName = Path.Combine("C:\\WardshipUploads", Path.GetFileName(model.uploadFile.FileName));
                    (new FileInfo(fileName)).Directory.Create();
                    model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    XmlDocument document = new XmlDocument();
                    document.Load(fileName);
                    xml = document.InnerXml;
                    //Delete file
                    System.IO.File.Delete(fileName);
                    model.Template.templateXML = xml;
                    model.Template.active = true;
                    db.AddNewTemplate(model.Template);
                    return RedirectToAction("Index");
                }
                else
                {
                    model.ErrorMessage = "Please select a template file";
                    model.UploadSuccessful = false;
                    ModelState.AddModelError("Error", "Please select a template file");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
                model.UploadSuccessful = false;
                _logger.LogError(ex, $"Exception in TemplatesController in Create method, for user {User.Identity.Name}");
                return View("Error");
            }
        }

        public ActionResult Edit(int id)
        {
            TemplateEdit model = new TemplateEdit(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(TemplateEdit model)
        {
            WordTemplate oldTemplate = db.GetTemplateByID(model.Template.templateID);

            var xml = string.Empty;
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("xml")) { throw new NotUploaded("Please select an XML file to upload"); }
                    if (model.uploadFile.ContentLength == 0) { throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try"); }
                    //Upload
                    var fileName = Path.Combine("C:\\WardshipUploads", Path.GetFileName(model.uploadFile.FileName));
                    model.uploadFile.SaveAs(fileName); //Save to uploads folder     
                    XmlDocument document = new XmlDocument();
                    document.Load(fileName);
                    xml = document.InnerXml;
                    //Delete file
                    System.IO.File.Delete(fileName);
                }
                else
                {
                    xml = db.GetTemplateByID(model.Template.templateID).templateXML;
                }
                model.Template.templateXML = xml;
                db.UpdateTemplate(model.Template);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
                model.UploadSuccessful = false;
                _logger.LogError(ex, $"Exception in TemplatesController in Edit method, for user {User.Identity.Name}");
                return View("Error");
            }
        }

        // GET: /Admin/Template/Delete/5
        public ActionResult Deactivate(int id)
        {
            WordTemplate model = db.GetTemplateByID(id);
            if (model.active == false)
            {
                ErrorModel errModel = new ErrorModel(2);
                errModel.ErrorMessage = string.Format("You cannot view {0} as it has been deactivated, please raise a help desk call to re-activate it.", model.templateName);
                TempData["ErrorModel"] = errModel;
                return RedirectToAction("IndexByModel", "Error", new { area = "", model = errModel ?? null });
            }
            return View(model);
        }
        //
        // POST: /Admin/Solicitor/Delete/5
        [HttpPost, ActionName("Deactivate")]
        public ActionResult DeactivateConfirmed(int id)
        {
            try
            {
                WordTemplate model = db.GetTemplateByID(id);
                model.active = false;
                model.deactivated = DateTime.Now;
                model.deactivatedBy = ((Wardship.ICurrentUser)User).DisplayName;
                //model.templateXML = null;
                db.UpdateTemplate(model);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in TemplatesController in Deactivate method, for user {User.Identity.Name}");
                return View("Error");
            }
        }

    }
}
