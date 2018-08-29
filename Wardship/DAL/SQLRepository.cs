using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wardship.Models;
using System.Security.Principal;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

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
                User usr = db.Users.Where(x => x.Name == userName).FirstOrDefault();
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
            User usr = db.Users.Where(x => x.Name == userName).FirstOrDefault();
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
            User usr = db.Users.Single(x=>x.Name==Name);
            usr.LastActive = DateTime.Now;
            db.SaveChanges();
            return usr;
        }
        void ISQLRepository.UpdateUser(User model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            //Original update user.
            //db.Entry(model).State = EntityState.Modified;
            //db.SaveChanges();
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
           return db.WardshipRecord.Where(x =>x.FileNumber.Contains(p)).ToList();
        }

        IEnumerable<WardshipRecord> ISQLRepository.QuickSearchSurname(string p)
        {
            return db.WardshipRecord.Where(x => x.ChildSurname.Contains(p)).ToList();
        }

        public IEnumerable<WardshipRecord> QuickSearchByForename(string p)
        {
            return db.WardshipRecord.Where(x => x.ChildForenames.Contains(p)).ToList();
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
    }
}