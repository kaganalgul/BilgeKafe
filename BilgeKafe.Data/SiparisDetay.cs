﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BilgeKafe.Data
{
    [Table("SiparisDetaylar")]
    public class SiparisDetay
    {
        public int Id { get; set; }

        public string UrunAd { get; set; }

        public decimal BirimFiyat { get; set; }

        public int Adet { get; set; }

        [NotMapped]
        public string TutarTL { get { return $"{Tutar():n2}₺"; } }

        [ForeignKey("Urun")]
        public int UrunId { get; set; }
        public virtual Urun Urun { get; set; }

        [ForeignKey("Siparis")]
        public int SiparisId { get; set; }
        public virtual Siparis Siparis { get; set; }

        public decimal Tutar()
        {
            return Adet * BirimFiyat; 
        }
    }
}
