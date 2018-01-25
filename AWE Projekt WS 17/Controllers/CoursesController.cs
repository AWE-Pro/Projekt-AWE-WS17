using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AWE_Projekt_WS_17.Models;
using Microsoft.AspNet.Identity;

namespace AWE_Projekt_WS_17.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Courses
        public async Task<ActionResult> Index()
        {
            return View(await db.Courses.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = await db.Courses.FindAsync(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public async Task<JsonResult> GetTags(string text)
        {
            return Json(await db.Tags.Where(x => x.Name.StartsWith(text)).ToListAsync(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Course(int CourseId)
        {
            for(int i=0; i< db.Courses.Count(); i++)
            {
                if (db.Courses.ToList()[i].ID.Equals(CourseId))
                {
                    ViewBag.Course = db.Courses.ToList()[i].Title;
                }
            }
            ViewBag.Title = db.Courses.Where(x => x.ID.Equals(CourseId));
            if (User.Identity.GetUserId() != null)
            {
                db.Enrollments.Add(new Enrollment { UserID = User.Identity.GetUserId(), CourseID = CourseId, Date = DateTime.Now, Rating = 0 });
                db.SaveChanges();
            }
            List<ContentGroup> groups = db.ContentGroups.ToList().Where(x => x.CourseID.Equals(CourseId)).ToList().OrderBy(x => x.Order).GroupBy(p => p.ContentElement.Order).SelectMany(k => k).ToList();
                     
            return View(groups);
        }

    

    public ActionResult SearchResult(Tag tagname)
    {
        List<Course> Courses = new List<Course>();
        if (tagname != null)
        {
            for (int k = 0; k < db.Courses.Count(); k++)
            {
                for (int i = 0; i < db.Courses.ToList()[k].Tags.Count(); i++)
                {
                    if (db.Courses.ToList()[k].Tags.ToList()[i].Name.Equals(tagname.Name))
                    {
                        Courses.Add(db.Courses.ToList()[k]);
                    }

                }
            }
            return View(Courses);
        }
        else
        {
            return View(Courses);
        }
    }


    // POST: Courses/Create
    // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
    // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Create([Bind(Include = "ID,Title,Description,Owner")] Course course)
    {
        if (ModelState.IsValid)
        {
            db.Courses.Add(course);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(course);
    }

    // GET: Courses/Edit/5
    public async Task<ActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Course course = await db.Courses.FindAsync(id);
        if (course == null)
        {
            return HttpNotFound();
        }
        return View(course);
    }

    // POST: Courses/Edit/5
    // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
    // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Edit([Bind(Include = "ID,Title,Description,Owner")] Course course)
    {
        if (ModelState.IsValid)
        {
            db.Entry(course).State = EntityState.Modified;
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(course);
    }

    // GET: Courses/Delete/5
    public async Task<ActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Course course = await db.Courses.FindAsync(id);
        if (course == null)
        {
            return HttpNotFound();
        }
        return View(course);
    }

    // POST: Courses/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> DeleteConfirmed(int id)
    {
        Course course = await db.Courses.FindAsync(id);
        db.Courses.Remove(course);
        await db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            db.Dispose();
        }
        base.Dispose(disposing);
    }
}
}
