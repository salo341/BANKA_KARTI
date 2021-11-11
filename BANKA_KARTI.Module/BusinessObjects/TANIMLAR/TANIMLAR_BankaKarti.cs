using BANKA_KARTI.Module.BusinessObjects.HAREKET;
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

namespace BANKA_KARTI.Module.BusinessObjects.TANIMLAR
{
    [DefaultClassOptions]
    [DefaultProperty("AD")]

    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TANIMLAR_BankaKarti : TANIMLAR_KartTanım
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public TANIMLAR_BankaKarti(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private TANIMLAR_MusteriKarti _musteri_ad;
        public TANIMLAR_MusteriKarti MusteriAdi
        {
            get { return _musteri_ad; }
            set { SetPropertyValue(nameof(MusteriAdi), ref _musteri_ad, value); }
        }

        private string _banka_iban;
        public string BankaIban
        {
            get { return _banka_iban; }
            set { SetPropertyValue(nameof(BankaIban), ref _banka_iban, value); }
        }




        [Association("banka")]
        public XPCollection<HAREKET_BankaParaHareket> Banka
        {
            get
            {
                return GetCollection<HAREKET_BankaParaHareket>(nameof(Banka));
            }
        }

        public decimal Miktarlar { get; internal set; }





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