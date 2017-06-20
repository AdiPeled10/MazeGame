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
                        UserName = c.String(nullable: false, maxLength: 128),
                        Salt = c.String(nullable: false),
                        HashValue = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Wins = c.Short(nullable: false),
                        Loses = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
        }
    }
}
