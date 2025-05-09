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
    using System.ComponentModel.DataAnnotations;

    public partial class Admin
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string TelNo { get; set; }
        public Nullable<int> BirimId { get; set; }
        public string AKullaniciAd { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "�ifre 6 ile 20 karakter aras�nda olmal�d�r.")]
        public string ASifre { get; set; }
        public Nullable<int> YetkiId { get; set; }
    
        public virtual Birim Birim { get; set; }
        public virtual Yetki Yetki { get; set; }
    }
}
