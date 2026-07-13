using System;
using System.Web.Mvc;
using Wardship.Models;
using Wardship.Helpers;
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
            WordTemplate template = db.GetTemplateByID(id);
            return File(template.templateDOTX, "application/vnd.openxmlformats-officedocument.wordprocessingml.template", template.templateName + ".dotx"); 
        }
        public ActionResult Create()
        {
            TemplateEdit model = new TemplateEdit();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(TemplateEdit model)
        {
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("dotx")) { throw new NotUploaded("Please select a .dotx file to upload"); }
                    if (model.uploadFile.ContentLength == 0) { throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try"); }
                    //Upload
                    byte[] fileBytes;
                    using (var ms = new MemoryStream())
                    {
                        model.uploadFile.InputStream.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    if (!SensitivityLabelValidator.HasSensitivityLabel(fileBytes))
                        throw new NotUploaded("The template must have a Microsoft sensitivity label applied before uploading. Please open the file in Word, apply the appropriate label, and re-upload.");

                    model.Template.templateDOTX = fileBytes;
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
            catch (NotUploaded ex)
            {
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
                model.UploadSuccessful = false;
                return View(model);
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
            try
            {
                //Tests before uploading
                if (model.uploadFile != null)
                {
                    if (!Path.GetExtension(model.uploadFile.FileName.ToLower()).EndsWith("dotx"))
                    {
                        throw new NotUploaded("Please select a .dotx file to upload");
                    }
                    if (model.uploadFile.ContentLength == 0)
                    {
                        throw new NotUploaded("The selected file appears to be empty, please select a different file and re-try");
                        }
                    //Upload
                    byte[] fileBytes;
                    using (var ms = new MemoryStream())
                    {
                        model.uploadFile.InputStream.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }

                    if (!SensitivityLabelValidator.HasSensitivityLabel(fileBytes))
                        throw new NotUploaded("The template must have a Microsoft sensitivity label applied before uploading. Please open the file in Word, apply the appropriate label, and re-upload.");

                    model.Template.templateDOTX = fileBytes;
                }
                else
                {
                    model.Template.templateDOTX = db.GetTemplateByID(model.Template.templateID).templateDOTX;
                }

                db.UpdateTemplate(model.Template);
                return RedirectToAction("Index");
            }
            catch (NotUploaded ex)
            {
                model.ErrorMessage = genericFunctions.GetLowestError(ex);
                model.UploadSuccessful = false;
                return View(model);
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
