using DS_System.Functions;
using DS_System.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DS_System.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        LoginFuncion servicio = new LoginFuncion();
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UsuariosEntities usuarios)
        {
            string mensaje = "";
            if (ModelState.IsValid)
            {
                if (servicio.ValidarLogin(usuarios))
                {
                    Session["SessionAdmin"] = usuarios.UserName;
                    return RedirectToAction("Index", "Productos");
                }
                else
                {
                    mensaje = "Usuario o contraseña incorrectos";
                }
                
            }
            ViewBag.Message=mensaje;
            return View("Login");
        }

    }
}