using System;
using System.Collections.Generic;
using Wardship.Models;
using System.Security.Principal;

namespace Wardship
{
    public interface ISQLRepository : IDisposable
    {
        #region FAQ
        IEnumerable<FAQ> FAQsGetAll();
        IEnumerable<FAQ> FAQsGetOnline();
        IEnumerable<FAQ> FAQsGetOffline();
        FAQ FAQGetbyID(int id);
        void FAQUpdate(FAQ faq);
        void FAQAdd(FAQ faq);
        #endregion

        #region Audit
        IEnumerable<AuditEvent> AuditEventsGetAll();
        #endregion

        #region Users and Groups
        IEnumerable<ADGroup> GetAllGroups();
        IEnumerable<User> GetAllUsers();
        ADGroup GetGroupByID(int id);
        AccessLevel UserAccessLevel(IPrincipal User);
        void UserAdd(User model);
        string curUserDisplay(IPrincipal User);
        IEnumerable<Role> GetAllRoles();
        User GetUserByID(int id);
        User GetUserByName(string Name);
        void UpdateUser(User model);
        #endregion

        #region Alerts
        IEnumerable<Alert> getAllAlerts();
        IEnumerable<Alert> getCurrentAlerts();
        void CreateAlert(Alert model);
        Alert getAlertbyID(int id);
        void updateAlert(Alert model);
        #endregion

        #region Wardships records and collections
        IEnumerable<WardshipRecord> WardshipsGetAll();
        WardshipRecord GetWardshipRecordByID(int id);
        #endregion

        #region Salutations
        //IEnumerable<Salutation> GetListofSalutations();
        //Salutation GetSalutationByID(int id);
        //void CreateSalutation(Salutation model);
        //void SalutationEditByID(Salutation model);
        //Salutation SalutationDeleteByID(int id);
        //void SalutationDeactivateByID(Salutation model);
        #endregion

        #region Courts
        IEnumerable<Court> getAllCourts();
        Court getCourtByID(int id);
        void CreateCourt(Court model);
        void EditCourt(Court model);
        #endregion

        #region Templates
        IEnumerable<WordTemplate> GetAllTemplates();
        WordTemplate GetTemplateByID(int id);
        void UpdateTemplate(WordTemplate model);
        void DeactivateTemplate(WordTemplate model);
        void AddNewTemplate(WordTemplate model);
        #endregion

        #region Quick Search
        IEnumerable<WardshipRecord> QuickSearchByNumber(string p);
        IEnumerable<WardshipRecord> QuickSearchSurname(string p);
        IEnumerable<WardshipRecord> QuickSearchByForename(string p);
        IEnumerable<WardshipRecord> QuickSearchByDOB(DateTime? DOB);
        IEnumerable<WardshipRecord> QuickSearchByType(DateTime? DofOS);
        #endregion Quick Search

        #region New Properties
        IEnumerable<CaseType> CaseTypes { get; }
        IEnumerable<Court> Courts { get; }
        IEnumerable<Wardship.Models.Type> Types { get; }
        IEnumerable<Gender> Genders { get; }
        IEnumerable<DistrictJudge> DistrictJudges { get; }
        IEnumerable<Record> Records { get; }
        IEnumerable<Lapsed> Lapseds { get; }
        IEnumerable<Status> Statuses { get; }
        IEnumerable<CWO> CWOs { get; }
        IEnumerable<CAFCASS> CAFCASSs { get; }
        IEnumerable<WardshipRecord> WardshipRecords { get; }
        void SaveChanges();
        void AddWardshipRecord(WardshipRecord record);
        void UpdateWardshipRecord(WardshipRecord record);
        #endregion

        void AddAuditEvent(AuditEvent Audit);
    }
}
