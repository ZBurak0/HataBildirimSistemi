using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Generic;

namespace HataBildirimSistemi.Models
{
    public class AdminPanelViewModel
    {
        public List<Kullanici> Kullanicilar { get; set; }
        public List<ArızaTur> ArizaTurleri { get; set; }

        public List<Birim> Birimler {  get; set; }
    }
}
