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
    public class ProductosController : Controller
    {
        private puntoDeventaEntities dbPr = new puntoDeventaEntities();

        // GET: Productos
        public ActionResult Index()
        {
            var productos = dbPr.Productos.Include(p => p.Clasificacion).Include(p => p.Proveedores);
            return View(productos.ToList());
        }

        // GET: Productos/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = dbPr.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // GET: Productos/Create
        public ActionResult Create()
        {
            ViewBag.IdClasificacion = new SelectList(dbPr.Clasificacion, "IdClasificacion", "Descripcion");
            ViewBag.IdProveedor = new SelectList(dbPr.Proveedores, "IdProveedor", "NombreContacto");
            return View();
        }

        // POST: Productos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CDBarras,IdProveedor,NommbreProduc,Descripcion,Stock,PrecioCom,PrecioVen,Caducidad,FechaRegis,IdClasificacion, ContactoImagen")] Productos productos)
        {
            HttpPostedFileBase FileBase = Request.Files[0];

            WebImage image = new WebImage(FileBase.InputStream);
            productos.ContactoImagen = image.GetBytes();

            if (ModelState.IsValid)
            {
                dbPr.Productos.Add(productos);
                dbPr.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdClasificacion = new SelectList(dbPr.Clasificacion, "IdClasificacion", "Descripcion", productos.IdClasificacion);
            ViewBag.IdProveedor = new SelectList(dbPr.Proveedores, "IdProveedor", "NombreContacto", productos.IdProveedor);
            return View(productos);
        }

        // GET: Productos/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = dbPr.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdClasificacion = new SelectList(dbPr.Clasificacion, "IdClasificacion", "Descripcion", productos.IdClasificacion);
            ViewBag.IdProveedor = new SelectList(dbPr.Proveedores, "IdProveedor", "NombreContacto", productos.IdProveedor);
            return View(productos);
        }

        // POST: Productos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CDBarras,IdProveedor,NommbreProduc,Descripcion,Stock,PrecioCom,PrecioVen,Caducidad,FechaRegis,IdClasificacion,ContactoImagen")] Productos productos)
        {

            byte[] imagenActual = null;
            HttpPostedFileBase FileBase = Request.Files[0];
            if (FileBase == null)
            {
                imagenActual = dbPr.Productos.SingleOrDefault(x => x.CDBarras == productos.CDBarras).ContactoImagen;
            }
            else
            {
                WebImage image = new WebImage(FileBase.InputStream);
                productos.ContactoImagen = image.GetBytes();
            }

            if (ModelState.IsValid)
            {
                dbPr.Entry(productos).State = EntityState.Modified;
                dbPr.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdClasificacion = new SelectList(dbPr.Clasificacion, "IdClasificacion", "Descripcion", productos.IdClasificacion);
            ViewBag.IdProveedor = new SelectList(dbPr.Proveedores, "IdProveedor", "NombreContacto", productos.IdProveedor);
            return View(productos);
        }

        // GET: Productos/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productos productos = dbPr.Productos.Find(id);
            if (productos == null)
            {
                return HttpNotFound();
            }
            return View(productos);
        }

        // POST: Productos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Productos productos = dbPr.Productos.Find(id);
            dbPr.Productos.Remove(productos);
            dbPr.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbPr.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult getImagen(int id)
        {
            Productos prove = dbPr.Productos.Find(id);
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
