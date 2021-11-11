using BANKA_KARTI.Module.BusinessObjects.TANIMLAR;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
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
    [Appearance("HAREKET_BankaParaHareket2", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "Miktarlar<=200", Context = "ListView", BackColor = "red", FontColor = "white", Priority = 1)]
    [Appearance("HAREKET_BankaParaHareket", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "Miktarlar<1000&& Miktarlar>200", Context = "ListView", BackColor = "yellow", FontColor = "Maroon", Priority = 2)]
    [Appearance("HAREKET_BankaParaHareket3", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "Miktarlar>=1000", Context = "ListView", BackColor = "green", FontColor = "white", Priority = 3)]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [RuleCriteria("MiktarlarUyari", DefaultContexts.Save, "Miktarlar >= 0",
   "Miktarlar - olamaz", SkipNullOrEmptyValues = false)]
    public class HAREKET_BankaParaHareket : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public HAREKET_BankaParaHareket(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private TANIMLAR_MusteriKarti _musteri_ad;
        [Association("MusteriAdi")]
        public TANIMLAR_MusteriKarti MusteriAdi
        {
            get { return _musteri_ad; }
            set { SetPropertyValue(nameof(MusteriAdi), ref _musteri_ad, value); }
        }
        private TANIMLAR_BankaKarti _banka;
        [Association("banka")]
        public TANIMLAR_BankaKarti Banka
        {
            get
            {
                return _banka;
            }
            set
            {
                SetPropertyValue(nameof(Banka), ref _banka, value);
            }

        }

        private TANIMLAR_DovizKarti _doviz;
        [Association("Doviz")]
        public TANIMLAR_DovizKarti Dovizz
        {
            get
            {
                return _doviz;
            }
            set
            {
                SetPropertyValue(nameof(Dovizz), ref _doviz, value);
            }

        }


        private decimal _miktar;
        public decimal Miktarlar
        {
            get { return _miktar; }
            set { SetPropertyValue(nameof(Miktarlar), ref _miktar, value); }
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