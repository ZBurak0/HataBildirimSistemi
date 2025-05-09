//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HataBildirimSistemi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ArızaBildirim
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public Nullable<int> ArizaTurId { get; set; }
        public string KullaniciAd { get; set; }
        public Nullable<int> BirimId { get; set; }
        public string Aciklama { get; set; }
        public Nullable<System.DateTime> Tarih { get; set; }
        public string DosyaYolu { get; set; }
        public Nullable<int> DurumId { get; set; }
    
        public virtual ArızaTur ArızaTur { get; set; }
        public virtual Birim Birim { get; set; }
        public virtual Durum Durum { get; set; }
    }
}
