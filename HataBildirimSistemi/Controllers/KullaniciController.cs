using HataBildirimSistemi.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;



namespace HataBildirimSistemi.Controllers
{

    public class KullaniciController : Controller
    {
        HataBildirimModelMvcEntities3 entity = new HataBildirimModelMvcEntities3();
        // GET: Kullanici
        public ActionResult Bildirim()
        {
            var Arizalar = entity.ArızaTur.ToList();
            var Binalar = entity.Bina.ToList();

            ViewBag.Arizalar = Arizalar;
            ViewBag.Binalar = new SelectList(Binalar, "Id", "Ad");

            int BirimId = Convert.ToInt32(Session["KBirimId"]);
            var birim = entity.Birim.Find(BirimId);
            ViewBag.BirimAdi = birim != null ? birim.Ad : "Birim bulunamadı";

            return View();
        }

        [HttpPost]
        public ActionResult Bildirim(FormCollection formCollection, HttpPostedFileBase Dosya)
        {
            var Arizalar = entity.ArızaTur.ToList();
            var Binalar = entity.Bina.ToList();

            ViewBag.Arizalar = Arizalar;
            ViewBag.Binalar = new SelectList(Binalar, "Id", "Ad");

            int BirimId = Convert.ToInt32(Session["KBirimId"]);
            var birim = entity.Birim.Find(BirimId);
            ViewBag.BirimAdi = birim != null ? birim.Ad : "Birim bulunamadı";

            string Ad = (string)Session["KUAd"];

            string ArizaAdi = formCollection["ArizaAdi"];
            int ArizaTurId = Convert.ToInt32(formCollection["ArizaTuru"]);
            string Aciklama = formCollection["Aciklama"];

            string binaSecim = formCollection["BinaId"];
            string digerBinaAdi = formCollection["DigerBinaAdi"];

            int binaId;

            if (binaSecim == "Diger")
            {
                if (!string.IsNullOrWhiteSpace(digerBinaAdi))
                {
                    Bina yeniBina = new Bina { Ad = digerBinaAdi };
                    entity.Bina.Add(yeniBina);
                    entity.SaveChanges();

                    binaId = yeniBina.Id;
                }
                else
                {
                    ModelState.AddModelError("DigerBinaAdi", "Lütfen bina adını giriniz.");
                    return View();
                }
            }
            else
            {
                binaId = Convert.ToInt32(binaSecim);
            }

            ArızaBildirim arzb = new ArızaBildirim();

            if (Dosya != null && Dosya.ContentLength > 0)
            {
                var dosyaAdi = Path.GetFileName(Dosya.FileName);
                var klasor = Server.MapPath("~/Uploads/");
                var yol = Path.Combine(klasor, dosyaAdi);

                if (!Directory.Exists(klasor))
                    Directory.CreateDirectory(klasor);

                Dosya.SaveAs(yol);
                arzb.DosyaYolu = "/Uploads/" + dosyaAdi;
            }
            else
            {
                arzb.DosyaYolu = null;
            }

            arzb.Ad = ArizaAdi;
            arzb.KullaniciAd = Ad;
            arzb.BirimId = BirimId;
            arzb.ArizaTurId = ArizaTurId;
            arzb.Aciklama = Aciklama;
            arzb.Tarih = DateTime.Now;
            arzb.DurumId = 5;
            arzb.BinaId = binaId;

            entity.ArızaBildirim.Add(arzb);
            entity.SaveChanges();

            ViewBag.basari = "Kaydetme başarılı";

            return View();
        }



        public ActionResult Listeleme()
        {
            var KAd = Session["KUAd"];
            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad");
            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad");
            ViewBag.Durumlar = new SelectList(entity.Durum.ToList(), "Id", "Ad");
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad"); // Bina listesi

            var arizalarr = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina) // Bina dahil edildi
                .Where(a => a.KullaniciAd == KAd)
                .ToList();

            return View(arizalarr);
        }

        [HttpPost]
        public ActionResult Listeleme(int? BirimId, int? ArizaTurId, int? DurumId, int? BinaId)  // BinaId eklendi
        {
            var KAd = Session["KUAd"];
            var arizalarr = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina) // Bina dahil edildi
                .Where(a => a.KullaniciAd == KAd)
                .AsQueryable();

            if (BirimId.HasValue)
            {
                arizalarr = arizalarr.Where(a => a.BirimId == BirimId);
            }

            if (ArizaTurId.HasValue)
            {
                arizalarr = arizalarr.Where(a => a.ArizaTurId == ArizaTurId);
            }

            if (DurumId.HasValue)
            {
                arizalarr = arizalarr.Where(a => a.DurumId == DurumId);
            }

            if (BinaId.HasValue)  // Bina filtreleme eklendi
            {
                arizalarr = arizalarr.Where(a => a.BinaId == BinaId);
            }

            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", BirimId);
            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad", ArizaTurId);
            ViewBag.Durumlar = new SelectList(entity.Durum.ToList(), "Id", "Ad", DurumId);
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad", BinaId);  // Bina dropdown

            return View(arizalarr.ToList());
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
