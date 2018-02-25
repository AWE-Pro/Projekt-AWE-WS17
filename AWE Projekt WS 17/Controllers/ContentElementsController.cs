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

namespace AWE_Projekt_WS_17.Controllers
{
    public class ContentElementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ContentElements
        public async Task<ActionResult> Index()
        {
            var contentElements = db.ContentElements.Include(c => c.ContentGroup).Include(c => c.Type);
            return View(await contentElements.ToListAsync());
        }

        // GET: ContentElements/Details/5
        public async Task<ActionResult> Details(int? id)
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
            return View(contentElement);
        }

        // GET: ContentElements/Create
        public ActionResult Create()
        {
            ViewBag.ContentID = new SelectList(db.ContentGroups, "ID", "Header");
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name");
            return View();
        }

        // POST: ContentElements/Create
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,ContentID,Description,Url,TypeID,Order")] ContentElement contentElement)
        {
            if (ModelState.IsValid)
            {
                db.ContentElements.Add(contentElement);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ContentID = new SelectList(db.ContentGroups, "ID", "Header", contentElement.ContentID);
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name", contentElement.TypeID);
            return View(contentElement);
        }

        // GET: ContentElements/Edit/5
        public async Task<ActionResult> Edit(int? id)
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
            ViewBag.ContentID = new SelectList(db.ContentGroups, "ID", "Header", contentElement.ContentID);
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name", contentElement.TypeID);
            return View(contentElement);
        }

        // POST: ContentElements/Edit/5
        // Aktivieren Sie zum Schutz vor übermäßigem Senden von Angriffen die spezifischen Eigenschaften, mit denen eine Bindung erfolgen soll. Weitere Informationen 
        // finden Sie unter https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,ContentID,Description,Url,TypeID,Order")] ContentElement contentElement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contentElement).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ContentID = new SelectList(db.ContentGroups, "ID", "Header", contentElement.ContentID);
            ViewBag.TypeID = new SelectList(db.Types, "ID", "Name", contentElement.TypeID);
            return View(contentElement);
        }

        // GET: ContentElements/Delete/5
        public async Task<ActionResult> Delete(int? id)
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
            return View(contentElement);
        }

        // POST: ContentElements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ContentElement contentElement = await db.ContentElements.FindAsync(id);
            db.ContentElements.Remove(contentElement);
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
