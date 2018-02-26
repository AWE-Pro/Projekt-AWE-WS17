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
                new Course { Title = "Data Mining", Description = "Mining Methoden und Neuronale Netzwerke", Owner = "testperson3"  },
                new Course { Title = "Advanced Web-Engineering", Description = "Entwicklung von Anwendungen mit ASP.NET", Owner = "testperson4" }
            };
            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Title, s));
            context.SaveChanges();

            var tags = new List<Tag>
            {
                new Tag {Name ="Awesome"},
                new Tag {Name ="Bad"},
                new Tag {Name ="Boring"},
                new Tag {Name ="Funny"},
                new Tag {Name ="Awe"},
                new Tag {Name ="Annyoing"},
                new Tag {Name ="Ajax"},
                new Tag {Name ="Asp"}
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
                new ContentGroup {CourseID = 3, Order = 1, Header = "Formeln"},
                new ContentGroup {CourseID = 4, Order = 1, Header = "Controllers & Views"},
                new ContentGroup {CourseID = 4, Order = 2, Header = "Client Components"}

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
                new ContentElement {ContentID = 2, Description ="Zusammenfassung", Url ="Wo schneiden sich Angebots- und Nachfragekurve?",TypeID=3, Order=1},
                new ContentElement {ContentID = 7, Description ="Aufbau eines MVC-Projekts", Url ="https://www.youtube.com/embed/f2s4k7q5Hd4",TypeID=1, Order=1},
                new ContentElement {ContentID = 7, Description ="Web-Formulare", Url ="https://www.youtube.com/embed/A-wgEGurKGw",TypeID=1, Order=2},
                new ContentElement {ContentID = 7, Description ="Aufgabenblatt", Url ="https://wuecampus2.uni-wuerzburg.de/moodle/pluginfile.php/1142027/mod_resource/content/0/AWE_Aufgabenblatt_3%2001%20FTH.pdf",TypeID=2, Order=3},
                new ContentElement {ContentID = 8, Description ="Kendo UI Components", Url ="https://www.youtube.com/embed/0QAEBbGwINk",TypeID=1, Order=1},
                new ContentElement {ContentID = 8, Description ="Kendo AutoComplete mit Server-Filterung", Url ="https://www.youtube.com/embed/YQNg4PwFLbk",TypeID=1, Order=2},
                new ContentElement {ContentID = 8, Description ="Hinweis", Url ="Kendo UI macht viele Probleme!",TypeID=3, Order=3}
            };
            contentElements.ForEach(s => context.ContentElements.AddOrUpdate(p => p.Description, s));
            context.SaveChanges();

            AddOrUpdateTag(context, "Volkswirtschaftslehre", "Bad");
            AddOrUpdateTag(context, "Volkswirtschaftslehre", "Boring");
            AddOrUpdateTag(context, "Algorithmen und Datenstrukturen", "Boring");
            AddOrUpdateTag(context, "Data Mining", "Funny");
            AddOrUpdateTag(context, "Advanced Web-Engineering", "Awe");
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
