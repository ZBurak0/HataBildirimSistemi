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
            var Arizalar = (from p in entity.ArızaTur select p).ToList();
            ViewBag.Arizalar = Arizalar;

            return View();
        }
        [HttpPost]
        public ActionResult Bildirim(FormCollection formCollection, HttpPostedFileBase Dosya)
        {
            var Arizalar = (from p in entity.ArızaTur select p).ToList();
            var Durumlar = (from p in entity.Durum select p).ToList();
            ViewBag.Arizalar = Arizalar;
            ViewBag.Durum = Durumlar;//viewde combobox ayarlamada kaldı

            int BirimId = Convert.ToInt32(Session["KBirimId"]);
            string Ad = (string)Session["Ad"];
            //formdan alınanlar 
            string ArizaAdi = formCollection["ArizaAdi"];
            int ArizaTurId = Convert.ToInt32(formCollection["ArizaTuru"]);
            string Aciklama = formCollection["Aciklama"];
            DateTime? tarih = null;
            if (DateTime.TryParse(formCollection["Tarih"], out DateTime parsed))
            {
                tarih = parsed;
            }
            ArızaBildirim arzb = new ArızaBildirim();


            //dosya yolu gelecek

            // Dosya adını almak
            var dosyaAdi = Path.GetFileName(Dosya.FileName);

            // Dosyanın kaydedileceği yol
            var yol = Path.Combine(Server.MapPath("~/Uploads/"), dosyaAdi);
            var klasor = Server.MapPath("~/Uploads/");
            if (!Directory.Exists(klasor))
                Directory.CreateDirectory(klasor);
            // Dosyayı kaydetmek
            Dosya.SaveAs(yol);

            // Dosya yolunu modelde tutmak
            arzb.DosyaYolu = "/Uploads/" + dosyaAdi;

            arzb.Ad = ArizaAdi;
            arzb.KullaniciAd = Ad;
            arzb.BirimId = BirimId;
            arzb.ArizaTurId = ArizaTurId;
            arzb.Aciklama = Aciklama;
            arzb.Tarih = tarih;

            entity.ArızaBildirim.Add(arzb);
            entity.SaveChanges();
            ViewBag.basari = "Kaydetme başarılı";
            return View();
        }


        [HttpGet]
        public ActionResult Listeleme()
        {
            var KAd = Session["KAd"];
            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad");
            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad");

            var arizalarr = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Where(a => a.KullaniciAd == KAd)
                .ToList();

            return View(arizalarr);
        }

        [HttpPost]
        public ActionResult Listeleme(int? BirimId, int? ArizaTurId)
        {
            var KAd = Session["KAd"];
            var arizalarr = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
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

            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", BirimId);
            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad", ArizaTurId);

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
            return View(kullanici);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(Kullanici model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = entity.Kullanici.Find(model.Id);
                if (kullanici != null)
                {
                    kullanici.Ad = model.Ad;
                    kullanici.Soyad = model.Soyad;
                    kullanici.TelNo = model.TelNo;
                    kullanici.KSifre = model.KSifre;

                    entity.SaveChanges();
                    return RedirectToAction("Profil");
                }
            }

            return View(model);
        }
    }
    //opensource anket uygulaması 
    //gorselliği kullan 
    }
