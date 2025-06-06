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
    
    public partial class Kullanici
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kullanici()
        {
            this.ArızaBildirim = new HashSet<ArızaBildirim>();
            this.ServisArizaTur = new HashSet<ServisArizaTur>();
        }
    
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int BirimId { get; set; }
        public string TelNo { get; set; }
        public string KKullaniciAd { get; set; }
        public string KSifre { get; set; }
        public int YetkiId { get; set; }
        public Nullable<int> ArizaTurId { get; set; }
        public Nullable<int> AltArizaTurId { get; set; }
        public Nullable<int> AltBirimId { get; set; }
    
        public virtual AltBirim AltBirim { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ArızaBildirim> ArızaBildirim { get; set; }
        public virtual ArızaTur ArızaTur { get; set; }
        public virtual Birim Birim { get; set; }
        public virtual Yetki Yetki { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServisArizaTur> ServisArizaTur { get; set; }
    }
}
