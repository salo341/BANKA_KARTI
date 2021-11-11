using BANKA_KARTI.Module.BusinessObjects.TANIMLAR;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace BANKA_KARTI.Module.BusinessObjects.HAREKET
{
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCriteria("Miktaruyari", DefaultContexts.Save, "Miktar > 0",
    "Miktar 0 veya - olamaz.!", SkipNullOrEmptyValues = false)]
    public class HAREKET_BankaParaTransfer : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior100, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HAREKET_BankaParaTransfer(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private TANIMLAR_MusteriKarti _musteri_ad;
        public TANIMLAR_MusteriKarti GondericiAdi
        {
            get { return _musteri_ad; }
            set { SetPropertyValue(nameof(GondericiAdi), ref _musteri_ad, value); }
        }


        private TANIMLAR_BankaKarti _cikan_banka;

        public TANIMLAR_BankaKarti CikisBanka
        {
            get
            {
                return _cikan_banka;
            }
            set
            {
                SetPropertyValue(nameof(CikisBanka), ref _cikan_banka, value);
            }
        }
        private TANIMLAR_DovizKarti _doviz_secimi;
        public TANIMLAR_DovizKarti GöndericiDoviz
        {
            get { return _doviz_secimi; }
            set { SetPropertyValue(nameof(GöndericiDoviz), ref _doviz_secimi, value); }
        }

        private TANIMLAR_MusteriKarti _alici_ad;
        public TANIMLAR_MusteriKarti AliciAdi
        {
            get { return _alici_ad; }
            set { SetPropertyValue(nameof(AliciAdi), ref _alici_ad, value); }
        }
        private TANIMLAR_BankaKarti _giren_banka;
        public TANIMLAR_BankaKarti GirisBanka
        {
            get
            {
                return _giren_banka;
            }
            set
            {
                SetPropertyValue(nameof(GirisBanka), ref _giren_banka, value);
            }
        }
        private TANIMLAR_DovizKarti _doviz_secimi2;
        public TANIMLAR_DovizKarti AliciDoviz
        {
            get { return _doviz_secimi2; }
            set { SetPropertyValue(nameof(AliciDoviz), ref _doviz_secimi2, value); }
        }


        private decimal _miktar;
        public decimal Miktar
        {
            get
            {
                return _miktar;
            }
            set
            {
                SetPropertyValue(nameof(Miktar), ref _miktar, value);
            }
        }




        private DateTime _tarih;
        public DateTime Tarih
        {
            get
            {
                return _tarih;
            }
            set
            {
                SetPropertyValue(nameof(Tarih), ref _tarih, value);
            }
        }


        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            if (propertyName == nameof(Miktar) && CikisBanka != null && GöndericiDoviz != null && GondericiAdi != null && GirisBanka != null && AliciDoviz != null && AliciAdi != null)
            {
                var girisBanka = Session.Query<HAREKET_BankaParaHareket>().Where(i => i.Dovizz == AliciDoviz && i.Banka == GirisBanka && i.MusteriAdi == AliciAdi).FirstOrDefault();
                var cikisBanka = Session.Query<HAREKET_BankaParaHareket>().Where(i => i.Dovizz == GöndericiDoviz && i.Banka == CikisBanka && i.MusteriAdi == GondericiAdi).FirstOrDefault();
                if (propertyName == "Miktar" || propertyName == "GirisBanka" || propertyName == "CikisBanka")
                {
                    girisBanka.Miktarlar -= (decimal)oldValue;
                    cikisBanka.Miktarlar += (decimal)oldValue;
                    //GirisBanka.AD = (string)oldValue;
                    //CikisBanka.AD = (string)oldValue;
                }
                    base.OnChanged(propertyName, oldValue, newValue);

                
                

                
                if (GöndericiDoviz == AliciDoviz && girisBanka != null && cikisBanka != null)
                {
                    girisBanka.Miktarlar += Miktar;
                    cikisBanka.Miktarlar -= Miktar;
                }
                if (girisBanka == null && cikisBanka != null)
                {
                    var stokHareket = new HAREKET_BankaParaHareket(Session)
                    {
                        MusteriAdi = GondericiAdi,
                        Banka = GirisBanka,
                        Dovizz = AliciDoviz,
                        Miktarlar = Miktar
                    };

                    Session.Save(stokHareket);
                    cikisBanka.Miktarlar -= Miktar;
                }
                if (girisBanka != null && cikisBanka == null)
                {
                    var stokHareket2 = new HAREKET_BankaParaHareket(Session)
                    {
                        MusteriAdi=AliciAdi,
                        Banka = GirisBanka,
                        Dovizz = GöndericiDoviz,
                        Miktarlar = Miktar
                    };

                    Session.Save(stokHareket2);
                    girisBanka.Miktarlar += Miktar;
                }
                if (GöndericiDoviz != AliciDoviz)
                {
                    cikisBanka.Miktarlar -= Miktar;
                    girisBanka.Miktarlar += ((Miktar * GöndericiDoviz.KUR) / AliciDoviz.KUR);
                }
                
            }

            //if (propertyName == nameof(Miktar) && CikisBanka != null && GirisBanka != null && Doviz != null)
            //{
            //    var girisBanka = Session.Query<HAREKET_BankaParaHareket>().Where(i => i.Dovizz.ID == Doviz.ID && i.Banka.ID == GirisBanka.ID).FirstOrDefault();
            //    var cikisBanka = Session.Query<HAREKET_BankaParaHareket>().Where(i => i.Dovizz.ID == Doviz.ID && i.Banka.ID == CikisBanka.ID).FirstOrDefault();

            //    if (girisBanka != null && cikisBanka != null)
            //    {
            //        girisBanka.Miktarlar += Miktar;
            //        cikisBanka.Miktarlar -= Miktar;
            //    }

            //   

        }














        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue(nameof(PersistentProperty), ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}
