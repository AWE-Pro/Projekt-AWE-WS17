namespace AWE_Projekt_WS_17.Migrations
{
    using AWE_Projekt_WS_17.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<AWE_Projekt_WS_17.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "AWE_Projekt_WS_17.Models.ApplicationDbContext";
        }

        protected override void Seed(AWE_Projekt_WS_17.Models.ApplicationDbContext context)
        {
            var courses = new List<Course>
            {
                new Course
                {
                    ID = 1,
                    Title = "AWE",
                    Description = "Isn geiles Fach",
                    Owner = 1,
                    Tags = new List<Tag>
                {
                    new Tag
                    {
                        ID = 1,
                        Name = "Awesome",
                    }
                }
            }

            };
            courses.ForEach(course => context.Courses.AddOrUpdate(x => x.ID, course));

        }
    }
}
