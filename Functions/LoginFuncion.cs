using DS_System.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DS_System.Models;

namespace DS_System.Functions
{
    public class LoginFuncion
    {
        puntoDeventaEntities puntoDeventa = new puntoDeventaEntities();
        public bool ValidarLogin(UsuariosEntities usuario)
        {
            bool respuesta = false;
            Usuarios user = puntoDeventa.Usuarios.FirstOrDefault(x => x.User_Name == usuario.UserName && x.Password == usuario.Password);
            if (user != null)
            {
                respuesta = true;
            }
            return respuesta;
        }
    }
}