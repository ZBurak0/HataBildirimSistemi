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

        // Kullanıcıyı Admin Yap
        public ActionResult KullaniciyiAdminYap(int id)
        {
            var kullanici = entity.Kullanici.Find(id);
            if (kullanici != null)
            {
                Admin yeniAdmin = new Admin
                {
                    Ad = kullanici.Ad,
                    Soyad = kullanici.Soyad,
                    TelNo = kullanici.TelNo,
                    AKullaniciAd = kullanici.KKullaniciAd,
                    ASifre = kullanici.KSifre,
                    BirimId = kullanici.BirimId,
                    YetkiId = 1
                };

                entity.Admin.Add(yeniAdmin);
                entity.Kullanici.Remove(kullanici);
                entity.SaveChanges();
            }
            return RedirectToAction("Yetkilendirme");
        }

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

                entity.Kullanici.Add(yeniKullanici);
                entity.Admin.Remove(admin);
                entity.SaveChanges();
            }
            return RedirectToAction("Yetkilendirme");
        }

        // Kullanıcı Sil
        public ActionResult KullaniciSil(int id)
        {
            var kullanici = entity.Kullanici.Find(id);
            if (kullanici != null)
            {
                entity.Kullanici.Remove(kullanici);
                entity.SaveChanges();
            }
            return RedirectToAction("Yetkilendirme");
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
        [HttpGet]
        public ActionResult ArızaGoruntuleme()
        {
            ViewBag.Birimler = new SelectList(entity.Birim.ToList(), "Id", "Ad");
            ViewBag.Arizalar = new SelectList(entity.ArızaTur.ToList(), "Id", "Ad");

            var arizalarr = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
                .ToList();

            return View(arizalarr);
        }

        [HttpPost]
        public ActionResult ArızaGoruntuleme(int? BirimId, int? ArizaTurId)
        {
            var arizalarr = entity.ArızaBildirim
                .Include(a => a.Birim)
                .Include(a => a.ArızaTur)
                .Include(a => a.Durum)
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
        // Arıza Türü Ekleme / Çıkarma
        [HttpGet]
        public ActionResult ArizaTurEkle()
        {
            var model = new AdminPanelViewModel
            {
                ArizaTurleri = entity.ArızaTur.ToList() // Arıza türlerini listele
            };

            return View(model);
        }

        // Arıza Türü Ekleme
        [HttpPost]
        public ActionResult ArizaTurEkle(string TurAd)
        {
            if (!string.IsNullOrEmpty(TurAd))
            {
                ArızaTur arizaTur = new ArızaTur
                {
                    Ad = TurAd
                };
                entity.ArızaTur.Add(arizaTur);
                entity.SaveChanges();
            }

            return RedirectToAction("ArizaTurEkle");
        }

        // Arıza Türü Silme
        [HttpPost]
        public ActionResult ArizaTurSil(int Id)
        {
            var arizaTur = entity.ArızaTur.FirstOrDefault(x => x.Id == Id);
            if (arizaTur != null)
            {
                entity.ArızaTur.Remove(arizaTur);
                entity.SaveChanges();
            }

            return RedirectToAction("ArizaTurEkle");
        }

        // Birim Ekleme / Çıkarma
        [HttpGet]
        public ActionResult BirimEkle()
        {
            var model = new AdminPanelViewModel
            {
                Birimler = entity.Birim.ToList() // Birimleri listele
            };

            return View(model);
        }

        // Birim Ekleme
        [HttpPost]
        public ActionResult BirimEkle(string BirimAd)
        {
            if (!string.IsNullOrEmpty(BirimAd))
            {
                Birim birim = new Birim
                {
                    Ad = BirimAd
                };
                entity.Birim.Add(birim);
                entity.SaveChanges();
            }

            return RedirectToAction("BirimEkle");
        }

        // Birim Silme
        [HttpPost]
        public ActionResult BirimSil(int Id)
        {
            var birim = entity.Birim.FirstOrDefault(x => x.Id == Id);
            if (birim != null)
            {
                entity.Birim.Remove(birim);
                entity.SaveChanges();
            }

            return RedirectToAction("BirimEkle");
        }


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

                    entity.SaveChanges();
                    return RedirectToAction("AProfil");
                }
            }

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