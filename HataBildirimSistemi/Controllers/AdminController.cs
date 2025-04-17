using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HataBildirimSistemi.Controllers
{
    public class AdminController : Controller
    {
        HataBildirimModelMvcEntities entity = new HataBildirimModelMvcEntities();
        // GET: Admin
        public ActionResult Index()
        {
            int yetkiturıd = Convert.ToInt32(Session["AYetkiId"]);
            if (yetkiturıd == 1)
            {
                return View();
            }
            else
            {
                RedirectToAction("Index", "Login");
            }
            return View();
        }

    }
}