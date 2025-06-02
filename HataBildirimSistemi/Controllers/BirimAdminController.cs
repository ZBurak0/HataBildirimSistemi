 using HataBildirimSistemi.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        private void SetViewBags(int? arizaTurId = null, int? binaId = null, bool? oncelik = null, string searchText = "")
        {
            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad", arizaTurId);
            ViewBag.Binalar = new SelectList(entity.Bina.ToList(), "Id", "Ad", binaId);
            ViewBag.Durumlar = entity.Durum.Where(a => a.Id != 3).ToList();
            ViewBag.AltArizalar = entity.AltArizaTur.ToList();


            ViewBag.SelectedArizaTurId = arizaTurId;
            ViewBag.SelectedBinaId = binaId;
            ViewBag.SelectedOncelik = oncelik;
            ViewBag.SearchText = searchText;
        }

        public ActionResult ArizaGoruntule()
        {
            var birimId = Session["KBirimId"] as int?;
            if (birimId == null)
                return RedirectToAction("Index", "Login");

            SetViewBags();

            var ArizaTurID = Session["ArızaTurYet"] as int?;

            var arizalar = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Where(a => a.KullaniciId == null && a.ArızaTur.Id == ArizaTurID)
                .ToList();

            foreach (var ariza in arizalar)
            {
                ariza.AltArizaTurleri = entity.AltArizaTur.Where(s => s.ArizaTurId == ariza.ArizaTurId).ToList();
                ariza.Kullanicilar = entity.Kullanici
                    .Where(a => a.ArizaTurId == ariza.ArizaTurId && a.YetkiId == 3).ToList();
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(arizalar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArizaGoruntule(int? ArizaTurId, int? BinaId, string SearchText, bool? Oncelik)
        {
            var birimId = Session["KBirimId"] as int?;
            if (birimId == null)
                return RedirectToAction("Index", "Login");

            var ArizaTurID = Session["ArızaTurYet"] as int?;

            var arizalarQuery = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Include(a => a.AltArizaTur)
                .Where(a => a.ArızaTur.Id == ArizaTurID && a.KullaniciId == null);

            if (ArizaTurId.HasValue && ArizaTurId.Value != 0)
                arizalarQuery = arizalarQuery.Where(a => a.ArizaTurId == ArizaTurId);

            if (BinaId.HasValue && BinaId.Value != 0)
                arizalarQuery = arizalarQuery.Where(a => a.BinaId == BinaId);

            if (Oncelik.HasValue)
                arizalarQuery = arizalarQuery.Where(a => a.Oncelik == Oncelik.Value);

            if (!string.IsNullOrWhiteSpace(SearchText))
                arizalarQuery = arizalarQuery.Where(a => a.Aciklama != null && a.Aciklama.Contains(SearchText));

            var arizaList = arizalarQuery.ToList();

            foreach (var ariza in arizaList) 
            {
                ariza.AltArizaTurleri = entity.AltArizaTur
                    .Where(s => s.ArizaTurId == ariza.ArizaTurId).ToList();

                ariza.Kullanicilar = entity.Kullanici
                    .Where(a => a.ArizaTurId == ariza.ArizaTurId && a.YetkiId == 3).ToList();
            }

            SetViewBags(ArizaTurId, BinaId, Oncelik, SearchText);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(arizaList);
        }
        public ActionResult AtanmisArizaGoruntule()
        {
            var birimId = Session["KBirimId"] as int?;
            if (birimId == null)
                return RedirectToAction("Index", "Login");

            SetViewBags();

            var ArizaTurID = Session["ArızaTurYet"] as int?;

            var arizaList = entity.ArızaBildirim
                .Include(a => a.Birim)
                //.Include(a => a.AltArizaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Where(a => a.KullaniciId != null && a.ArızaTur.Id == ArizaTurID)
                .ToList();

            foreach (var ariza in arizaList)
            {
                ariza.AltArizaTurleri = entity.AltArizaTur
                    .Where(s => s.ArizaTurId == ariza.ArizaTurId).ToList();

                ariza.Kullanicilar = entity.Kullanici
                    .Where(a => a.ArizaTurId == ariza.ArizaTurId && a.YetkiId == 3).ToList();
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(arizaList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AtanmisArizaGoruntule(int? ArizaTurId, int? BinaId, string SearchText, bool? Oncelik)
        {
            var birimId = Session["KBirimId"] as int?;
            if (birimId == null)
                return RedirectToAction("Index", "Login");

            var ArizaTurID = Session["ArızaTurYet"] as int?;

            var arizalarQuery = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .Include(a => a.Bina)
                .Include(a => a.AltArizaTur)
                .Where(a => a.ArızaTur.Id == ArizaTurID && a.KullaniciId != null);

            if (ArizaTurId.HasValue && ArizaTurId.Value != 0)
                arizalarQuery = arizalarQuery.Where(a => a.ArizaTurId == ArizaTurId);

            if (BinaId.HasValue && BinaId.Value != 0)
                arizalarQuery = arizalarQuery.Where(a => a.BinaId == BinaId);

            if (Oncelik.HasValue)
                arizalarQuery = arizalarQuery.Where(a => a.Oncelik == Oncelik.Value);

            if (!string.IsNullOrWhiteSpace(SearchText))
                arizalarQuery = arizalarQuery.Where(a => a.Aciklama != null && a.Aciklama.Contains(SearchText));

            var arizaList = arizalarQuery.ToList();

            foreach (var ariza in arizaList)
            {
                ariza.AltArizaTurleri = entity.AltArizaTur
                    .Where(s => s.ArizaTurId == ariza.ArizaTurId).ToList();

                ariza.Kullanicilar = entity.Kullanici
                    .Where(a => a.ArizaTurId == ariza.ArizaTurId && a.YetkiId == 3).ToList();
            }

            SetViewBags(ArizaTurId, BinaId, Oncelik, SearchText);
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(arizaList);
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
        public ActionResult AtanmisOncelikDegistir(int arizaId, bool? yeniOncelik)
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
            return RedirectToAction("AtanmisArizaGoruntule");
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AltArizaBelirle(int arizaId, int AltArizaId)
        {
            var ariza = entity.ArızaBildirim.Find(arizaId);
            if (ariza != null)
            {
                ariza.AltArizaTurId = AltArizaId;
                entity.SaveChanges();
                TempData["SuccessMessage"] = "Durum başarıyla güncellendi.";
            }

            return RedirectToAction("ArizaGoruntule");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AtanmisAltArizaBelirle(int arizaId, int AltArizaId)
        {
            var ariza = entity.ArızaBildirim.Find(arizaId);
            if (ariza != null)
            {
                ariza.AltArizaTurId = AltArizaId;
                entity.SaveChanges();
                TempData["SuccessMessage"] = "Durum başarıyla güncellendi.";
            }

            return RedirectToAction("AtanmisArizaGoruntule");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsAta(int arizaId, int KullaniciId)
        {
            var ariza = entity.ArızaBildirim.Find(arizaId);
            if (ariza != null)
            {
                ariza.KullaniciId = KullaniciId;
                entity.SaveChanges();
                TempData["SuccessMessage"] = "Durum başarıyla güncellendi.";
            }

            return RedirectToAction("ArizaGoruntule");
        }


        public ActionResult Raporla(int arizaId)
        {
            var ariza = entity.ArızaBildirim
                              .Include(a => a.Birim)
                              .Include(a => a.Bina)
                              .Include(a => a.AltArizaTur.ArızaTur)
                              .FirstOrDefault(a => a.Id == arizaId);

            if (ariza == null)
                return HttpNotFound();

            // using bloğu kullanmıyoruz burada
            MemoryStream memoryStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 50, 50, 80, 50);
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
            document.Open();

            // Başlık
            Font titleFont = FontFactory.GetFont("Arial", 20, Font.BOLD, BaseColor.BLACK);
            Paragraph title = new Paragraph("Arıza Raporu", titleFont)
            {
                Alignment = Element.ALIGN_CENTER,
                SpacingAfter = 20f
            };
            document.Add(title);

            // Tarih
            Font dateFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Paragraph tarih = new Paragraph("Rapor Tarihi: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm"), dateFont)
            {
                Alignment = Element.ALIGN_RIGHT,
                SpacingAfter = 20f
            };
            document.Add(tarih);

            // Tablo
            Font headerFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            Font bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);
            PdfPTable table = new PdfPTable(2)
            {
                WidthPercentage = 100,
                SpacingBefore = 10f
            };

            void AddRow(string label, string value)
            {
                PdfPCell cell1 = new PdfPCell(new Phrase(label, headerFont))
                {
                    BackgroundColor = new BaseColor(230, 230, 250),
                    Padding = 5
                };
                table.AddCell(cell1);

                PdfPCell cell2 = new PdfPCell(new Phrase(value, bodyFont))
                {
                    Padding = 5
                };
                table.AddCell(cell2);
            }

            AddRow("Arıza ID", ariza.Id.ToString());
            AddRow("Arıza Adı", ariza.Ad);
            AddRow("Arıza Türü", ariza.AltArizaTur?.ArızaTur?.Ad ?? "—");
            AddRow("Alt Türü", ariza.AltArizaTur?.Ad ?? "—");
            AddRow("Bildirilen Birim", ariza.Birim?.Ad ?? "—");
            //AddRow("Alt Birim", ariza.Kullanici.AltBirim?.Ad ?? "—");//arizabildirim tablosuna altbirimıd baglanılacak 
            AddRow("Bina", ariza.Bina?.Ad ?? "—");
            AddRow("Açıklama", ariza.Aciklama ?? "—");
            AddRow("Tarih", ariza.Tarih.ToString("dd.MM.yyyy HH:mm"));
            AddRow("Durum", ariza.Durum?.Ad ?? "—");
            AddRow("Öncelik", ariza.Oncelik == true ? "Acil" : "Normal");
            AddRow("Atanan Kişi", ariza.Kullanici?.Ad ?? "—");

            document.Add(table);
            document.Close(); // belgenin yazımı bitti
            writer.Close();   // writer'ı da kapat

            byte[] bytes = memoryStream.ToArray(); // içeriği al
            return File(new MemoryStream(bytes), "application/pdf", $"ArizaRaporu_{ariza.Id}.pdf");
        }

        public ActionResult BAProfil()
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

            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", kullanici.BirimId);
            ViewBag.AltBirimler = new SelectList(entity.AltBirim.Where(ab => ab.BirimId == kullanici.BirimId).ToList(), "Id", "Ad", kullanici.AltBirimId);

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
                    kullanici.BirimId = model.BirimId;
                    kullanici.AltBirimId = model.AltBirimId;

                    entity.SaveChanges();
                    return RedirectToAction("BAProfil");
                }

                return HttpNotFound();
            }
            return View(model);
        }

        public JsonResult AltBirimleriGetir(int birimId)
        {
            var altBirimler = entity.AltBirim
                .Where(ab => ab.BirimId == birimId)
                .Select(ab => new { ab.Id, ab.Ad })
                .ToList();

            return Json(altBirimler, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut() 
        {
            return RedirectToAction("Index", "Login");
        }
    }
    
}