using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HataBildirimSistemi.Controllers
{
    public class YetkiliServisController : Controller
    {
        HataBildirimModelMvcEntities3 entity = new HataBildirimModelMvcEntities3();
        // GET: Yetkiliservis
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ArizaGoruntule()
        {
            var ArizaTurrID = Session["ArızaTurYet"] as int?;
            var kullaniciiID = Session["KId"] as int?;
            if (ArizaTurrID == null)
                return RedirectToAction("Index", "Login");

            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Include(a => a.AltArizaTur)
                .Where(a => a.ArizaTurId == ArizaTurrID)
                .Where(a => a.KullaniciId == kullaniciiID)
                .Where(a => a.DurumId != 3);

            var arizaListesi = arizalar.ToList();

            foreach (var ariza in arizaListesi)
            {
                ariza.AltArizaTurleri = entity.AltArizaTur
                    .Where(s => s.ArizaTurId == ariza.ArizaTurId)
                    .ToList();
            }

            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad");
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad");
            ViewBag.Durumlar = entity.Durum.Where(a => a.Id > 2 ).ToList();

            ViewBag.AltArizalar = entity.AltArizaTur.ToList();

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.SelectedArizaTurId = null;
            ViewBag.SelectedBinaId = null;
            ViewBag.SearchText = "";
            ViewBag.SelectedOncelik = null;

            return View(arizaListesi);
        }



        // POST: BirimAdmin/ArizaGoruntule
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArizaGoruntule(int? ArizaTurId, int? BinaId, string SearchText, bool? Oncelik)
        {
            var ArizaTurrID = Session["ArızaTurYet"] as int?;
            var kullaniciiID = Session["KId"] as int?;
            if (ArizaTurrID == null)
                return RedirectToAction("Index", "Login");

            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Include(a => a.AltArizaTur)
                .Where(a => a.ArizaTurId == ArizaTurrID)
                .Where(a => a.KullaniciId == kullaniciiID)
                .Where(a => a.DurumId != 3);


            if (ArizaTurId.HasValue && ArizaTurId.Value != 0)
                arizalar = arizalar.Where(a => a.ArizaTurId == ArizaTurId);

            if (BinaId.HasValue && BinaId.Value != 0)
                arizalar = arizalar.Where(a => a.BinaId == BinaId);

            if (Oncelik.HasValue)
                arizalar = arizalar.Where(a => a.Oncelik == Oncelik.Value);

            if (!string.IsNullOrWhiteSpace(SearchText))
                arizalar = arizalar.Where(a => a.Aciklama != null && a.Aciklama.Contains(SearchText));

            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad", ArizaTurId);
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad", BinaId);
            ViewBag.Durumlar = entity.Durum.Where(a => a.Id > 2).ToList();
            ViewBag.AltArizalar = entity.AltArizaTur.ToList();

            ViewBag.SelectedArizaTurId = ArizaTurId;
            ViewBag.SelectedBinaId = BinaId;
            ViewBag.SearchText = SearchText;
            ViewBag.SelectedOncelik = Oncelik;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(arizalar.ToList());
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

        public ActionResult TamamlananArizaGoruntule()
        {
            var ArizaTurrID = Session["ArızaTurYet"] as int?;
            var kullaniciiID = Session["KId"] as int?;
            if (ArizaTurrID == null)
                return RedirectToAction("Index", "Login");

            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Include(a => a.AltArizaTur)
                .Where(a => a.ArizaTurId == ArizaTurrID)
                .Where(a => a.KullaniciId == kullaniciiID)
                .Where(a => a.DurumId == 3)
                .ToList();

            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad");
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.SearchText = "";
            ViewBag.SelectedOncelik = null;

            return View(arizalar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TamamlananArizaGoruntule(int? ArizaTurId, int? BinaId, string SearchText, bool? Oncelik)
        {
            var ArizaTurrID = Session["ArızaTurYet"] as int?;
            var kullaniciiID = Session["KId"] as int?;
            if (ArizaTurrID == null)
                return RedirectToAction("Index", "Login");

            // Önce sadece DurumId = 3 olanları filtrele
            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Include(a => a.AltArizaTur)
                .Where(a => a.ArizaTurId == ArizaTurrID && a.KullaniciId == kullaniciiID && a.DurumId == 3);

            // Diğer filtreleri sırayla uygula
            if (ArizaTurId.HasValue && ArizaTurId.Value != 0)
                arizalar = arizalar.Where(a => a.ArizaTurId == ArizaTurId);

            if (BinaId.HasValue && BinaId.Value != 0)
                arizalar = arizalar.Where(a => a.BinaId == BinaId);

            if (Oncelik.HasValue)
                arizalar = arizalar.Where(a => a.Oncelik == Oncelik.Value);

            if (!string.IsNullOrWhiteSpace(SearchText))
                arizalar = arizalar.Where(a => a.Aciklama != null && a.Aciklama.Contains(SearchText));

            // ViewBag değerleri
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad", BinaId);
            ViewBag.SearchText = SearchText;
            ViewBag.SelectedOncelik = Oncelik;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(arizalar.ToList()); // ToList en sonda olmalı
        }





        public ActionResult Profil()
        {
            int? kullaniciId = Session["KId"] as int?;
            if (kullaniciId == null)
            {
                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa login sayfasına yönlendir
            }
            var kullanici = entity.Kullanici.FirstOrDefault(a => a.Id == kullaniciId);
            if (kullanici == null)
            {
                return HttpNotFound();
            }
            return View(kullanici);
        }
        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var kullanici = entity.Kullanici.Find(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }

            // Birim listesini ViewBag ile View'a gönder
            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", kullanici.BirimId);

            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(Kullanici model)
        {
            if (ModelState.IsValid)
            {
                if (model.KSifre.Length < 6 || model.KSifre.Length > 20)
                {
                    ModelState.AddModelError("KSifre", "Şifre 6 ile 20 karakter arasında olmalıdır.");
                    return View(model);
                }
                // Kullanıcı adı kontrolü
                if (!model.KKullaniciAd.ToLower().EndsWith("@akdeniz.edu.tr"))
                {
                    ModelState.AddModelError("KKullaniciAdi", "Kullanıcı adı @akdeniz.edu.tr ile bitmelidir.");
                    return View(model);
                }

                var kullanici = entity.Kullanici.Find(model.Id);
                if (kullanici != null)
                {
                    kullanici.Ad = model.Ad;
                    kullanici.Soyad = model.Soyad;
                    kullanici.TelNo = model.TelNo;
                    kullanici.KKullaniciAd = model.KKullaniciAd;
                    kullanici.KSifre = model.KSifre;

                    entity.SaveChanges();
                    return RedirectToAction("Profil");
                }

                return HttpNotFound();
            }
            return View(model);
        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}