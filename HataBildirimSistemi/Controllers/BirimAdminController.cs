 using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HataBildirimSistemi.Controllers
{
    public class BirimAdminController : Controller
    {
        HataBildirimModelMvcEntities3 entity = new HataBildirimModelMvcEntities3();
        // GET: BirimAdmin
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ArizaGoruntule()
        {
            var birimId = Session["ABirimId"] as int?;
            if (birimId == null)
                return RedirectToAction("Index", "Login");

            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad");
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad");
            ViewBag.Durumlar = entity.Durum.ToList();

            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Where(a => a.BirimId == birimId)
                .ToList();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.SelectedArizaTurId = null;
            ViewBag.SelectedBinaId = null;
            ViewBag.SearchText = "";

            // Varsayılan olarak öncelik filtresi eklenmedi
            ViewBag.SelectedOncelik = null;

            return View(arizalar);
        }

        // POST: BirimAdmin/ArizaGoruntule
        [HttpPost]
        public ActionResult ArizaGoruntule(int? ArizaTurId)
        {
            var birimId = Session["ABirimId"] as int?;
            if (birimId == null)
                return RedirectToAction("Index", "Login");

            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Where(a => a.BirimId == birimId);

            if (ArizaTurId.HasValue)
                arizalar = arizalar.Where(a => a.ArizaTurId == ArizaTurId);

            if (BinaId.HasValue && BinaId.Value != 0)
                arizalar = arizalar.Where(a => a.BinaId == BinaId);

            if (Oncelik.HasValue)
                arizalar = arizalar.Where(a => a.Oncelik == Oncelik.Value);

            if (!string.IsNullOrWhiteSpace(SearchText))
                arizalar = arizalar.Where(a => a.Aciklama != null && a.Aciklama.Contains(SearchText));

            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad", ArizaTurId);
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad", BinaId);
            ViewBag.Durumlar = entity.Durum.ToList();

            ViewBag.SelectedArizaTurId = ArizaTurId;
            ViewBag.SelectedBinaId = BinaId;
            ViewBag.SearchText = SearchText;
            ViewBag.SelectedOncelik = Oncelik;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(arizalar.ToList());
        }

        // POST: BirimAdmin/OncelikDegistir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OncelikDegistir(int arizaId, bool? yeniOncelik)
        {
            var ariza = entity.ArızaBildirim.Find(arizaId);
            if (ariza != null)
            {
                ariza.Oncelik = yeniOncelik;
                entity.SaveChanges();
                TempData["SuccessMessage"] = "Öncelik başarıyla güncellendi.";
            }
            else
            {
                TempData["SuccessMessage"] = "Arıza bulunamadı.";
            }
            return RedirectToAction("ArizaGoruntule");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DurumDegistir(int arizaId, int durumId)
        {
            var ariza = entity.ArızaBildirim.Find(arizaId);
            if (ariza != null)
            {
                ariza.DurumId = durumId;
                entity.SaveChanges();
                TempData["SuccessMessage"] = "Durum başarıyla güncellendi.";
            }

            return RedirectToAction("ArizaGoruntule");
        }

        public ActionResult BAProfil()
        {
            int? AdminId = Session["AId"] as int?;
            if (AdminId == null)
                return RedirectToAction("Index", "Login");

            var Admin = entity.Admin.FirstOrDefault(a => a.Id == AdminId);
            if (Admin == null)
                return HttpNotFound();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(Admin);
        }

        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var Admin = entity.Admin.Find(id);
            if (Admin == null)
                return HttpNotFound();
            }

            // Birim listesini ViewBag ile View'a gönder
            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", kullanici.BirimId);

            return View(Admin);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(Admin model)
        {
            if (ModelState.IsValid)
            {
                if (model.ASifre.Length < 6 || model.ASifre.Length > 20)
                {
                    ModelState.AddModelError("ASifre", "Şifre 6 ile 20 karakter arasında olmalıdır.");
                    return View(model);
                }
                // Kullanıcı adı kontrolü
                if (!model.AKullaniciAd.ToLower().EndsWith("@akdeniz.edu.tr"))
                {
                    ModelState.AddModelError("KullaniciAdi", "Kullanıcı adı @akdeniz.edu.tr ile bitmelidir.");
                    return View(model);
                }

                var Admin = entity.Admin.Find(model.Id);
                if (Admin != null)
                    {
                    Admin.Ad = model.Ad;
                    Admin.Soyad = model.Soyad;
                    Admin.TelNo = model.TelNo;
                    Admin.ASifre = model.ASifre;
                    Admin.AKullaniciAd = model.AKullaniciAd;

                        entity.SaveChanges();
                    TempData["SuccessMessage"] = "Profil başarıyla güncellendi.";
                    return RedirectToAction("BAProfil");
                }
            }

            // Hata varsa birim listesini tekrar yükle
            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", model.BirimId);

            return View(model);
        }
        public ActionResult LogOut() 
        {
            return RedirectToAction("Index", "Login");
        }
    }
    
}