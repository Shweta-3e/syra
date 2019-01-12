namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LoginTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseLogins",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        UserID = c.String(),
                        Token = c.String(),
                        LoginDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BaseLogins");
        }
    }
}
