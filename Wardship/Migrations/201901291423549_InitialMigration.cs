namespace Wardship.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ADGroups",
                c => new
                    {
                        ADGroupID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 80),
                        RoleStrength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ADGroupID)
                .ForeignKey("dbo.Roles", t => t.RoleStrength, cascadeDelete: true)
                .Index(t => t.RoleStrength);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        strength = c.Int(nullable: false),
                        Detail = c.String(nullable: false, maxLength: 20),
                    })
                .PrimaryKey(t => t.strength);
            
            CreateTable(
                "dbo.Alerts",
                c => new
                    {
                        AlertID = c.Int(nullable: false, identity: true),
                        Live = c.Boolean(nullable: false),
                        EventStart = c.DateTime(nullable: false),
                        RaisedHours = c.Int(nullable: false),
                        WarnStart = c.DateTime(nullable: false),
                        Message = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.AlertID);
            
            CreateTable(
                "dbo.AuditEventDescriptions",
                c => new
                    {
                        idAuditEventDescription = c.Int(nullable: false, identity: true),
                        AuditDescription = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.idAuditEventDescription);
            
            CreateTable(
                "dbo.AuditEvents",
                c => new
                    {
                        idAuditEvent = c.Int(nullable: false, identity: true),
                        EventDate = c.DateTime(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 40),
                        idAuditEventDescription = c.String(nullable: false),
                        ChildSurname = c.String(maxLength: 100),
                        ChildForenames = c.String(maxLength: 100),
                        ChildDateofBirth = c.DateTime(),
                        AuditEventDescription_idAuditEventDescription = c.Int(),
                    })
                .PrimaryKey(t => t.idAuditEvent)
                .ForeignKey("dbo.AuditEventDescriptions", t => t.AuditEventDescription_idAuditEventDescription)
                .Index(t => t.AuditEventDescription_idAuditEventDescription);
            
            CreateTable(
                "dbo.AuditEventDataRows",
                c => new
                    {
                        idAuditData = c.Int(nullable: false, identity: true),
                        idAuditEvent = c.Int(nullable: false),
                        ColumnName = c.String(nullable: false, maxLength: 200),
                        Was = c.String(nullable: false, maxLength: 200),
                        Now = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.idAuditData)
                .ForeignKey("dbo.AuditEvents", t => t.idAuditEvent, cascadeDelete: true)
                .Index(t => t.idAuditEvent);
            
            CreateTable(
                "dbo.CAFCASSes",
                c => new
                    {
                        CAFCASSID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 10),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CAFCASSID);
            
            CreateTable(
                "dbo.CaseTypes",
                c => new
                    {
                        CaseTypeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 2),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.CaseTypeID);
            
            CreateTable(
                "dbo.Courts",
                c => new
                    {
                        CourtID = c.Int(nullable: false, identity: true),
                        CourtName = c.String(nullable: false, maxLength: 100),
                        AddressLine1 = c.String(maxLength: 50),
                        AddressLine2 = c.String(maxLength: 50),
                        AddressLine3 = c.String(maxLength: 50),
                        AddressLine4 = c.String(maxLength: 50),
                        Town = c.String(maxLength: 30),
                        County = c.String(maxLength: 30),
                        Country = c.String(maxLength: 20),
                        Postcode = c.String(maxLength: 8),
                        DX = c.String(maxLength: 60),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.CourtID);
            
            CreateTable(
                "dbo.CWOes",
                c => new
                    {
                        CWOID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.CWOID);
            
            CreateTable(
                "dbo.DataUploads",
                c => new
                    {
                        DataUploadID = c.Int(nullable: false, identity: true),
                        UploadStarted = c.DateTime(nullable: false),
                        UploadedBy = c.String(),
                        FileName = c.String(),
                        FullPathandName = c.String(),
                        FileSize = c.Int(nullable: false),
                        UploadCompleted = c.DateTime(),
                        NumberofRows = c.Int(nullable: false),
                        NumberOfErrs = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DataUploadID);
            
            CreateTable(
                "dbo.DistrictJudges",
                c => new
                    {
                        DistrictJudgeID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.DistrictJudgeID);
            
            CreateTable(
                "dbo.FAQs",
                c => new
                    {
                        faqID = c.Int(nullable: false, identity: true),
                        loggedInUser = c.Boolean(nullable: false),
                        question = c.String(nullable: false, maxLength: 150),
                        answer = c.String(nullable: false, maxLength: 4000),
                    })
                .PrimaryKey(t => t.faqID);
            
            CreateTable(
                "dbo.Genders",
                c => new
                    {
                        GenderID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 1),
                    })
                .PrimaryKey(t => t.GenderID);
            
            CreateTable(
                "dbo.Lapseds",
                c => new
                    {
                        LapsedID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 3),
                    })
                .PrimaryKey(t => t.LapsedID);
            
            CreateTable(
                "dbo.Records",
                c => new
                    {
                        RecordID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 5),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.RecordID);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 15),
                    })
                .PrimaryKey(t => t.StatusID);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        TypeID = c.Int(nullable: false, identity: true),
                        Detail = c.String(maxLength: 20),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.TypeID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 150),
                        DisplayName = c.String(maxLength: 150),
                        LastActive = c.DateTime(),
                        RoleStrength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Roles", t => t.RoleStrength, cascadeDelete: true)
                .Index(t => t.RoleStrength);
            
            CreateTable(
                "dbo.WardshipRecords",
                c => new
                    {
                        WardshipCaseID = c.Int(nullable: false, identity: true),
                        ChildSurname = c.String(maxLength: 100),
                        ChildForenames = c.String(maxLength: 100),
                        ChildDateofBirth = c.DateTime(),
                        DateOfOS = c.DateTime(),
                        FileNumber = c.String(maxLength: 15),
                        FileDuplicate = c.String(maxLength: 10),
                        Xreg = c.String(maxLength: 150),
                        TypeID = c.Int(),
                        CourtID = c.Int(),
                        StatusID = c.Int(),
                        GenderID = c.Int(),
                        RecordID = c.Int(),
                        LapsedID = c.Int(),
                        CWOID = c.Int(),
                        DistrictJudgeID = c.Int(),
                        CaseTypeID = c.Int(),
                        CAFCASSID = c.Int(),
                        LapseLetterSent = c.DateTime(),
                        FirstAppointmentDate = c.DateTime(),
                        HearingDate = c.DateTime(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.WardshipCaseID)
                .ForeignKey("dbo.CAFCASSes", t => t.CAFCASSID)
                .ForeignKey("dbo.CaseTypes", t => t.CaseTypeID)
                .ForeignKey("dbo.Courts", t => t.CourtID)
                .ForeignKey("dbo.CWOes", t => t.CWOID)
                .ForeignKey("dbo.DistrictJudges", t => t.DistrictJudgeID)
                .ForeignKey("dbo.Genders", t => t.GenderID)
                .ForeignKey("dbo.Lapseds", t => t.LapsedID)
                .ForeignKey("dbo.Records", t => t.RecordID)
                .ForeignKey("dbo.Status", t => t.StatusID)
                .ForeignKey("dbo.Types", t => t.TypeID)
                .Index(t => t.TypeID)
                .Index(t => t.CourtID)
                .Index(t => t.StatusID)
                .Index(t => t.GenderID)
                .Index(t => t.RecordID)
                .Index(t => t.LapsedID)
                .Index(t => t.CWOID)
                .Index(t => t.DistrictJudgeID)
                .Index(t => t.CaseTypeID)
                .Index(t => t.CAFCASSID);
            
            CreateTable(
                "dbo.WordTemplates",
                c => new
                    {
                        templateID = c.Int(nullable: false, identity: true),
                        templateName = c.String(nullable: false, maxLength: 80),
                        templateXML = c.String(nullable: false),
                        active = c.Boolean(nullable: false),
                        deactivated = c.DateTime(),
                        deactivatedBy = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.templateID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WardshipRecords", "TypeID", "dbo.Types");
            DropForeignKey("dbo.WardshipRecords", "StatusID", "dbo.Status");
            DropForeignKey("dbo.WardshipRecords", "RecordID", "dbo.Records");
            DropForeignKey("dbo.WardshipRecords", "LapsedID", "dbo.Lapseds");
            DropForeignKey("dbo.WardshipRecords", "GenderID", "dbo.Genders");
            DropForeignKey("dbo.WardshipRecords", "DistrictJudgeID", "dbo.DistrictJudges");
            DropForeignKey("dbo.WardshipRecords", "CWOID", "dbo.CWOes");
            DropForeignKey("dbo.WardshipRecords", "CourtID", "dbo.Courts");
            DropForeignKey("dbo.WardshipRecords", "CaseTypeID", "dbo.CaseTypes");
            DropForeignKey("dbo.WardshipRecords", "CAFCASSID", "dbo.CAFCASSes");
            DropForeignKey("dbo.Users", "RoleStrength", "dbo.Roles");
            DropForeignKey("dbo.AuditEventDataRows", "idAuditEvent", "dbo.AuditEvents");
            DropForeignKey("dbo.AuditEvents", "AuditEventDescription_idAuditEventDescription", "dbo.AuditEventDescriptions");
            DropForeignKey("dbo.ADGroups", "RoleStrength", "dbo.Roles");
            DropIndex("dbo.WardshipRecords", new[] { "CAFCASSID" });
            DropIndex("dbo.WardshipRecords", new[] { "CaseTypeID" });
            DropIndex("dbo.WardshipRecords", new[] { "DistrictJudgeID" });
            DropIndex("dbo.WardshipRecords", new[] { "CWOID" });
            DropIndex("dbo.WardshipRecords", new[] { "LapsedID" });
            DropIndex("dbo.WardshipRecords", new[] { "RecordID" });
            DropIndex("dbo.WardshipRecords", new[] { "GenderID" });
            DropIndex("dbo.WardshipRecords", new[] { "StatusID" });
            DropIndex("dbo.WardshipRecords", new[] { "CourtID" });
            DropIndex("dbo.WardshipRecords", new[] { "TypeID" });
            DropIndex("dbo.Users", new[] { "RoleStrength" });
            DropIndex("dbo.AuditEventDataRows", new[] { "idAuditEvent" });
            DropIndex("dbo.AuditEvents", new[] { "AuditEventDescription_idAuditEventDescription" });
            DropIndex("dbo.ADGroups", new[] { "RoleStrength" });
            DropTable("dbo.WordTemplates");
            DropTable("dbo.WardshipRecords");
            DropTable("dbo.Users");
            DropTable("dbo.Types");
            DropTable("dbo.Status");
            DropTable("dbo.Records");
            DropTable("dbo.Lapseds");
            DropTable("dbo.Genders");
            DropTable("dbo.FAQs");
            DropTable("dbo.DistrictJudges");
            DropTable("dbo.DataUploads");
            DropTable("dbo.CWOes");
            DropTable("dbo.Courts");
            DropTable("dbo.CaseTypes");
            DropTable("dbo.CAFCASSes");
            DropTable("dbo.AuditEventDataRows");
            DropTable("dbo.AuditEvents");
            DropTable("dbo.AuditEventDescriptions");
            DropTable("dbo.Alerts");
            DropTable("dbo.Roles");
            DropTable("dbo.ADGroups");
        }
    }
}
