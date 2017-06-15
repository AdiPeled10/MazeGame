namespace Project.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Project.Models.ProjectContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Project.Models.ProjectContext context)
        {
            context.Users.AddOrUpdate(x => x.UserName,
                new User() { UserName = "Lior", Password = "als", Email = "lio@gmail.com" },
                new User() { UserName = "Moshe", Password = "kkd", Email = "moshe@gmail.com" },
                new User() { UserName = "spetix", Password = "Russ", Email = "rus@gmail.com" });
            context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
