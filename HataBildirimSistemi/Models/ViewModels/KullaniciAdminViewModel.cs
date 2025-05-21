using System.Collections.Generic;
using HataBildirimSistemi.Models;  // Kullanici, Admin, Birim sınıflarının namespace'i

namespace HataBildirimSistemi.Models.ViewModels
{
    public class KullaniciAdminViewModel
    {
        public List<Kullanici> Kullanicilar { get; set; }
        public List<Birim> Birimler { get; set; }
        public string SeciliRol { get; set; }
    }
}
