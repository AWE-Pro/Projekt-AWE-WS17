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
                new Course { Title = "Volkswirtschaftslehre", Description = "Angebot und Nachfrage", Owner = "testperson" },
                new Course { Title = "Algorithmen und Datenstrukturen", Description = "Programmier Basics Java", Owner = "testperson2" },
                new Course { Title = "Data Mining", Description = "Mining Methoden und Neuronale Netzwerke", Owner = "testperson3"  }
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

            var types = new List<Models.Type>
            {
                new Models.Type {Name ="Video"},
                new Models.Type {Name ="PDF"},
                new Models.Type {Name ="Text"}
            };
            types.ForEach(s => context.Types.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();



            var contentGroups = new List<ContentGroup>
            {
                new ContentGroup {CourseID = 1, Order = 1, Header = "Wirtschaft"},
                new ContentGroup {CourseID = 2, Order = 2, Header = "Rechnen"},
                new ContentGroup {CourseID = 3, Order = 1, Header = "Formeln"},
                new ContentGroup {CourseID = 1, Order = 2, Header = "Programmieren"},
                new ContentGroup {CourseID = 2, Order = 1, Header = "Algorithmen"},
                new ContentGroup {CourseID = 3, Order = 2, Header = "Methoden"}
            };
            contentGroups.ForEach(s => context.ContentGroups.AddOrUpdate(p => p.Header, s));
            context.SaveChanges();

            var contentElements = new List<ContentElement>
            {
                new ContentElement {ContentID = 1, Description ="Tolles ContentElement im Video-Format", Url ="bluppbluppblupp",TypeID= 1, Order =1},
                new ContentElement {ContentID = 2, Description ="Tolles ContentElement im PDF-Format", Url ="blablabla",TypeID=2, Order=2},
                new ContentElement {ContentID = 3, Description ="Tolles ContentElement im Text-Format", Url ="uiuiui",TypeID=3, Order=1},
                new ContentElement {ContentID = 4, Description ="weiteres Tolles ContentElement im Video-Format", Url ="bluppbluppblupp",TypeID= 1, Order =1},
                new ContentElement {ContentID = 5, Description ="weiteres Tolles ContentElement im PDF-Format", Url ="blablabla",TypeID=2, Order=1},
                new ContentElement {ContentID = 6, Description ="weiteres Tolles ContentElement im Text-Format", Url ="uiuiui",TypeID=3, Order=1},
                new ContentElement {ContentID = 1, Description ="NOCH ein weiteres Tolles ContentElement im PDF-Format", Url ="blululul",TypeID=2, Order=2},
                new ContentElement {ContentID = 2, Description ="NOCH ein weiteres Tolles ContentElement im Text-Format", Url ="blaaaulul",TypeID=3, Order=1}
            };
            contentElements.ForEach(s => context.ContentElements.AddOrUpdate(p => p.Description, s));
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
