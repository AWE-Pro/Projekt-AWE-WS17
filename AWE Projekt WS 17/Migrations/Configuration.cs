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
                new Course { Title = "Volkswirtschaftslehre", Description = "Angebot und Nachfrage", Owner = 1 },
                new Course { Title = "Algorithmen und Datenstrukturen", Description = "Programmier Basics Java", Owner = 2 },
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
                new ContentGroup {CourseID = 1, Order = 2, Header = "Rechnen"},
                new ContentGroup {CourseID = 2, Order = 2, Header = "Programmieren"},
                new ContentGroup {CourseID = 2, Order = 1, Header = "Algorithmen"},
                new ContentGroup {CourseID = 3, Order = 2, Header = "Methoden"},
                new ContentGroup {CourseID = 3, Order = 1, Header = "Formeln"}
            };
            contentGroups.ForEach(s => context.ContentGroups.AddOrUpdate(p => p.Header, s));
            context.SaveChanges();

            var contentElements = new List<ContentElement>
            {
                new ContentElement {ContentID = 1, Description ="Kern des Faches", Url ="https://www.youtube.com/embed/k78OjoJZcVc?start=15",TypeID= 1, Order =1},
                new ContentElement {ContentID = 2, Description ="Meme", Url ="http://www.mvla.net/view/23112.pdf",TypeID=2, Order=2},
                new ContentElement {ContentID = 3, Description ="Guter Tipp", Url ="Versuche nicht einzuschlafen!",TypeID=3, Order=1},
                new ContentElement {ContentID = 4, Description ="Lernhilfe", Url ="https://www.youtube.com/embed/miomuSGoPzI?start=18",TypeID= 1, Order =1},
                new ContentElement {ContentID = 5, Description ="Prüfungsplan", Url ="https://www.uni-wuerzburg.de/fileadmin/32020000/WiWi-Bach/20172_2_Februar_PPlan_BA_ZR.pdf",TypeID=2, Order=1},
                new ContentElement {ContentID = 6, Description ="Das schlimmste am Fach", Url ="Neuronale Netzwerke",TypeID=3, Order=1},
                new ContentElement {ContentID = 6, Description ="Während der Klausur", Url ="https://www.youtube.com/embed/77sS5IuR0Gs",TypeID=1, Order=2},
                new ContentElement {ContentID = 2, Description ="Zusammenfassung", Url ="Wo schneiden sich Angebots- und Nachfragekurve?",TypeID=3, Order=1}
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
