using System;
using System.Linq;
using System.Web.Mvc;
using Wardship.Models;
using TPLibrary.Logger;
using ClosedXML.Excel;
using System.IO;
using PagedList;

namespace Wardship.Controllers
{
    public class ReportController : Controller
    {
        private readonly ISQLRepository db;
        private readonly ICloudWatchLogger _logger;

        public ReportController(ISQLRepository repository, ICloudWatchLogger logger)
        {
            db = repository;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                var model = new Report
                {
                    ReportBegin = Convert.ToDateTime("01/01/2013"),
                    ReportFinal = DateTime.Today
                };
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in ReportController in Index method, for user {(User as ICurrentUser).DisplayName}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Report model, int? page)
        {
            if (ModelState.IsValid && model.IsValidDateRange())
            {
                var wardshipRecords = db.WardshipsGetAll()
                    .Where(w => w.DateOfOS >= model.ReportBegin && w.DateOfOS <= model.ReportFinal);

                int pageSize = 20; // You can adjust this value as needed
                int pageNumber = (page ?? 1);
                model.WardshipRecordsList = wardshipRecords.ToPagedList(pageNumber, pageSize);

                return View("Report", model);
            }

            if (!model.IsValidDateRange())
            {
                ModelState.AddModelError("", "End date must be after or equal to Begin date");
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(Report model, int? page)
        {
            if (ModelState.IsValid && model.IsValidDateRange())
            {
                var wardshipRecords = db.WardshipsGetAll()
                    .Where(w => w.DateOfOS >= model.ReportBegin && w.DateOfOS <= model.ReportFinal);

                int pageSize = 20; // Keep this consistent with the Index action
                int pageNumber = (page ?? 1);
                model.WardshipRecordsList = wardshipRecords.ToPagedList(pageNumber, pageSize);

                return View(model);
            }

            return RedirectToAction("Index");
        }

        public ActionResult ExportToExcel(Report model)
        {
            var wardshipRecords = db.WardshipsGetAll()
                .Where(w => w.DateOfOS >= model.ReportBegin && w.DateOfOS <= model.ReportFinal)
                .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Report");

                // Add headers
                worksheet.Cell(1, 1).Value = "Date Issued";
                worksheet.Cell(1, 2).Value = "Wardship Number";
                worksheet.Cell(1, 3).Value = "Child Surname";
                worksheet.Cell(1, 4).Value = "Child Forenames";
                worksheet.Cell(1, 5).Value = "Gender";
                worksheet.Cell(1, 6).Value = "Child Date of Birth";
                worksheet.Cell(1, 7).Value = "Case Type";
                worksheet.Cell(1, 8).Value = "Type";
                worksheet.Cell(1, 9).Value = "Record";
                worksheet.Cell(1, 10).Value = "Judge";

                // Add data
                for (int i = 0; i < wardshipRecords.Count; i++)
                {
                    var record = wardshipRecords[i];
                    var row = i + 2;
                    worksheet.Cell(row, 1).Value = record.DateOfOS?.ToShortDateString() ?? "";
                    worksheet.Cell(row, 2).Value = record.FileNumber ?? "";
                    worksheet.Cell(row, 3).Value = record.ChildSurname ?? "";
                    worksheet.Cell(row, 4).Value = record.ChildForenames ?? "";
                    worksheet.Cell(row, 5).Value = record.Gender?.Detail ?? "";
                    worksheet.Cell(row, 6).Value = record.ChildDateofBirth?.ToShortDateString() ?? "";
                    worksheet.Cell(row, 7).Value = record.CaseType?.Detail ?? "";
                    worksheet.Cell(row, 8).Value = record.Type?.Detail ?? "";
                    worksheet.Cell(row, 9).Value = record.Record?.Detail ?? "";
                    worksheet.Cell(row, 10).Value = record.DistrictJudge?.Name ?? "";
                }

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                // Generate the Excel file
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    var fileName = $"WardshipReport_{model.ReportBegin:yyyyMMdd}-{model.ReportFinal:yyyyMMdd}.xlsx";
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }
        }
    }
}