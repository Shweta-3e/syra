namespace Syra.Admin.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbChanged : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ManageDbs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Intent = c.String(),
                        Entity = c.String(),
                        Response = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ManageDbs");
        }
    }
}
