using HataBildirimSistemi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace HataBildirimSistemi.Controllers
{
    
    public class KullaniciController : Controller
    {
        HataBildirimModelMvcEntities entity = new HataBildirimModelMvcEntities();
        // GET: Kullanici
        public ActionResult Bildirim()
        {
            var Arizalar = (from p in entity.ArızaTur select p).ToList();
            ViewBag.Arizalar = Arizalar;

            return View();
        }
        [HttpPost]
        public ActionResult Bildirim(FormCollection formCollection)
        {
            var Arizalar = (from p in entity.ArızaTur select p).ToList();
            ViewBag.Arizalar = Arizalar;

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

            arzb.Ad = ArizaAdi;
            arzb.KullaniciAd = Ad;
            arzb.BirimId = BirimId;
            arzb.ArizaTurId = ArizaTurId;
            arzb.Aciklama = Aciklama;
            arzb.Tarih = tarih;

            entity.ArızaBildirim.Add(arzb);
            entity.SaveChanges();

            return RedirectToAction("Listeleme");
        }
        public ActionResult Listeleme()
        {

            return View();
        }
    }
}