using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HataBildirimSistemi.Models;
using BCrypt.Net;
using System.Net.Mail;
using System.Net;
using BCrypt.Net;
using ClosedXML.Excel;
using System.IO;

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
            var kullanici = entity.Kullanici.FirstOrDefault(p => p.KKullaniciAd == KKullaniciAd);

            if (kullanici != null)
            {
                if (KSifre == kullanici.KSifre)
                {
                    Session["KId"] = kullanici.Id;
                    Session["KUAd"] = kullanici.KKullaniciAd;
                    Session["KAd"] = kullanici.Ad;
                    Session["KBirimId"] = kullanici.BirimId;
                    Session["KYetkiId"] = kullanici.YetkiId;
                    Session["ArızaTurYet"] = kullanici.ArizaTurId;

                    if (kullanici.YetkiId == 2)
                    {
                        return RedirectToAction("Bildirim", "Kullanici");
                    }
                    else if (kullanici.YetkiId == 1)
                    {
                        return RedirectToAction("ArizaGoruntule", "BirimAdmin");
                    }
                    else if (kullanici.YetkiId == 3)
                    {
                        return RedirectToAction("ArizaTakip", "YetkiliServis");
                    }
                    else
                    {
                        ViewBag.Mesaj = "Yetki tanımlı değil.";
                        return View();
                    }
                }

                ViewBag.Mesaj = "Kullanıcı adı veya şifre yanlış.";
                return View();
            }

            // ❗Eksik olan kısım buydu — kullanıcı bulunamazsa buraya düşer
            ViewBag.Mesaj = "Kullanıcı bulunamadı.";
            return View();
        }

        public ActionResult Register()
        {
            ViewBag.BirimList = entity.Birim.Where(k => k.Id != 4).ToList();
            return View(new Kullanici());
        }

        [HttpPost]
        public ActionResult Register(Kullanici yeniKullanici)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcı adı @akdeniz.edu.tr ile bitmeli
                if (!yeniKullanici.KKullaniciAd.EndsWith("@akdeniz.edu.tr"))
                {
                    ModelState.AddModelError("KKullaniciAd", "Kullanıcı adı @akdeniz.edu.tr ile bitmelidir.");
                }
                else
                {
                    // Aynı kullanıcı adı var mı kontrol et
                    var mevcutKullanici = entity.Kullanici.FirstOrDefault(x => x.KKullaniciAd == yeniKullanici.KKullaniciAd);
                    if (mevcutKullanici != null)
                    {
                        ModelState.AddModelError("KKullaniciAd", "Bu kullanıcı adı zaten alınmış.");
                    }
                }

                // Hata yoksa kayıt yap
                if (ModelState.IsValid)
                {
                    // Şifre hashle (istersen aktif et)
                    // yeniKullanici.KSifre = BCrypt.Net.BCrypt.HashPassword(yeniKullanici.KSifre);

                    // Varsayılan yetki
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
        // Şifremi Unuttum (GET)
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // Şifremi Unuttum (POST)
        [HttpPost]
        public ActionResult ForgotPassword(string KKullaniciAd)
        {
            var kullanici = entity.Kullanici.FirstOrDefault(x => x.KKullaniciAd == KKullaniciAd);
            if (kullanici == null)
            {
                ViewBag.Message = "Böyle bir kullanıcı bulunamadı.";
                return View();
            }

            string kod = new Random().Next(100000, 999999).ToString();
            TempData["OnayKodu"] = kod;
            TempData["KKullaniciAd"] = KKullaniciAd;

            ViewBag.Message = $"Onay Kodunuz: {kod} (Demo amaçlı burada gösterilmektedir)";
            return View("OnayKoduGir");
        }

        // Onay Kodu Doğrulama (POST)
        [HttpPost]
        public ActionResult VerifyCode(string kod)
        {
            if (TempData["OnayKodu"] != null && kod == TempData["OnayKodu"].ToString())
            {
                string kullaniciAd = TempData["KKullaniciAd"]?.ToString();
                return RedirectToAction("ResetPassword", new { kullaniciAd });
            }

            ViewBag.Hata = "Onay kodu geçersiz!";
            return View("OnayKoduGir");
        }

        // Yeni Şifre Sayfası (GET)
        public ActionResult ResetPassword(string kullaniciAd)
        {
            return View((object)kullaniciAd);
        }

        // Yeni Şifre Kaydetme (POST)
        [HttpPost]
        public ActionResult ResetPassword(string kullaniciAd, string yeniSifre)
        {
            var kullanici = entity.Kullanici.FirstOrDefault(x => x.KKullaniciAd == kullaniciAd);
            if (kullanici != null)
            {
                kullanici.KSifre = yeniSifre;
                entity.SaveChanges();
                ViewBag.Message = "Şifreniz başarıyla güncellendi.";
                return RedirectToAction("Index");
            }

            ViewBag.Message = "Bir hata oluştu.";
            return View();
        }
    }
}