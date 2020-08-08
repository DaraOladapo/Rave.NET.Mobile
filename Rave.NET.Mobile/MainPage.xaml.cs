using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rave.NET;
using Rave.NET.Models.Card;
using Rave.NET.Models.Charge;

namespace Rave.NET.Mobile
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private static string PbKey = "FLWPUBK_TEST-e249f67527cbc6331261c8d935fa5735-X";
        private static string ScKey = "FLWSECK_TEST-3bca3e0b02cff2b0f79d7b83fb81fb67-X";
        private static string tranxRef = "454839";
        private static string CardNumber = "5377283645077450";
        private static string ExpiryMonth = "09";
        private static string ExpiryYear = "21";
        private static string CVV = "789";

        private static RaveConfig raveConfig = new RaveConfig(PbKey, ScKey, false);
        private static Card card = new Card(CardNumber, ExpiryMonth, ExpiryYear, CVV);
        private static CardParams payload = new CardParams(PbKey, ScKey, "Anonymous", "Tester", "user@example.com", 50000, "NGN");
        private static ChargeCard cardCharge = new ChargeCard(raveConfig);

        public MainPage()
        {
            InitializeComponent();
            CardEntryPanel.IsVisible = true;
            PINEntryPanel.IsVisible = false;
            OTPEntryPanel.IsVisible = false;
        }

        private void OnSubmitCard(object sender, EventArgs e)
        {
            CardEntryPanel.IsVisible = false;
            PINEntryPanel.IsVisible = true;
            OTPEntryPanel.IsVisible = false;
        }
        private void OnSubmitPIN(object sender, EventArgs e)
        {
            CardEntryPanel.IsVisible = false;
            PINEntryPanel.IsVisible = false;
            OTPEntryPanel.IsVisible = true;
        }


        private async void OnSubmitOTP(object sender, EventArgs e)
        {
            var chargeResult = await cardCharge.Charge(payload);
            //await DisplayAlert(chargeResult.Status);
        }
    }
}
