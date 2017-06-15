namespace Project.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Password = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Password);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
