using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using DS_System.Models;

namespace DS_System.Controllers
{
    public class ProveedoresController : Controller
    {
        private puntoDeventaEntities db = new puntoDeventaEntities();

        // GET: Proveedores
        public ActionResult Index()
        {
            return View(db.Proveedores.ToList());
        }

        // GET: Proveedores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedores proveedores = db.Proveedores.Find(id);
            if (proveedores == null)
            {
                return HttpNotFound();
            }
            return View(proveedores);
        }

        // GET: Proveedores/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProveedor,NombreContacto,Rfc,Direccion,Telefono,email,Estatus,ContactoImagen")] Proveedores proveedores)
        {
            HttpPostedFileBase FileBase = Request.Files[0];

            WebImage image = new WebImage(FileBase.InputStream);
            proveedores.ContactoImagen = image.GetBytes();


            if (ModelState.IsValid)
            {
                db.Proveedores.Add(proveedores);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(proveedores);
        }

        // GET: Proveedores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedores proveedores = db.Proveedores.Find(id);
            if (proveedores == null)
            {
                return HttpNotFound();
            }
            return View(proveedores);
        }

        // POST: Proveedores/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProveedor,NombreContacto,Rfc,Direccion,Telefono,email,Estatus, ContactoImagen")] Proveedores proveedores)
        {
            byte[] imagenActual = null;
            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase == null)
            {
                imagenActual = db.Proveedores.SingleOrDefault(x => x.IdProveedor == proveedores.IdProveedor).ContactoImagen;
            }
            else 
            {
                WebImage image = new WebImage(FileBase.InputStream);
                proveedores.ContactoImagen = image.GetBytes();
            }



            if (ModelState.IsValid)
            {
                db.Entry(proveedores).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(proveedores);
        }

        // GET: Proveedores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proveedores proveedores = db.Proveedores.Find(id);
            if (proveedores == null)
            {
                return HttpNotFound();
            }
            return View(proveedores);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proveedores proveedores = db.Proveedores.Find(id);
            db.Proveedores.Remove(proveedores);
            db.SaveChanges();
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
        public ActionResult getImagen(int id) { 
         Proveedores prove = db.Proveedores.Find(id);
            byte[] ByteImage = prove.ContactoImagen;

            MemoryStream memoryStream = new MemoryStream(ByteImage);
            Image image = Image.FromStream(memoryStream);
            
            memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0;

            return File(memoryStream, "imagen/jpg");
        }
    }
}
