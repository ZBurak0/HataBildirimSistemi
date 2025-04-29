using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HataBildirimSistemi.Models;
using BCrypt.Net;


namespace HataBildirimSistemi.Controllers
{
    public class LoginController : Controller
    {
        HataBildirimModelMvcEntities3 entity = new HataBildirimModelMvcEntities3();
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Mesaj = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string KKullaniciAd, string KSifre)
        {

            // First check if username exists in any of the tables
            var kullanici = entity.Kullanici.FirstOrDefault(p => p.KKullaniciAd == KKullaniciAd);
            var admin = entity.Admin.FirstOrDefault(p => p.AKullaniciAd == KKullaniciAd);
            var yetkiliServis = entity.YetkiliServis.FirstOrDefault(p => p.YKullaniciAd == KKullaniciAd);

            // Check Kullanici table
            if (kullanici != null)
            {
                Session["KId"] = kullanici.Id;
                Session["KAd"] = kullanici.Ad;
                Session["KBirimId"] = kullanici.BirimId;
                Session["KYetkiId"] = kullanici.YetkiId;

                if (kullanici.YetkiId == 2)
                {
                    return RedirectToAction("Bildirim", "KUllanici");
                }
                if (kullanici.YetkiId == 1)
                {
                    switch (kullanici.BirimId)
                    {
                        case 1:
                            return RedirectToAction("Yazilim", "Admin");
                        case 2:
                            return RedirectToAction("Sistem", "Admin");
                        case 3:
                            return RedirectToAction("Ag", "Admin");
                    }
                }
                else
                {
                    ViewBag.Mesaj = "Kullanıcı adı veya şifre yanlış.";
                    return View();
                }
            }

            // Check Admin table
            if (admin != null)
            {
                Session["AId"] = admin.Id;
                Session["AAd"] = admin.Ad;

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
                if (admin.YetkiId == 4 && admin.BirimId == 4)
                {
                    return RedirectToAction("Index", "GenelAdmin");
                }
                else
                {
                    ViewBag.Mesaj = "Kullanıcı adı veya şifre yanlış.";
                    return View();
                }
            }

            // Check YetkiliServis table
            if (yetkiliServis != null)
            {
                Session["YYetkiId"] = yetkiliServis.YetkiId;
                Session["ArizaTurId"] = yetkiliServis.ArizaTurId;
                if (yetkiliServis.YetkiId == 3)
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
                else
                {
                    ViewBag.Mesaj = "Kullanıcı adı veya şifre yanlış.";
                    return View();
                }
            }

            ViewBag.Mesaj = "Kullanıcı adı veya şifre yanlış.";
            return View();
        }
        public ActionResult Register()
        {
            ViewBag.BirimList = entity.Birim.ToList();
            return View(new Kullanici());
        }

        [HttpPost]
        public ActionResult Register(Kullanici yeniKullanici)
        {
            if (ModelState.IsValid)
            {
                if (!yeniKullanici.KKullaniciAd.EndsWith("@akdeniz.edu.tr"))
                {
                    ModelState.AddModelError("KKullaniciAd", "Kullanıcı adı @akdeniz.edu.tr ile bitmelidir.");
                }

                var mevcutKullanici = entity.Kullanici.FirstOrDefault(x => x.KKullaniciAd == yeniKullanici.KKullaniciAd);
                if (mevcutKullanici != null)
                {
                    ModelState.AddModelError("KKullaniciAd", "Bu kullanıcı adı zaten alınmış.");
                }

                if (ModelState.IsValid)
                {
                    // Şifreyi hashle
                    //yeniKullanici.KSifre = BCrypt.Net.BCrypt.HashPassword(yeniKullanici.KSifre);

                    // Varsayılan yetki ata
                    yeniKullanici.YetkiId = 2;

                    // Veritabanına kaydet
                    entity.Kullanici.Add(yeniKullanici);
                    entity.SaveChanges();

                    return RedirectToAction("Index", "Login");
                }
            }

            ViewBag.BirimList = entity.Birim.ToList();
            return View(yeniKullanici);
        }

    }
}