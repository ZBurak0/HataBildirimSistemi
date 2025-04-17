using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HataBildirimSistemi.Models;

namespace HataBildirimSistemi.Controllers
{
    public class LoginController : Controller
    {
        HataBildirimModelMvcEntities entity = new HataBildirimModelMvcEntities();
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Mesaj = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string KKullaniciAd, string KSifre)
        {

            var kullanici = (from p in entity.Kullanici
                             where p.KKullaniciAd == KKullaniciAd
                             && p.KSifre == KSifre
                             select p).FirstOrDefault();

            var admin = (from p in entity.Admin
                         where p.AKullaniciAd == KKullaniciAd
                         && p.ASifre == KSifre
                         select p).FirstOrDefault();

            var yetkiliServis = (from p in entity.YetkiliServis
                                 where p.YKullaniciAd == KKullaniciAd
                                 && p.YSifre == KSifre
                                 select p).FirstOrDefault();

            if (kullanici != null)
            {
                Session["Id"] = kullanici.Id;
                Session["Ad"] = kullanici.Ad;
                Session["KBirimId"] = kullanici.BirimId;
                Session["KYetkiId"] = kullanici.YetkiId;

                return RedirectToAction("Bildirim", "Kullanici");

            }
            if (admin != null)
            {
                Session["ABirimId"] = admin.BirimId;
                Session["AYetkiId"] = admin.YetkiId;

                if (admin.YetkiId == 1)
                {
                    switch (admin.BirimId)
                    {
                        case 1:
                            return RedirectToAction("Yazilim", "Admin");
                        case 2:
                            return RedirectToAction("Sistem", "Admin");
                        case 3:
                            return RedirectToAction("Ag", "Admin");
                    }
                }
            }
            if (yetkiliServis != null)
            {
                Session["YYetkiId"] = yetkiliServis.YetkiId;
                Session["ArizaTurId"] = yetkiliServis.ArizaTurId;
                if(yetkiliServis.YetkiId ==3)
                {
                    switch (yetkiliServis.ArizaTurId)
                    {
                        case 1:
                            return RedirectToAction("Elektrik", "YetkiliServis");
                        case 2:
                            return RedirectToAction("SuTesisat", "YetkiliServis");
                        case 3:
                            return RedirectToAction("Yazilim", "YetkiliServis");
                        case 4:
                            return RedirectToAction("Donanim", "YetkiliServis");
                    }
                }    
            }
            ViewBag.Mesaj = "Kullanıcı adı veya şifre yanlış.";
            return View();
        }


    }
}