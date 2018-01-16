using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AWE_Projekt_WS_17.Models
{
    // Sie können Profildaten für den Benutzer hinzufügen, indem Sie der ApplicationUser-Klasse weitere Eigenschaften hinzufügen. Weitere Informationen finden Sie unter https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Beachten Sie, dass der "authenticationType" mit dem in "CookieAuthenticationOptions.AuthenticationType" definierten Typ übereinstimmen muss.
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Benutzerdefinierte Benutzeransprüche hier hinzufügen
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class Course
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }
    }
    
    public class Tag
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Enrollment
    {
        public DateTime Date { get; set; }
        public int UserID { get; set; }
        public int CourseID { get; set; }
        public int Rating { get; set; }
    }

    public class ContentGroup
    {
        public int CourseID { get; set; }
        public int ContentID { get; set; }
        public int Order { get; set; }
        public string Header { get; set; }
    }

    public class ContentElement
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int TypeID { get; set; }
        public int Order { get; set; }
    }

    public class Type
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}