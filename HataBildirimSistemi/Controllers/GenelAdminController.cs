using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using ClosedXML.Excel;


namespace HataBildirimSistemi.Controllers
{
    public class GenelAdminController : Controller
    {
        HataBildirimModelMvcEntities3 entity = new HataBildirimModelMvcEntities3();
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
        public ActionResult Yetkilendirme()
        {
            var model = new AdminPanelViewModel
            {
                Kullanicilar = entity.Kullanici.ToList(),
                Adminler = entity.Admin.ToList(),
                ArizaTurleri = entity.ArızaTur.ToList()
            };
            return View(model);
        }

//namespace HataBildirimSistemi.Controllers
//{
//    public class GenelAdminController : Controller
//    {
//        HataBildirimModelMvcEntities3 entity = new HataBildirimModelMvcEntities3();
//        // GET: Admin
//        public ActionResult Index()
//        {
//            int yetkiturıd = Convert.ToInt32(Session["AYetkiId"]);
//            if (yetkiturıd == 4)
//            {
//                return View();
//            }
//            else
//            {
//                return RedirectToAction("Index", "Login");
//            }
//        }
//        public ActionResult Yetkilendirme()
//        {
//            var model = new AdminPanelViewModel
//            {
//                Kullanicilar = entity.Kullanici
//        .Where(k => k.YetkiId != 4)
//        .ToList(),
//                Adminler = entity.Admin
//        .Where(a => a.YetkiId != 4)
//        .ToList(),
//                ArizaTurleri = entity.ArızaTur.ToList()
//            };
//            return View(model);
//        }

//        // Kullanıcıyı Admin Yap
//        public ActionResult KullaniciyiAdminYap(int id)
//        {
//            var kullanici = entity.Kullanici.Find(id);
//            if (kullanici != null)
//            {
//                Admin yeniAdmin = new Admin
//                {
//                    Ad = kullanici.Ad,
//                    Soyad = kullanici.Soyad,
//                    TelNo = kullanici.TelNo,
//                    AKullaniciAd = kullanici.KKullaniciAd,
//                    ASifre = kullanici.KSifre,
//                    BirimId = kullanici.BirimId,
//                    YetkiId = 1
//                };

        // Admini Kullanıcı Yap
        public ActionResult AdminiKullaniciYap(int id)
        {
            var admin = entity.Admin.Find(id);
            if (admin != null)
            {
                Kullanici yeniKullanici = new Kullanici
                {
                    Ad = admin.Ad,
                    Soyad = admin.Soyad,
                    TelNo = admin.TelNo,
                    KKullaniciAd = admin.AKullaniciAd,
                    KSifre = admin.ASifre,
                    BirimId = (int)admin.BirimId
                };

//        // Admini Kullanıcı Yap
//        public ActionResult AdminiKullaniciYap(int id)
//        {
//            var admin = entity.Admin.Find(id);
//            if (admin != null)
//            {
//                Kullanici yeniKullanici = new Kullanici
//                {
//                    Ad = admin.Ad,
//                    Soyad = admin.Soyad,
//                    TelNo = admin.TelNo,
//                    KKullaniciAd = admin.AKullaniciAd,
//                    KSifre = admin.ASifre,
//                    BirimId = (int)admin.BirimId,
//                    YetkiId = 2
//                };

//                entity.Kullanici.Add(yeniKullanici);
//                entity.Admin.Remove(admin);
//                entity.SaveChanges();
//            }
//            return RedirectToAction("Yetkilendirme");
//        }

//        // Kullanıcı Sil
//        public ActionResult KullaniciSil(int id)
//        {
//            var kullanici = entity.Kullanici.Find(id);
//            if (kullanici != null)
//            {
//                entity.Kullanici.Remove(kullanici);
//                entity.SaveChanges();
//            }
//            return RedirectToAction("Yetkilendirme");
//        }

//        // şimdilik sisteme dahil edilmedi 
//        [HttpGet]
//        public ActionResult YetkiliServisEkle()
//        {
//            var model = new AdminPanelViewModel
//            {
//                ArizaTurleri = entity.ArızaTur.ToList() // ArizaTurleri listesi dolu geliyor
//            };

//            return View(model);
//        }

//        // Yetkili Servis Ekle
//        [HttpPost]
//        public ActionResult YetkiliServisEkle(string Ad, string Soyad, string TelNo, string Mail, string YKullaniciAd, string YSifre, int ArizaTurId)
//        {
//            YetkiliServis servis = new YetkiliServis
//            {
//                Ad = Ad,
//                Soyad = Soyad,
//                TelNo = TelNo,
//                Mail = Mail,
//                YKullaniciAd = YKullaniciAd,
//                YSifre = YSifre,
//                YetkiId = 3,
//                ArizaTurId = ArizaTurId
//            };
//            entity.YetkiliServis.Add(servis);
//            entity.SaveChanges();

//            return RedirectToAction("YetkiliServisEkle");
//        }

//        [HttpGet]
//        public JsonResult ArizaAra(string kelime)//search kutusu icin eklenmistir 
//        {
//            var arizalar = entity.ArızaBildirim
//                .Include(a => a.Birim)
//                .Include(a => a.ArızaTur)
//                .Include(a => a.Durum)
//                .Where(a => a.Aciklama.Contains(kelime))
//                .Select(a => new
//                {
//                    a.Id,
//                    a.Ad,
//                    ArizaTur = a.ArızaTur.Ad,
//                    Birim = a.Birim.Ad,
//                    a.Aciklama,
//                    a.DosyaYolu,
//                    Tarih = a.Tarih.ToString(),
//                    Durum = a.Durum != null ? a.Durum.Ad : "Tanımsız"
//                })
//                .ToList();

//            return Json(arizalar, JsonRequestBehavior.AllowGet);
//        }

//        [HttpGet]
//        public ActionResult ArızaGoruntuleme()
//        {
//            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad");
//            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad");
//            ViewBag.ArananKelime = "";

//            var arizalarr = entity.ArızaBildirim
//                .Include(a => a.Birim)
//                .Include(a => a.ArızaTur)
//                .Include(a => a.Durum)
//                .ToList();

//            return View(arizalarr);
//        }

//        [HttpPost]
//        public ActionResult ArızaGoruntuleme(int? BirimId, int? ArizaTurId, string ArananKelime)
//        {
//            var arizalarr = entity.ArızaBildirim
//                .Include(a => a.Birim)
//                .Include(a => a.ArızaTur)
//                .Include(a => a.Durum)
//                .AsQueryable();

//            if (BirimId.HasValue)
//            {
//                arizalarr = arizalarr.Where(a => a.BirimId == BirimId);
//            }

//            if (ArizaTurId.HasValue)
//            {
//                arizalarr = arizalarr.Where(a => a.ArizaTurId == ArizaTurId);
//            }

//            if (!string.IsNullOrEmpty(ArananKelime))
//            {
//                arizalarr = arizalarr.Where(a => a.Aciklama.Contains(ArananKelime));
//            }

//            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad", BirimId);
//            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad", ArizaTurId);
//            ViewBag.ArananKelime = ArananKelime;

//            return View(arizalarr.ToList());
//        }

//        // Arıza Türü Ekleme / Çıkarma
//        [HttpGet]
//        public ActionResult ArizaTurEkle()
//        {
//            var model = new AdminPanelViewModel
//            {
//                ArizaTurleri = entity.ArızaTur.ToList() // Arıza türlerini listele
//            };

//            return View(model);
//        }

//        // Arıza Türü Ekleme
//        [HttpPost]
//        public ActionResult ArizaTurEkle(string TurAd)
//        {
//            if (!string.IsNullOrEmpty(TurAd))
//            {
//                ArızaTur arizaTur = new ArızaTur
//                {
//                    Ad = TurAd
//                };
//                entity.ArızaTur.Add(arizaTur);
//                entity.SaveChanges();
//            }

//            return RedirectToAction("ArizaTurEkle");
//        }

//        // Arıza Türü Silme
//        [HttpPost]
//        public ActionResult ArizaTurSil(int Id)
//        {
//            var arizaTur = entity.ArızaTur.FirstOrDefault(x => x.Id == Id);
//            if (arizaTur != null)
//            {
//                entity.ArızaTur.Remove(arizaTur);
//                entity.SaveChanges();
//            }

//            return RedirectToAction("ArizaTurEkle");
//        }

//        // Birim Ekleme / Çıkarma
//        [HttpGet]
//        public ActionResult BirimEkle()
//        {
//            var model = new AdminPanelViewModel
//            {
//                Birimler = entity.Birim
//                .Where(b => b.Id != 4)
//                .ToList() // Birimleri listele
//            };

//            return View(model);
//        }

//        // Birim Ekleme
//        [HttpPost]
//        public ActionResult BirimEkle(string BirimAd)
//        {
//            if (!string.IsNullOrEmpty(BirimAd))
//            {
//                Birim birim = new Birim
//                {
//                    Ad = BirimAd
//                };
//                entity.Birim.Add(birim);
//                entity.SaveChanges();
//            }

//            return RedirectToAction("BirimEkle");
//        }

//        // Birim Silme
//        [HttpPost]
//        public ActionResult BirimSil(int Id)
//        {
//            var birim = entity.Birim.FirstOrDefault(x => x.Id == Id);
//            if (birim != null)
//            {
//                entity.Birim.Remove(birim);
//                entity.SaveChanges();
//            }

//            return RedirectToAction("BirimEkle");
//        }


//        public ActionResult AProfil()
//        {
//            int? AdminId = Session["AId"] as int?;
//            if (AdminId == null)
//            {
//                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa login sayfasına yönlendir
//            }
//            var Admin = entity.Admin.FirstOrDefault(a => a.Id == AdminId);
//            if (Admin == null)
//            {
//                return HttpNotFound();
//            }
//            return View(Admin);
//        }
//        [HttpGet]
//        public ActionResult Duzenle(int id)
//        {
//            var Admin = entity.Admin.Find(id);
//            if (Admin == null)
//            {
//                return HttpNotFound();
//            }
//            return View(Admin);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public ActionResult Duzenle(Admin model)
//        {
//            if (ModelState.IsValid)
//            {
//                if (model.ASifre.Length < 6 || model.ASifre.Length > 20)
//                {
//                    ModelState.AddModelError("ASifre", "Şifre 6 ile 20 karakter arasında olmalıdır.");
//                    return View(model);
//                }
//                // Kullanıcı adı kontrolü
//                if (!model.AKullaniciAd.ToLower().EndsWith("@akdeniz.edu.tr"))
//                {
//                    ModelState.AddModelError("KullaniciAdi", "Kullanıcı adı @akdeniz.edu.tr ile bitmelidir.");
//                    return View(model);
//                }

//                var Admin = entity.Admin.Find(model.Id);
//                if (Admin != null)
//                {
//                    Admin.Ad = model.Ad;
//                    Admin.Soyad = model.Soyad;
//                    Admin.TelNo = model.TelNo;
//                    Admin.ASifre = model.ASifre;
//                    Admin.AKullaniciAd = model.AKullaniciAd;

//                    entity.SaveChanges();
//                    return RedirectToAction("AProfil");
//                }
//            }

//            return View(model);
//        }
//        // Arıza Bildirim Raporu (Excel İndirme)
//        public ActionResult ArizaRaporuIndir()
//        {
//            var arizalar = entity.ArızaBildirim
//                                 .Select(x => new
//                                 {
//                                     ArizaAdi = x.Ad,
//                                     Tarih = x.Tarih,
//                                     BirimAdi = x.Birim.Ad,
//                                     DurumAdi = x.Durum.Ad,
//                                     ArizaTuru = x.ArızaTur.Ad,
//                                     Aciklama = x.Aciklama,
//                                     kullanici = x.KullaniciAd
//                                 })
//                                 .ToList();

//            using (var workbook = new XLWorkbook())
//            {
//                var worksheet = workbook.Worksheets.Add("Ariza Raporu");

//                // Başlıklar
//                worksheet.Cell(1, 1).Value = "Arıza Adı";
//                worksheet.Cell(1, 2).Value = "Arıza Türü";
//                worksheet.Cell(1, 3).Value = "Birim";
//                worksheet.Cell(1, 4).Value = "Açıklama";
//                worksheet.Cell(1, 5).Value = "Tarih";
//                worksheet.Cell(1, 6).Value = "Durum";
//                worksheet.Cell(1, 7).Value = "Kullanıcı";

//                int row = 2;
//                foreach (var item in arizalar)
//                {
//                    worksheet.Cell(row, 1).Value = item.ArizaAdi;
//                    worksheet.Cell(row, 2).Value = item.ArizaTuru;
//                    worksheet.Cell(row, 3).Value = item.BirimAdi;
//                    worksheet.Cell(row, 4).Value = item.Aciklama;                 
//                    worksheet.Cell(row, 5).Value = item.Tarih.HasValue ? item.Tarih.Value.ToShortDateString() : "";
//                    worksheet.Cell(row, 6).Value = item.DurumAdi;
//                    worksheet.Cell(row, 7).Value = item.kullanici;
//                    row++;
//                }

//                using (var stream = new MemoryStream())
//                {
//                    workbook.SaveAs(stream);
//                    stream.Position = 0;
//                    return File(stream.ToArray(),
//                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
//                                "ArizaRaporu.xlsx");
//                }
//            }

//        }
//        public ActionResult LogOut()
//        {
//            return RedirectToAction("Index", "Login");
//        }
//        [HttpGet]
//        public ActionResult KullaniciListesi(string rol = "Tümü")
//        {
//            var birimler = entity.Birim.Where(b => b.Id != 4).ToList();

//            var model = new KullaniciAdminViewModel();
//            model.Birimler = birimler;
//            model.SeciliRol = rol;

//            if (rol == "Kullanıcı")
//            {
//                model.Kullanicilar = entity.Kullanici
//                    .Include(k => k.Birim)
//                    .Where(k => k.BirimId != 4 && k.YetkiId == 2)
//                    .ToList();
//                model.Adminler = new List<Admin>();
//            }
//            else if (rol == "Admin")
//            {
//                model.Adminler = entity.Admin
//                    .Include(a => a.Birim)
//                    .Where(a => a.BirimId != 4 && a.YetkiId == 1)
//                    .ToList();
//                model.Kullanicilar = new List<Kullanici>();
//            }
//            else // Tümü
//            {
//                model.Kullanicilar = entity.Kullanici
//                    .Include(k => k.Birim)
//                    .Where(k => k.BirimId != 4)
//                    .ToList();

//                model.Adminler = entity.Admin
//                    .Include(a => a.Birim)
//                    .Where(a => a.BirimId != 4)
//                    .ToList();
//            }

        public ActionResult AProfil()
        {
            int? AdminId = Session["AId"] as int?;
            if (AdminId == null)
            {
                return RedirectToAction("Index", "Login"); // Giriş yapmamışsa login sayfasına yönlendir
            }
            var Admin = entity.Admin.FirstOrDefault(a => a.Id == AdminId);
            if (Admin == null)
            {
                return HttpNotFound();
            }
            return View(Admin);
        }
        [HttpGet]
        public ActionResult Duzenle(int id)
        {
            var Admin = entity.Admin.Find(id);
            if (Admin == null)
            {
                return HttpNotFound();
            }
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

//            if (rol == "Kullanıcı")
//            {
//                var kullanici = entity.Kullanici.FirstOrDefault(k => k.Id == kullaniciId);
//                if (kullanici != null)
//                {
//                    kullanici.BirimId = yeniBirimId;
//                    entity.SaveChanges();
//                    TempData["SuccessMessage"] = "Kullanıcının birimi güncellendi.";
//                }
//                else
//                {
//                    TempData["ErrorMessage"] = "Kullanıcı bulunamadı.";
//                }
//            }
//            else if (rol == "Admin")
//            {
//                var admin = entity.Admin.FirstOrDefault(a => a.Id == kullaniciId);
//                if (admin != null)
//                {
//                    admin.BirimId = yeniBirimId;
//                    entity.SaveChanges();
//                    TempData["SuccessMessage"] = "Adminin birimi güncellendi.";
//                }
//                else
//                {
//                    TempData["ErrorMessage"] = "Admin bulunamadı.";
//                }
//            }
//            else
//            {
//                TempData["ErrorMessage"] = "Geçersiz rol seçildi.";
//            }

            return View(model);
        }
        // Arıza Bildirim Raporu (Excel İndirme)
        public ActionResult ArizaRaporuIndir()
        {
            var arizalar = entity.ArızaBildirim
                                 .Select(x => new
                                 {
                                     ArizaAdi = x.Ad,
                                     Tarih = x.Tarih,
                                     BirimAdi = x.Birim.Ad,
                                     DurumAdi = x.Durum.Ad,
                                 })
                                 .ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Ariza Raporu");

                // Başlıklar
                worksheet.Cell(1, 1).Value = "Arıza Adı";
                worksheet.Cell(1, 2).Value = "Tarih";
                worksheet.Cell(1, 3).Value = "Birim";
                worksheet.Cell(1, 4).Value = "Durum";

                int row = 2;
                foreach (var item in arizalar)
                {
                    worksheet.Cell(row, 1).Value = item.ArizaAdi;
                    worksheet.Cell(row, 2).Value = item.Tarih.HasValue ? item.Tarih.Value.ToShortDateString() : "";
                    worksheet.Cell(row, 3).Value = item.BirimAdi;
                    worksheet.Cell(row, 4).Value = item.DurumAdi;
                    row++;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Position = 0;
                    return File(stream.ToArray(),
                                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                                "ArizaRaporu.xlsx");
                }
            }

        }
        public ActionResult LogOut()
        {
            return RedirectToAction("Index", "Login");
        }
    }
}