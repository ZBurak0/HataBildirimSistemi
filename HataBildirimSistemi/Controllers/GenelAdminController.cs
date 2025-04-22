using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HataBildirimSistemi.Controllers
{
    public class GenelAdminController : Controller
    {
        HataBildirimModelMvcEntities entity = new HataBildirimModelMvcEntities();
        // GET: Admin
        public ActionResult Index()
        {
            int yetkiturıd = Convert.ToInt32(Session["AYetkiId"]);
            if (yetkiturıd == 4)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Login");
                
            }

        }
        [HttpGet]
        public ActionResult YetkiliServisEkle()
        {
            var model = new AdminPanelViewModel
            {
                ArizaTurleri = entity.ArızaTur.ToList() // ArizaTurleri listesi dolu geliyor
            };

            return View(model);
        }

        // Yetkili Servis Ekle
        [HttpPost]
        public ActionResult YetkiliServisEkle(string Ad, string Soyad, string TelNo, string Mail, string YKullaniciAd, string YSifre, int ArizaTurId)
        {
            YetkiliServis servis = new YetkiliServis
            {
                Ad = Ad,
                Soyad = Soyad,
                TelNo = TelNo,
                Mail = Mail,
                YKullaniciAd = YKullaniciAd,
                YSifre = YSifre,
                YetkiId = 3,
                ArizaTurId = ArizaTurId
            };
            entity.YetkiliServis.Add(servis);
            entity.SaveChanges();

            return RedirectToAction("YetkiliServisEkle");
        }


    }
}