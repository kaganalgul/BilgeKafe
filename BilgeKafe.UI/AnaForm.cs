﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BilgeKafe.Data;
using BilgeKafe.UI.Properties;

namespace BilgeKafe.UI
{
    public partial class AnaForm : Form
    {
        KafeVeri db = new KafeVeri();

        public AnaForm()
        {
            OrnekUrunleriOlustur();
            InitializeComponent();
            MasalariOlustur();
        }

        private void OrnekUrunleriOlustur()
        {
            db.Urunler.Add(new Urun() { UrunAd = "Kola", BirimFiyat = 5.99m });
            db.Urunler.Add(new Urun() { UrunAd = "Çay", BirimFiyat = 4.50m });
        }

        private void MasalariOlustur()
        {
            #region Image Listesinin Oluşturulması
            ImageList imageList = new ImageList();
            imageList.Images.Add("empty", Resources.emptytable);
            imageList.Images.Add("full", Resources.fulltable);
            imageList.ImageSize = new Size(64, 64);
            lvwMasalar.LargeImageList = imageList;
            #endregion

            for (int i = 1; i <= db.MasaAdet; i++)
            {
                ListViewItem lvi = new ListViewItem($"Masa {i}");
                lvi.Tag = i;
                lvi.ImageKey = "empty";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvwMasalar.SelectedItems[0];
            lvi.ImageKey = "full";
            int masaNo = (int)lvi.Tag;

            // Tıklanan masaya ait(varsa) siparişi bul
            Siparis siparis = db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);

            //Eğer sipariş henüz oluşturulmadıysa (o masaya ait)
            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                db.AktifSiparisler.Add(siparis);
            }
            
            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            frmSiparis.ShowDialog();

            if (siparis.Durum != SiparisDurum.Aktif)
            {
                lvi.ImageKey = "empty";
            }
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparislerForm(db).ShowDialog();
        }
    }
}
