using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Wardship;
using Wardship.Models;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Wardship.Logger;

namespace Wardship.Tests
{
    class MockRepository : SourceRepository
    {
        private readonly ITelemetryLogger _logger;

        private HttpContextBase httpContext { get; set; }
        public MockRepository(ITelemetryLogger logger)
        {
            _logger = logger;
        }
        public MockRepository(RequestContext rc, ITelemetryLogger logger)
        {
            httpContext = rc.HttpContext;
            _logger = logger;
        }

        #region data
        IList<FAQ> testFAQS = new[] { 
                new FAQ() { faqID = 1, question = "Who", answer="Me", loggedInUser=false },
                new FAQ() { faqID = 2, question = "What", answer="Coder", loggedInUser=true  },
                new FAQ() { faqID = 3, question = "Why", answer="KHAN!", loggedInUser=true  },
                new FAQ() { faqID = 4, question = "Where", answer="Here", loggedInUser=true  }
            }.ToList();
        IList<ADGroup> groups = new[] {
            new ADGroup() { ADGroupID=1, Name="soldev\\gg_ssg_developer", RoleStrength=75 },
            new ADGroup() { ADGroupID=2, Name="soldev\\SSGDeveloper", RoleStrength=100 }
        }.ToList();
        IList<Role> roles = new[] {
            new Role{ Detail = "Admin",         strength=100},
            new Role{ Detail = "Manager",       strength=75},
            new Role{ Detail = "User",          strength=50},
            new Role{ Detail = "QuickSearch",   strength=35},
            new Role{ Detail = "ReadOnly",      strength=25},
            new Role{ Detail = "Deactive",      strength=0},
            new Role{ Detail = "Denied",        strength=-1}
        };
        IList<User> users = new[] {
            new User() { Name="cbruce", RoleStrength=50, DisplayName="Colin Bruce"},
            new User() { Name="dpenny", RoleStrength=25, DisplayName="Dan Penny"},
            new User() { Name="ijones", RoleStrength=-1, DisplayName="Ian Jones"},
            new User() { Name="oUser", RoleStrength=0, DisplayName="Old User"},
            new User() { Name="Admin", RoleStrength=100, DisplayName="An Admin"},
            new User() { Name="Manager", RoleStrength=75, DisplayName="A Manager"},
        }.ToList();
        IList<Alert> Alerts = new[] {
            new Alert{ AlertID=1, WarnStart=DateTime.Now.AddMinutes(60), EventStart=DateTime.Now.AddMinutes(240), RaisedHours=2, Live=true, Message="Alert Message 1"},
            new Alert{ AlertID=2, WarnStart=DateTime.Now.AddMinutes(-1), EventStart=DateTime.Now.AddMinutes(120), RaisedHours=1, Live=true, Message="Alert Message 2"},
            new Alert{ AlertID=3, WarnStart=DateTime.Now.AddMinutes(-60), EventStart=DateTime.Now.AddMinutes(30), RaisedHours=1, Live=true, Message="Alert Message 3"},
            new Alert{ AlertID=4, WarnStart=DateTime.Now.AddMinutes(-180), EventStart=DateTime.Now.AddMinutes(-60), RaisedHours=1, Live=true, Message="Alert Message 4"},
        };
        IList<Salutation> Salutations = new[] {
            new Salutation{ SalutationID=1, Detail="Mr", active=true, },
            new Salutation{ SalutationID=1, Detail="Mrs", active=true, },
        };
        IList<WordTemplate> Templates = new[]{
   //         new WordTemplate { active=true, Discriminator="Error", templateName="Return Letter to Court", templateXML="to be provided"}
            new WordTemplate { active=true, templateName="Return Letter to Court", templateXML="to be provided"}
        };
        #endregion
        #region FAQtests
        IEnumerable<Wardship.Models.FAQ> Wardship.SourceRepository.FAQsGetAll()
        {
            return testFAQS.ToList();
        }

        IEnumerable<Wardship.Models.FAQ> Wardship.SourceRepository.FAQsGetOnline()
        {
            return testFAQS.Where(f => f.loggedInUser == true).ToList();
        }

        IEnumerable<Wardship.Models.FAQ> Wardship.SourceRepository.FAQsGetOffline()
        {
            return testFAQS.Where(f => f.loggedInUser == false).ToList();
        }

        Wardship.Models.FAQ Wardship.SourceRepository.FAQGetbyID(int id)
        {
            throw new NotImplementedException();
        }

        void Wardship.SourceRepository.FAQUpdate(Wardship.Models.FAQ faq)
        {
            FAQ toAmend = testFAQS.Where(f => f.faqID == faq.faqID).Single();
            toAmend.question = faq.question;
            toAmend.answer = faq.answer;
            toAmend.loggedInUser = faq.loggedInUser;
        }

        void Wardship.SourceRepository.FAQAdd(Wardship.Models.FAQ faq)
        {
            testFAQS.Add(faq);
        }

        #endregion
        #region Audits
        IEnumerable<AuditEvent> SourceRepository.AuditEventsGetAll()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region ActiveDirectory
        IEnumerable<ADGroup> SourceRepository.GetAllGroups()
        {
            return groups.ToList();
        }
        IEnumerable<User> SourceRepository.GetAllUsers()
        {
            return users.ToList();
        }
        ADGroup SourceRepository.GetGroupByID(int id)
        {
            return groups.Single(x => x.ADGroupID == id);
        }
        AccessLevel SourceRepository.UserAccessLevel(IPrincipal User)
        {
            try
            {
                AccessLevel? grpLvl = null;
                AccessLevel? usrLvl = null;

                //groups
                IEnumerable<ADGroup> ADGroups = groups;
                foreach (var group in ADGroups.OrderByDescending(g => g.RoleStrength))
                {
                    if (User.IsInRole(group.Name))
                    {
                        grpLvl = (AccessLevel)group.RoleStrength;
                        break;
                    }
                }
                //Not in a group?  Try loading a user object
                try
                {
                    usrLvl = (AccessLevel)users.Single(x => x.Name == User.Identity.Name).RoleStrength;
                }
                catch { }
                if (usrLvl != null)
                {
                    return (AccessLevel)usrLvl;
                }
                else
                {
                    return grpLvl ?? AccessLevel.Denied;
                }
            }
            catch
            {
                //return an error code
                return AccessLevel.Denied;
            }
        }
        string SourceRepository.curUserDisplay(IPrincipal User)
        {
            string userName = ((string)User.Identity.Name).Split('\\').Last();
            User usr = users.Where(x => x.Name == userName).FirstOrDefault();
            if (usr != null)
            {
                return usr.DisplayName;
            }
            foreach (var group in groups.OrderByDescending(g => g.RoleStrength))
            {
                if (User.IsInRole(group.Name))
                {
                    return group.Name;
                }
            }
            return User.Identity.Name;
        }
        IEnumerable<Role> SourceRepository.GetAllRoles()
        {
            
            ICurrentUser usr = new ICurrentUser(httpContext.User.Identity, this);
            if (usr.AccessLevel == AccessLevel.Admin)
            {
            return roles.ToList();
            }
            else
            {
                return roles.Where(x => x.Detail != "Admin");
            }
        }
        void SourceRepository.UserAdd(User model)
        {
            users.Add(model);
        }
        User SourceRepository.GetUserByID(int id)
        {
            User user = users.Single(x => x.UserID == id);
            user.Role = roles.Single(x => x.strength == user.RoleStrength);
            return user;
        }
        User SourceRepository.GetUserByName(string Name)
        {
            User user = users.Single(x => x.Name == Name);
            user.Role = roles.Single(x => x.strength == user.RoleStrength);
            return user;
        }
        void SourceRepository.UpdateUser(User model)
        {
            User toAmend = users.Single(f => f.UserID == model.UserID);
            toAmend = model;
        }
        #endregion
        #region Alerts
        IEnumerable<Alert> SourceRepository.getCurrentAlerts()
        {
            return Alerts.Where(x => x.Live == true && x.WarnStart < DateTime.Now);
        }
        IEnumerable<Alert> SourceRepository.getAllAlerts()
        {
            return Alerts;
        }
        void SourceRepository.CreateAlert(Alert model)
        {
            Alerts.Add(model);
        }
        Alert SourceRepository.getAlertbyID(int id)
        {
            return Alerts.Single(x=>x.AlertID==id);
        }
        void SourceRepository.updateAlert(Alert model)
        {
            Alert alert = Alerts.Single(x => x.AlertID == model.AlertID);
            alert = model;
        }
        #endregion
        #region Salutations
        //IEnumerable<Salutation> SourceRepository.GetListofSalutations()
        //{
        //    return Salutations.ToList();
        //}
        //Salutation SourceRepository.GetSalutationByID(int id)
        //{
        //    return Salutations.FirstOrDefault(d => d.SalutationID == id);
        //}
        //void SourceRepository.CreateSalutation(Salutation model)
        //{
        //    Salutations.Add(model);
        //}
        //void SourceRepository.SalutationEditByID(Salutation model)
        //{
        //    throw new NotImplementedException();
        //}
        //Salutation SourceRepository.SalutationDeleteByID(int id)
        //{
        //    return Salutations.FirstOrDefault(d => d.SalutationID == id);
        //}
        //void SourceRepository.SalutationDeactivateByID(Salutation model)
        //{
        //    throw new NotImplementedException();
        //}
        #endregion
        #region Courts
        IEnumerable<Court> SourceRepository.getAllCourts()
        {
            throw new NotImplementedException();
        }
        void SourceRepository.CreateCourt(Court model)
        {
            throw new NotImplementedException();
        }

        void SourceRepository.EditCourt(Court model)
        {
            throw new NotImplementedException();
        }

        Court SourceRepository.getCourtByID(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
       
        #region Templates
        IEnumerable<WordTemplate> SourceRepository.GetAllTemplates()
        {
            return Templates.Where(x => x.active == true);
        }
        WordTemplate SourceRepository.GetTemplateByID(int id)
        {
            return Templates.Single(x => x.templateID == id);
        }
        void SourceRepository.UpdateTemplate(WordTemplate model)
        {
            throw new NotImplementedException();
        }
        void SourceRepository.DeactivateTemplate(WordTemplate model)
        {
            throw new NotImplementedException();
        }
        void SourceRepository.AddNewTemplate(WordTemplate model)
        {
            Templates.Add(model);
        }
        #endregion

     
        #region QuickSearch
        public IEnumerable<WardshipRecord> QuickSearchByNumber(string p)
        {
            throw new NotImplementedException();
        }
        
        public IEnumerable<WardshipRecord> QuickSearchSurname(string p)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<WardshipRecord> QuickSearchByForename(string p)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<WardshipRecord> QuickSearchByDOB(DateTime? DOB)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<WardshipRecord> QuickSearchByType(DateTime? DofOS)
        {
            throw new NotImplementedException();
        }

#endregion QuickSearch


        #region Wardships records and collections

        public WardshipRecord GetWardshipRecordByID(int id) //to get wardship details from the search page
        {
            throw new NotImplementedException();
        }
        public IEnumerable<WardshipRecord> WardshipsGetAll()
        {
            throw new NotImplementedException();
        }

        #endregion

        void IDisposable.Dispose()
        {

        }

        public void AddAuditEvent(AuditEvent Audit)
        {
            throw new NotImplementedException();
        }
    }
}
