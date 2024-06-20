using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wardship.Models;
using System.Security.Principal;
using System.Data;
using System.Data.Entity;

using TPLibrary.Logger;

namespace Wardship
{
    public class SQLRepository : ISQLRepository
    {
        DataContext db = new DataContext();

        private readonly ICloudWatchLogger _logger;

        public SQLRepository(ICloudWatchLogger logger)
        {
            db = new DataContext();
            _logger = logger;
        }

        #region FAQ
        IEnumerable<Models.FAQ> ISQLRepository.FAQsGetAll()
        {
            return db.FAQs.ToList();
        }

        IEnumerable<Models.FAQ> ISQLRepository.FAQsGetOnline()
        {
            return db.FAQs.Where(f => f.loggedInUser == true).ToList();
        }

        IEnumerable<Models.FAQ> ISQLRepository.FAQsGetOffline()
        {
            return db.FAQs.Where(f => f.loggedInUser == false).ToList();
        }

        FAQ ISQLRepository.FAQGetbyID(int id)
        {
            return db.FAQs.Find(id);
        }

        void ISQLRepository.FAQUpdate(FAQ faq)
        {
            // db.Entry(control).State = EntityState.Modified;

            var tmpFAQ = db.FAQs.Single(f => f.faqID == faq.faqID);
            tmpFAQ.answer = faq.answer;
            tmpFAQ.loggedInUser = faq.loggedInUser;
            tmpFAQ.question = faq.question;
            db.SaveChanges();
        }

        void ISQLRepository.FAQAdd(FAQ faq)
        {
            db.FAQs.Add(faq);
            db.SaveChanges();
        }
        #endregion

        #region Audit
        IEnumerable<Models.AuditEvent> ISQLRepository.AuditEventsGetAll()
        {
            return db.AuditEvents.ToList();
        }
        #endregion

        #region Users and Groups
        IEnumerable<ADGroup> ISQLRepository.GetAllGroups()
        {
            return db.ADGroups.ToList();
        }
        IEnumerable<User> ISQLRepository.GetAllUsers()
        {
            return db.Users.ToList();
        }
        ADGroup ISQLRepository.GetGroupByID(int id)
        {
            return db.ADGroups.Find(id);
        }
        AccessLevel ISQLRepository.UserAccessLevel(IPrincipal User)
        {
            try
            {
                AccessLevel? grpLvl = null;
                AccessLevel? usrLvl = null;

                //groups
                IEnumerable<ADGroup> ADGroups = db.ADGroups;
                foreach (var group in ADGroups.OrderByDescending(g => g.RoleStrength))
                {

                    if (User.IsInRole(group.Name))
                    {
                        grpLvl = (AccessLevel)group.RoleStrength;
                        break;
                    }
                }
                //Not in a group?  Try loading a user object
                string userName = ((string)User.Identity.Name).Split('\\').Last();
                User usr = db.Users.FirstOrDefault(x => x.Name.ToLower() == userName.ToLower());
                if (usr != null)
                {
                    usrLvl = (AccessLevel)usr.RoleStrength;
                    if (usrLvl != null)
                    {
                        return (AccessLevel)usrLvl;
                    }
                }
                return grpLvl ?? AccessLevel.Denied;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception in SqlRepository in UserAccessLevel method, for user {(HttpContext.Current.User as IPrincipal).Identity.Name}");
                return AccessLevel.Denied;
            }
        }
        string ISQLRepository.curUserDisplay(IPrincipal User)
        {
            string userName = ((string)User.Identity.Name).Split('\\').Last();
            User usr = db.Users.FirstOrDefault(x => x.Name.ToLower() == userName.ToLower());
            if (usr != null)
            {
                return usr.DisplayName;
            }
            foreach (var group in db.ADGroups.OrderByDescending(g => g.RoleStrength))
            {
                if (User.IsInRole(group.Name))
                {
                    return group.Name;
                }
            }
            return User.Identity.Name;
        }
        void ISQLRepository.UserAdd(User model)
        {
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }
        IEnumerable<Role> ISQLRepository.GetAllRoles()
        {
            ICurrentUser usr = new ICurrentUser(HttpContext.Current.User.Identity);
            if (usr.AccessLevel == AccessLevel.Admin)
            {
                return db.Roles.ToList();
            }
            else
            {
                return db.Roles.Where(x => x.Detail != "Admin");
            }
        }

        User ISQLRepository.GetUserByID(int id)
        {
            return db.Users.Find(id);
        }
        User ISQLRepository.GetUserByName(string Name)
        {
            User usr = db.Users.Single(x => x.Name.ToLower() == Name.ToLower());
            usr.LastActive = DateTime.Now;
            db.SaveChanges();
            return usr;
        }
        void ISQLRepository.UpdateUser(User model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }


        #endregion

        #region Alerts
        IEnumerable<Alert> ISQLRepository.getCurrentAlerts()
        {
            return db.Alerts.Where(x => x.Live == true && x.WarnStart <= DateTime.Now);
        }
        IEnumerable<Alert> ISQLRepository.getAllAlerts()
        {
            return db.Alerts;
        }
        void ISQLRepository.CreateAlert(Alert model)
        {
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }
        Alert ISQLRepository.getAlertbyID(int id)
        {
            return db.Alerts.Find(id);
        }
        void ISQLRepository.updateAlert(Alert model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region Courts
        IEnumerable<Court> ISQLRepository.getAllCourts()
        {
            return db.Courts;
        }
        Court ISQLRepository.getCourtByID(int id)
        {
            return db.Courts.Find(id);
        }

        void ISQLRepository.CreateCourt(Court model)
        {
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }

        void ISQLRepository.EditCourt(Court model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region Templates
        IEnumerable<WordTemplate> ISQLRepository.GetAllTemplates()
        {
            return db.WordTemplate.Where(x => x.active == true);
        }
        WordTemplate ISQLRepository.GetTemplateByID(int id)
        {
            return db.WordTemplate.Find(id);
        }
        void ISQLRepository.UpdateTemplate(WordTemplate model)
        {
            WordTemplate oldTemplate = db.WordTemplate.Find(model.templateID);

            db.Entry(oldTemplate).CurrentValues.SetValues(model);
            db.SaveChanges();
        }
        void ISQLRepository.DeactivateTemplate(WordTemplate model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
        }
        void ISQLRepository.AddNewTemplate(WordTemplate model)
        {
            db.Entry(model).State = EntityState.Added;
            db.SaveChanges();
        }
        #endregion

        #region QuickSearch
        public IEnumerable<WardshipRecord> QuickSearchByNumber(string p)
        {
            return db.WardshipRecord.Where(x => x.FileNumber.ToLower().Contains(p.ToLower())).ToList();
        }

        IEnumerable<WardshipRecord> ISQLRepository.QuickSearchSurname(string p)
        {
            return db.WardshipRecord.Where(x => x.ChildSurname.ToLower().Contains(p.ToLower())).ToList();
        }

        public IEnumerable<WardshipRecord> QuickSearchByForename(string p)
        {
            return db.WardshipRecord.Where(x => x.ChildForenames.ToLower().Contains(p.ToLower())).ToList();
        }
        public IEnumerable<WardshipRecord> QuickSearchByDOB(DateTime? DOB)
        {
            return db.WardshipRecord.Where(x => x.ChildDateofBirth == DOB).ToList();
        }

        public IEnumerable<WardshipRecord> QuickSearchByType(DateTime? DofOS)
        {
            return db.WardshipRecord.Where(x => x.DateOfOS == DofOS).ToList();
        }


        #endregion

        #region Wardships records and collections
        public WardshipRecord GetWardshipRecordByID(int id)
        {
            return db.WardshipRecord.FirstOrDefault(d => d.WardshipCaseID == id);
        }
        public IEnumerable<WardshipRecord> WardshipsGetAll()
        {
            return db.WardshipRecord.ToList();
        }
        #endregion

        void IDisposable.Dispose()
        {

        }

        public void AddAuditEvent(AuditEvent Audit)
        {
            db.AuditEvents.Add(Audit);
            db.SaveChanges();
        }


         //CREATE
    public void WardshipRecordCreateNew(WardshipRecord WardshipRecordToCreate)
    {
        #region NumberCode

        //Building The Number 
        string slash;
        int DateYear;
        string num;
        string Prefix;


        int TheNumberToSaveIs;

        WardshipRecordToCreate.WardshipRecord_Status = db.WardshipRecord_Status.Find(WardshipRecordToCreate.WardshipRecord_StatusID);
        WardshipRecordToCreate.WardshipRecord_Type = db.WardshipRecord_Type.Find(WardshipRecordToCreate.WardshipRecord_TypeID);
        //Record in the database relating to status and type
        var record = db.AllocateNumber.Where(P => P.WardshipRecordStatus == WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus && P.WardshipRecordType == WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType).OrderByDescending(o => o.Number).FirstOrDefault();

        if (DateTime.Now.Year > record.Timestamp.Year)
        {
            TheNumberToSaveIs = 1;//re-start the numbering
        }
        else
        {
            TheNumberToSaveIs = record.Number + 1;//no need to re-start the numbering
        }

        //Allocate zeros
        if (TheNumberToSaveIs.ToString().Length == 1)
        { LeadingZeros = "0000"; }
        else if (TheNumberToSaveIs.ToString().Length == 2)
        { LeadingZeros = "000"; }
        else if (TheNumberToSaveIs.ToString().Length == 3)
        { LeadingZeros = "00"; }
        else if (TheNumberToSaveIs.ToString().Length == 4)
        { LeadingZeros = "0"; }
        else if (TheNumberToSaveIs.ToString().Length == 5)
        { LeadingZeros = ""; }


        num = LeadingZeros + TheNumberToSaveIs;
        slash = "/";
        DateYear = Int32.Parse(DateTime.Now.Year.ToString());

        //Registered WardshipRecord with Step 
        if (WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus == "Registered" && WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType == "Step")
        {
            Prefix = "5";
            WardshipRecord_No = Prefix + num + slash + (DateYear);
        }
        //Registered WardshipRecord Natural 
        else if (WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus == "Registered" && WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType == "Natural")
        {
            Prefix = "R";
            WardshipRecord_No = Prefix + num + slash + (DateYear);
        }

        //Registered WardshipRecord with HFE 
        else if (WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus == "Registered" && WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType == "HFE")
        {
            Prefix = "HFE";
            WardshipRecord_No = Prefix + num + slash + (DateYear);
        }

        //Stop WardshipRecord with Step 
        else if (WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus == "Stop" && WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType == "Step")
        {
            Prefix = "S5";
            WardshipRecord_No = Prefix + num + slash + (DateYear);
        }
        //Stop WardshipRecord Natural 
        else if (WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus == "Stop" && WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType == "Natural")
        {
            Prefix = "S";
            WardshipRecord_No = Prefix + num + slash + (DateYear);
        }

        //Stop WardshipRecord with HFE 
        else if (WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus == "Stop" && WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType == "HFE")
        {
            Prefix = "SHFE";
            WardshipRecord_No = Prefix + num + slash + (DateYear);
        }

        //WardshipRecord_No = num + slash + (DateYear);
        if (record.WardshipRecordStatus != WardshipRecordToCreate.WardshipRecord_Status.WardshipRecordStatus && record.WardshipRecordType != WardshipRecordToCreate.WardshipRecord_Type.WardshipRecordType)
        {

            //Error Trapping!!!
            //return View(??);
        }
        else
        {
            record.Number = TheNumberToSaveIs;
            record.Timestamp = DateTime.Now;
            record.WardshipRecord_Number = WardshipRecord_No;
        }

        db.Entry(record).State = EntityState.Modified;
        db.SaveChanges();


        string parameter = record.WardshipRecord_Number;
        WardshipRecordToCreate.WardshipRecordNumber = parameter;


        //WardshipRecordToCreate.WardshipRecordNumber = "R0000" + DateTime.Now.Second + "/2013";
        WardshipRecordToCreate.DateEntered = DateTime.Now;

        #endregion

        db.WardshipRecords.Add(WardshipRecordToCreate);
        db.SaveChanges();
        }
    }
}