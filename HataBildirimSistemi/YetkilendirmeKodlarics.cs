
        /* Yetkilendirme Paneli
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
                    Mail = kullanici.Mail,
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
                    Mail = admin.Mail,
                    KKullaniciAd = admin.AKullaniciAd,
                    KSifre = admin.ASifre,
                    BirimId = admin.BirimId
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
        
         view kodları 
        
@model HataBildirimSistemi.Models.AdminPanelViewModel
@{
    ViewBag.Title = "Yetkilendirme";
    Layout = "~/Views/Shared/_LayoutAdmin.cshtml";
}

<h2>Admin Yetkilendirme Paneli</h2>

<h3>Kullanıcılar</h3>
<table border="1" cellpadding="5" cellspacing="0">
    <tr>
        <th>Ad</th>
        <th>Soyad</th>
        <th>Birim</th>
        <th>İşlemler</th>
    </tr>
    @foreach (var k in Model.Kullanicilar)
    {
        <tr>
            <td>@k.Ad</td>
            <td>@k.Soyad</td>
            <td>@k.Birim.Ad</td>
            <td>
                @Html.ActionLink("Admin Yap", "KullaniciyiAdminYap", new { id = k.Id }) |
                @Html.ActionLink("Sil", "KullaniciSil", new { id = k.Id })
            </td>
        </tr>
    }
</table>

<h3>Adminler</h3>
<table border="1" cellpadding="5" cellspacing="0">
    <tr>
        <th>Ad</th>
        <th>Soyad</th>
        <th>İşlem</th>
    </tr>
    @foreach (var a in Model.Adminler)
    {
        <tr>
            <td>@a.Ad</td>
            <td>@a.Soyad</td>
            <td>@Html.ActionLink("Kullanıcı Yap", "AdminiKullaniciYap", new { id = a.Id })</td>
        </tr>
    }
</table>

<h3>Yetkili Servis Ekle</h3>
@using (Html.BeginForm("YetkiliServisEkle", "Admin", FormMethod.Post))
{
    <label>Ad:</label> @Html.TextBox("Ad")//max 50 hane olacak
    <br />
    <label>Soyad:</label> @Html.TextBox("Soyad")//max 50 hane olacak
    <br />
    <label>Tel No:</label> @Html.TextBox("TelNo")//max 11 hane olacak 
    <br />
    <label>Mail:</label> @Html.TextBox("Mail")//max 35 hane olacak 
    <br />
    <label>Kullanıcı Adı:</label> @Html.TextBox("YKullaniciAd")//max 20 hane olacak
    <br />
    <label>Şifre:</label> @Html.Password("YSifre")//max 20 hane olacak
    <br />
    <label>Arıza Türü:</label>
    @Html.DropDownList("ArizaTurId", new SelectList(Model.ArizaTurleri, "Id", "Ad"))
    <br />
    <br />
    <input type="submit" value="Ekle" />
}

         
         
         */




//    // GET: ForgotPassword
//    public ActionResult ForgotPassword()
//    {
//        ViewBag.Mesaj = "";
//        return View();
//    }

//    // POST: ForgotPassword
//    [HttpPost]
//    public ActionResult ForgotPassword(string email)
//    {
//        var kullanici = entity.Kullanici.FirstOrDefault(k => k.KKullaniciAd == email);
//        if (kullanici != null)
//        {
//            // Basit yeni şifre üret (örnek: 6 karakterli)
//            var yeniSifre = Guid.NewGuid().ToString("N").Substring(0, 6);
//            kullanici.KSifre = yeniSifre; // Gerçek projede hashlenmeli!
//            entity.SaveChanges();

//            ViewBag.Mesaj = $"Yeni şifreniz: {yeniSifre}";
//        }
//        else
//        {
//            ViewBag.Mesaj = "Bu e-posta ile kayıtlı kullanıcı bulunamadı.";
//        }

//        return View();
//    }
//}