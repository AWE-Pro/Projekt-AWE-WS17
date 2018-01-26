using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public virtual ICollection<Enrollment> Enrollments { get; set; }
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

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Enrollment> Enrollments { get; set; }
        public virtual DbSet<ContentGroup> ContentGroups { get; set; }
        public virtual DbSet<ContentElement> ContentElements { get; set; }
        public virtual DbSet<Type> Types { get; set; }

    }

    public class Course
    {
        public int ID { get; set; }
        [Display(Name = "Kursname")]
        public string Title { get; set; }
        public string Description { get; set; }
        public int Owner { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<ContentGroup> ContentGroups { get; set; }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }

    public class Tag
    {
        public int ID { get; set; }
        [Display(Name = "Tag-Name")]
        public string Name { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }

    public class Enrollment
    {
        [Key]
        [Column(Order = 3)]
        public DateTime Date { get; set; }
        [Key]
        [Column(Order = 1)]
        public string UserID { get; set; }
        [Key]
        [ForeignKey("Course")]
        [Column(Order = 2)]
        public int CourseID { get; set; }
        [Range(0,5)]
        public int Rating { get; set; }

        [ForeignKey("UserID")]
        public virtual ApplicationUser User { get; set; }
        public virtual Course Course { get; set; }
    }

    public class ContentGroup
    {
        [Key]
        [Column(Order = 1)]
        public int CourseID { get; set; }
        [Key]
        [ForeignKey("ContentElement")]
        [Column(Order = 2)]
        public int ContentID { get; set; }
        public int Order { get; set; }
        [Key]
        [Column(Order = 3)]
        public string Header { get; set; }

        public virtual Course Course { get; set; }
        public virtual ContentElement ContentElement { get; set; }
    }

    public class ContentElement
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int TypeID { get; set; }
        public int Order { get; set; }

        public virtual ICollection<ContentGroup> ContentGroups { get; set; }
        public virtual Type Type { get; set; }
    }

    public class Type
    {
        public int ID { get; set; }
        [Display(Name = "Type-Name")]
        public string Name { get; set; }

        public virtual ICollection<ContentElement> ContentElements { get; set; }
    }
}