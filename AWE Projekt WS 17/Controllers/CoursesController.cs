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
using System.Text.RegularExpressions;

namespace AWE_Projekt_WS_17.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Courses
        [Authorize]
        public async Task<ActionResult> Index()
        {
            string id = User.Identity.GetUserId();
            return View(await db.Courses.Where(x => x.Owner.Equals(id)).ToListAsync());
        }

        public async Task<ActionResult> ContentGroup(int id)
        {
            ViewBag.ID = id;
            var contentGroups = db.ContentGroups.Include(c => c.Course).Where(x => x.CourseID == id);
            return View(await contentGroups.OrderBy(x => x.Order).ToListAsync());
        }
        public ActionResult CreateContentGroup(int id)
        {
            ViewBag.CourseID = id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateContentGroup([Bind(Include = "ID,CourseID,Order,Header")] ContentGroup contentGroup)
        {
            if (ModelState.IsValid)
            {
                db.ContentGroups.Add(contentGroup);
                await db.SaveChangesAsync();
                return RedirectToAction("ContentGroup", new { id = contentGroup.CourseID });
            }

            ViewBag.CourseID = contentGroup.CourseID;
            return View(contentGroup);
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

        [Authorize]
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

        [Authorize]
        public ActionResult Course(int CourseId)
        {
            ViewBag.User = User.Identity.GetUserId();
            ViewBag.ID = CourseId;
            ViewBag.Identifier = CourseId.ToString();
            //Durchschnittliche Wertung
            List<int> average = new List<int>();
            for (int k = 0; k < db.Enrollments.Count(); k++)
            {
                if (db.Enrollments.ToList()[k].CourseID == CourseId && !db.Enrollments.ToList()[k].Rating.Equals(0))
                {
                    average.Add(db.Enrollments.ToList()[k].Rating);
                }

            }
            if (average.Count() > 0)
            {
                ViewBag.Average = average.Average();
            }
            else
            {
                ViewBag.Average = "Noch nicht genügend Wertungen!";
            }


            //Kursnamen heraussuchen
            for (int i = 0; i < db.Courses.Count(); i++)
            {
                if (db.Courses.ToList()[i].ID.Equals(CourseId))
                {
                    ViewBag.Course = db.Courses.ToList()[i].Title;
                }
            }
            ViewBag.Title = db.Courses.Where(x => x.ID.Equals(CourseId));
            //Prüfen ob User eingeloggt
            if (User.Identity.GetUserId() != null)
            {
                //überprüfen ob User bereits bewertet hat
                bool rating = false;
                for (int r = 0; r < db.Enrollments.Count(); r++)
                {
                    if (db.Enrollments.ToList()[r].UserID.Equals(User.Identity.GetUserId()) && db.Enrollments.ToList()[r].CourseID == CourseId && !db.Enrollments.ToList()[r].Rating.Equals(0))
                    {
                        rating = true;
                    }

                }
                if (rating == false)
                {
                    List<int> ratings = new List<int>() { 1, 2, 3, 4, 5 };
                    ViewBag.Rating = new SelectList(ratings);
                }
                else
                {
                    List<int> ratings = new List<int>();
                    ViewBag.Rating = new SelectList(ratings);
                }

                //Eintrag in Enrollments-Tabelle
                db.Enrollments.Add(new Enrollment { UserID = User.Identity.GetUserId(), CourseID = CourseId, Date = DateTime.Now, Rating = 0 });
                db.SaveChanges();
            }
            else
            {
                List<int> ratings = new List<int>();
                ViewBag.Rating = new SelectList(ratings);

            }
            List<ContentGroup> groups = db.ContentGroups.ToList().Where(x => x.CourseID.Equals(CourseId)).ToList().OrderBy(x => x.Order).ToList();
            for (int i = 0; i < groups.Count(); i++)
            {
                groups[i].ContentElements = groups[i].ContentElements.OrderBy(x => x.Order).ToList();

            }
            //Sortierte Liste zurückgeben
            return View(groups);
        }

        public ActionResult Rating(int rating, int id, string userid)
        {

            List<Enrollment> entrys = new List<Enrollment>();
            for (int i = 0; i < db.Enrollments.Count(); i++)
            {
                if (db.Enrollments.ToList()[i].UserID.Equals(userid) && db.Enrollments.ToList()[i].CourseID == id)
                {
                    entrys.Add(db.Enrollments.ToList()[i]);
                }
            }
            entrys = entrys.OrderByDescending(x => x.Date).ToList();
            Enrollment entry = entrys[0];

            db.Enrollments.Add(new Enrollment { UserID = userid, CourseID = id, Date = DateTime.Now, Rating = rating });
            db.SaveChanges();
            /** for (int i = 0; i < db.Enrollments.Count(); i++)
             {
                 if (db.Enrollments.ToList()[i].Equals(entry))
                 {
                     db.Enrollments.ToList()[i].Rating = rating;
                     db.SaveChanges();
                 }
             }*/

            return RedirectToAction("Course", new { CourseId = id });
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
            }
            return View(Courses);
            
        }

        public ActionResult CreateContentElement(String[] inputDescription, int[] inputType, String[] inputUrl, int[] inputOrder, int contentid)
        {
            if(inputDescription == null || inputType == null || inputUrl == null || inputOrder == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ContentGroup cG = db.ContentGroups.Find(contentid);

            // Content auslesen und erstelen
            for (int i=0; i < inputDescription.Length; i++)
            {
                ContentElement cE = new ContentElement();
                cE.Description = inputDescription[i];
                cE.TypeID = inputType[i];
                cE.Url = inputUrl[i];
                cE.Order = inputOrder[i];
                cE.ContentID = contentid;

                cG.ContentElements.Add(cE);
               
            }
            db.SaveChanges();
            int courseID = cG.CourseID;

                return RedirectToAction("Course", new { courseID });
        }


        // POST: Courses/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Title,Description,Owner")] Course course, string tags)
        {
            if (ModelState.IsValid)
            {
                tags = Regex.Replace(tags, @"\s+", "");
                List<string> allTags = tags.Split(',').ToList<string>();
                allTags.Reverse();

                List<string> allTagNames = new List<string>();
                for (int k = 0; k < db.Tags.ToList().Count; k++)
                {
                    allTagNames.Add(db.Tags.ToList()[k].Name);
                }

                for (int i = 0; i < allTags.Count(); i++)
                {
                    if (!allTagNames.Contains(allTags[i]))
                    {
                        allTagNames.Add(allTags[i]);
                        Tag tag = new Tag { Name = allTags[i] };
                        db.Tags.Add(tag);
                    }
                }
                course.Owner = User.Identity.GetUserId();

                db.Courses.Add(course);
                await db.SaveChangesAsync();
                List<Tag> taglist = new List<Tag>();
                for (int i = 0; i < db.Tags.Count(); i++)
                {
                    for (int k = 0; k < allTags.Count; k++)
                    {
                        if (db.Tags.ToList()[i].Name.Equals(allTags[k]))
                        {
                            taglist.Add(db.Tags.ToList()[i]);
                        }
                    }
                }


                for (int i = 0; i < db.Courses.Count(); i++)
                {
                    if (db.Courses.ToList()[i].ID == course.ID)
                    {
                        db.Courses.ToList()[i].Tags = taglist;
                    }
                }

                await db.SaveChangesAsync();
                List<Course> c = db.Courses.Where(x => x.ID == course.ID).ToList();

                return RedirectToAction("ContentGroup", new { id = c[0].ID });
            }
            return View(course);

        }


        // GET: Courses/Edit/5
        public async Task<ActionResult> Edit8(int? id)
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

        public async Task<ActionResult> Edit(int id)
        {
            ViewBag.ContentID = id;
            ViewBag.Counter = db.ContentElements.Where(x => x.ContentID.Equals(id)).Count();
            return View(await db.ContentElements.Where(x => x.ContentID.Equals(id)).ToListAsync());
        }
        public async Task<ActionResult> Edit2(int? id)
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
            ViewBag.ID = User.Identity.GetUserId();
            return View(course);
        }


        // POST: Courses/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit2([Bind(Include = "ID,Title,Description,Owner")] Course course)
        {

            if (ModelState.IsValid)
            {
                db.Entry(course).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ID = User.Identity.GetUserId();
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

        public async Task<ActionResult> DeleteContentGroup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentGroup contentGroup = await db.ContentGroups.FindAsync(id);
            if (contentGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = contentGroup.CourseID;    
            return View(contentGroup);
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

        [HttpPost, ActionName("DeleteContentGroup")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteContentGroupConfirmed(int id)
        {

            ContentGroup contentGroup = await db.ContentGroups.FindAsync(id);
            int temp = contentGroup.CourseID;
            db.ContentGroups.Remove(contentGroup);
            await db.SaveChangesAsync();
            return RedirectToAction("ContentGroup", new { id = temp });
        }
        public async Task<ActionResult> Edit3(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentGroup contentGroup = await db.ContentGroups.FindAsync(id);
            if (contentGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseID = contentGroup.CourseID;
            return View(contentGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit3([Bind(Include = "ID,CourseID,Order,Header")] ContentGroup contentGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contentGroup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ContentGroup", new { id = contentGroup.CourseID });
            }
            ViewBag.CourseID =contentGroup.CourseID;
            return View(contentGroup);
        }

        public async Task<ActionResult> DeleteElement(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentElement contentElement = await db.ContentElements.FindAsync(id);
            if (contentElement == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = contentElement.ContentID;
            return View(contentElement);
        }

        [HttpPost, ActionName("DeleteElement")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmedElement(int id)
        {
            ContentElement contentElement = await db.ContentElements.FindAsync(id);
            int ID = contentElement.ContentID;
            db.ContentElements.Remove(contentElement);
            await db.SaveChangesAsync();
            return RedirectToAction("Edit", new { id = ID});
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
