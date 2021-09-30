using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BilgeKafe.Data;
using BilgeKafe.UI.Properties;
using Newtonsoft.Json;

namespace BilgeKafe.UI
{
    public partial class AnaForm : Form
    {
        KafeVeri db = new KafeVeri();

        public AnaForm()
        {                        
            InitializeComponent();
            MasalariOlustur();
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
                lvi.ImageKey = db.Siparisler.Any(s => s.MasaNo == i && s.Durum == SiparisDurum.Aktif) ? "full" : "empty";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvwMasalar.SelectedItems[0];
            lvi.ImageKey = "full";
            int masaNo = (int)lvi.Tag;

            // Tıklanan masaya ait(varsa) siparişi bul
            Siparis siparis = db.Siparisler.FirstOrDefault(x => x.MasaNo == masaNo && x.Durum == SiparisDurum.Aktif);

            //Eğer sipariş henüz oluşturulmadıysa (o masaya ait)
            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                db.Siparisler.Add(siparis);
            }
            
            SiparisForm frmSiparis = new SiparisForm(db, siparis);
            frmSiparis.MasaTasindi += FrmSiparis_MasaTasindi;
            frmSiparis.ShowDialog();

            if (siparis.Durum != SiparisDurum.Aktif)
            {
                lvi.ImageKey = "empty";
            }
        }

        private void FrmSiparis_MasaTasindi(object sender, MasaTasindiEventArgs e)
        {
            foreach (ListViewItem lvi in lvwMasalar.Items)
            {
                if ((int)lvi.Tag == e.EskiMasaNo)
                {
                    lvi.ImageKey = "empty";
                }
                if ((int)lvi.Tag == e.YeniMasaNo)
                {
                    lvi.ImageKey = "full";
                }
            }
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparislerForm(db).ShowDialog();
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm(db).ShowDialog();
        }
    }
}
