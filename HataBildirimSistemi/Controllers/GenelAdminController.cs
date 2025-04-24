using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HataBildirimSistemi.Controllers
{
    public class GenelAdminController : Controller
    {
        HataBildirimModelMvcEntities2 entity = new HataBildirimModelMvcEntities2();
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
    }
}