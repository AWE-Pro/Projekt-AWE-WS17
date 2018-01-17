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

                new Course { Title = "Volkswirtschaftslehre", Description = "Angebot und Nachfrage", Owner = 18 },
                new Course { Title = "Algorithmen und Datenstrukturen", Description = "Programmier Basics Java", Owner = 11 },
                new Course { Title = "Data Mining", Description = "Mining Methoden und Neuronale Netzwerke", Owner = 3 }
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            var tags = new List<Tag>
            {
                new Tag {Name ="Awesome"},
                new Tag {Name ="Bad"},
                new Tag {Name ="Boring"},
                new Tag {Name ="Funny"}
            };
            tags.ForEach(s => context.Tags.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            AddOrUpdateTag(context, "Volkswirtschaftslehre", "Bad");
            AddOrUpdateTag(context, "Volkswirtschaftslehre", "Boring");
            AddOrUpdateTag(context, "Algorithmen und Datenstrukturen", "Boring");
            AddOrUpdateTag(context, "Data Mining", "Funny");
        }

        void AddOrUpdateTag(ApplicationDbContext context, string courseTitle, string tagName)
        {
            var crs = context.Courses.SingleOrDefault(c => c.Title == courseTitle);
            var tag = crs.Tags.SingleOrDefault(i => i.Name == tagName);
            if (tag == null)
                crs.Tags.Add(context.Tags.Single(i => i.Name == tagName));
        }

    }
}
