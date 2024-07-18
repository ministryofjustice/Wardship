namespace Wardship.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCAFCASSInvolvedToWardshipRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CAFCASSInvolvedIDs",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.WardshipRecords", "CAFCASSInvolvedID", c => c.Int(nullable: false));
            DropColumn("dbo.WardshipRecords", "CAFCASSInvolved");
        }
        
        public override void Down()
        {
            AddColumn("dbo.WardshipRecords", "CAFCASSInvolved", c => c.Boolean(nullable: false));
            DropColumn("dbo.WardshipRecords", "CAFCASSInvolvedID");
            DropTable("dbo.CAFCASSInvolvedIDs");
        }
    }
}
