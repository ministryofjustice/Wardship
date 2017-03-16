using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Wardship.Models
{// DBInitializer turned off applicatin is now live!

    //public class DBInitializer : DropCreateDatabaseIfModelChanges<DataContext> // DropCreateDatabaseAlways<DataContext> // CreateDatabaseIfNotExists<DataContext> // DropCreateDatabaseIfModelChanges<DataContext> 
    //{
    //    //ToDo - Update Initializer and remove Seed override before go live!
    //    protected override void Seed(DataContext context)
    //    {
    //        var faq = new List<FAQ>
    //        {
    //            new FAQ{loggedInUser=false, question="What is the Wardship Database?", answer="It is a system used by The Divorce section of the PRFD"},
    //            new FAQ{loggedInUser=false, question="Who supports the system?", answer="The system was developed, and is supported by, Solutions Development, a part of MoJ ICT.  All help call sshould be raised via the Atos Servicedesk on 0800 7830162"},
    //            new FAQ{loggedInUser=true, question="If I have a problem, how do I raise a call?", answer="Call the Atos Service desk on 0800 7830162"},
    //        };
    //        var a = new List<AuditEventDescription>
    //        {
    //            #region Audit Event Descriptions
    //            new AuditEventDescription{idAuditEventDescription=10, AuditDescription = "FAQs"},
    //            new AuditEventDescription{idAuditEventDescription=11, AuditDescription = "FAQ added"},
    //            new AuditEventDescription{idAuditEventDescription=12, AuditDescription = "FAQ amended"},
    //            new AuditEventDescription{idAuditEventDescription=13 ,AuditDescription = "FAQ deleted"},
    //            //new AuditEventDescription{idAuditEventDescription=0, AuditDescription = "XXXs"},
    //            //new AuditEventDescription{idAuditEventDescription=1, AuditDescription = "XXX added"},
    //            //new AuditEventDescription{idAuditEventDescription=2, AuditDescription = "XXX amended"},
    //            //new AuditEventDescription{idAuditEventDescription=3 ,AuditDescription = "XXX deleted"},
    //            #endregion
    //        };

    //        #region Access / AD Group

    //        var b = new List<Role>
    //        {
    //            new Role{ Detail = "Admin",         strength=100},
    //            new Role{ Detail = "Manager",       strength=75},
    //            new Role{ Detail = "User",          strength=50},
    //            new Role{ Detail = "QuickSearch",   strength=35},
    //            new Role{ Detail = "ReadOnly",      strength=25},
    //            new Role{ Detail = "Deactive",      strength=0},
    //            new Role{ Detail = "Denied",      strength=-1}
    //        };
    //        var c = new List<ADGroup>
    //        {
    //            new ADGroup { Name = "gg_ssg_developers", RoleStrength=100 },
    //            new ADGroup { Name = "SSGDevelopers", RoleStrength=50}
    //        };

    //        var d = new List<User>
    //        {
    //            new User { Name="cbruce", DisplayName="Colin Bruce", RoleStrength=100},
    //            new User { Name="gco09m", DisplayName="Mel Pierre", RoleStrength=100},
    //            new User { Name="tudin", DisplayName="Tehseen Udin", RoleStrength=0},
    //            new User { Name="klall", DisplayName="Kulwant Lall", RoleStrength=50},
    //        };


         

    //        a.ForEach(xx => context.AuditDescriptions.Add(xx));
    //        b.ForEach(xx => context.Roles.Add(xx));
    //        c.ForEach(xx => context.ADGroups.Add(xx));
    //        d.ForEach(xx => context.Users.Add(xx));
    //        //e.ForEach(xx => context.Courts.Add(xx));

    //        context.SaveChanges();
    //        faq.ForEach(xx => context.FAQs.Add(xx));
    //        context.SaveChanges();
    //        #endregion




    //        #region Wardship CaseType Seed
    //        var CaseType = new List<CaseType> 
    //            {   
    //                new CaseType {CaseTypeID = 1, Detail = "WG", Description = "Wardship",  },
    //                new CaseType {CaseTypeID = 2, Detail = "CA", Description = "Adoption", }
                   
    //             };
    //        CaseType.ForEach(s => context.CaseType.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Wardship Salutation Seed
    //        //var Salutation = new List<Salutation> 
    //        //    {   
    //        //        new Salutation {SalutationID = 1, Detail = "Mr", },
    //        //        new Salutation {SalutationID = 2, Detail = "Ms", },
    //        //        new Salutation {SalutationID = 3, Detail = "Mrs", },
    //        //        new Salutation {SalutationID = 4, Detail = "Miss", },
    //        //        new Salutation {SalutationID = 5, Detail = "Sir", },
    //        //        new Salutation {SalutationID = 6, Detail = "Lord", },
    //        //        new Salutation {SalutationID = 7, Detail = "Prof", }
    //        //      };
    //        //Salutation.ForEach(s => context.Salutations.Add(s));
    //        //context.SaveChanges();

    //        #endregion

    //        #region Wardship Court Seed
    //        //var Court = new List<Court> 
    //        //    {   
    //        //       new Court {CourtID = 1, CourtName="Abergavenny Magistrates' Court", AddressLine1="Tudor Street", AddressLine2=null, AddressLine3=null, AddressLine4=null, Town="Abergavenny", County="Monmouthshire", Country="Wales", Postcode="NP7 5DL", DX="DX 43665 Cwmbran"},
    //        //       new Court {CourtID = 2, CourtName="Aberystwyth Justice Centre", AddressLine1="Aberystwyth Justice Centre", AddressLine2="Y Lanfa", AddressLine3="Trefechan", AddressLine4=null, Town="Aberystwyth", County="Ceredigion", Country="Wales", Postcode="SY23 1AS", DX="DX 99560 Aberystwyth 2"},
    //        //       new Court {CourtID = 3, CourtName="Accrington County Court", AddressLine1="Office: Bradshawgate House", AddressLine2="1 Oak Street", AddressLine3=null, AddressLine4=null, Town="Accrington", County="Lancashire", Country="England", Postcode="BB5 1EQ", DX="DX 702645 Accrington 2"},
    //        //       new Court {CourtID = 4, CourtName="Accrington Magistrates' Court", AddressLine1="The Law Courts", AddressLine2="Manchester Road", AddressLine3=null, AddressLine4=null, Town="Accrington", County="Lancashire", Country="England", Postcode="BB5 2BH", DX=null},
    //        //       new Court {CourtID = 5, CourtName="Administrative Court", AddressLine1="Administrative Court Office", AddressLine2="The Royal Courts of Justice", AddressLine3="Strand", AddressLine4=null, Town=null, County="London", Country="England", Postcode="WC2A 2LL", DX="DX 44450 RCJ / Strand"},
    //        //    };
    //        //Court.ForEach(s => context.Courts.Add(s));
    //        //context.SaveChanges();
    //        #endregion



    //        #region Wardship Type Seed
    //        var Type = new List<Type> 
    //            {   
    //                new Type {TypeID = 1, Detail = "WD/GD", Description = "Wardship/Guardian",  },
    //                new Type {TypeID = 2, Detail = "GUARD", Description = "Guardian", },
    //                new Type {TypeID = 3, Detail = "WARD", Description = "Wardship", },
    //                new Type {TypeID = 4, Detail = "INHERENT JURIS", Description = "NHERENT JURIS", }
                   
    //             };
    //        Type.ForEach(s => context.Types.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Gender
    //        var Gender = new List<Gender> 
    //            {   
    //                new Gender {GenderID = 1, Detail = "M",  },
    //                new Gender {GenderID = 2, Detail = "F", }
                   
    //             };
    //        Gender.ForEach(s => context.Gender.Add(s));
    //        context.SaveChanges();

    //        #endregion

    //        #region Wardship Record Seed
    //        var Records = new List<Record> 
    //            {   
    //                new Record {RecordID = 1, Detail = "Sole",  },
    //                new Record {RecordID = 2, Detail = "Main", },
    //                new Record {RecordID = 3, Detail = "Xref", },
    //                new Record {RecordID = 4, Detail = "Xtra", }
    //             };
    //        Records.ForEach(s => context.Records.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Wardship DistrictJudge Seed
    //        //var DistrictJudge = new List<DistrictJudge> 
    //        //    {   
    //        //        new DistrictJudge {DistrictJudgeID = 1, Name = "District Judge CONN", },
    //        //        new DistrictJudge {DistrictJudgeID = 2, Name = "District Judge TURNER",  },
    //        //        new DistrictJudge {DistrictJudgeID = 3, Name = "District Judge GUEST",  },
    //        //        new DistrictJudge {DistrictJudgeID = 4, Name = "District Judge MORRIS",  }
                   
    //        //     };
    //        //DistrictJudge.ForEach(s => context.DistrictJudges.Add(s));
    //        //context.SaveChanges();
    //        #endregion

    //        #region Wardship Lapsed Seed
    //        var Lapsed = new List<Lapsed> 
    //            {   
    //                new Lapsed {LapsedID = 1, Detail = "Yes",  },
    //                new Lapsed {LapsedID = 2, Detail = "No", }
                   
    //             };
    //        Lapsed.ForEach(s => context.Lapsed.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Wardship Status Seed
    //        var Status = new List<Status> 
    //            {   
    //                new Status {StatusID = 1, Detail = "Dismissed",  },
    //                new Status {StatusID = 2, Detail = "Over age",  },
    //                new Status {StatusID = 3, Detail = "Discontinued",  },
    //                new Status {StatusID = 4, Detail = "Dewarded",  },
    //                new Status {StatusID = 5, Detail = "Withdrawn",  }
                   
    //             };
    //        Status.ForEach(s => context.Status.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Wardship CWO Seed
    //        var CWO = new List<CWO> 
    //            {   
    //                new CWO {CWOID = 1, Detail = "Yes", },
    //                new CWO {CWOID = 2, Detail = "No", }
                   
    //             };
    //        CWO.ForEach(s => context.CWO.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Wardship CAFCASS Seed
    //        var CAFCASS = new List<CAFCASS> 
    //            {   
    //                new CAFCASS {CAFCASSID = 1, Detail = "None",  Description = "None",},
    //                new CAFCASS {CAFCASSID = 2, Detail = "Minors", Description = "Minors"}, 
    //                new CAFCASS {CAFCASSID = 3, Detail = "Deff",  Description = "Deff",},
    //                new CAFCASS {CAFCASSID = 4, Detail = "Pltff", Description = "Pltff" }
                   
    //             };
    //        CAFCASS.ForEach(s => context.CAFCASS.Add(s));
    //        context.SaveChanges();
    //        #endregion

    //        #region Word template seeds
    //        var wordTemplate = new List<WordTemplate>
    //        {  

    //         //Search letter
    //         new WordTemplate {active=true, templateID=6, templateName = "Search Not Found", templateXML =  "Search Not Found"},
    //         new WordTemplate {active=true, templateID=7, templateName = "Search Found", templateXML =  "Search Found"}
    //        };



    //        wordTemplate.ForEach(s => context.WordTemplate.Add(s));
    //        context.SaveChanges();
    //        #endregion



    //    }
    //}
}