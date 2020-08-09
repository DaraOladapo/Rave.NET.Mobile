using Rave.NET.API;
using Rave.NET.Models.Card;
using Rave.NET.Models.Charge;
using System;
using System.ComponentModel;
using Xamarin.Forms;

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
        private static CardParams payload = new CardParams(PbKey, ScKey, "Anonymous", "Tester", "user@example.com", 50000, "NGN", card) { TxRef = tranxRef };
        private static ChargeCard cardCharge = new ChargeCard(raveConfig);
        RaveResponse<ResponseData> chargeResult = new API.RaveResponse<ResponseData>();
        public MainPage()
        {
            InitializeComponent();
            BindingContext = card;
            CardEntryPanel.IsVisible = true;
            PINEntryPanel.IsVisible = false;
            OTPEntryPanel.IsVisible = false;
        }

        private async void OnSubmitCard(object sender, EventArgs e)
        {
            CardEntryPanel.IsVisible = false;
            OTPEntryPanel.IsVisible = false;
            chargeResult = await cardCharge.Charge(payload);
            if (chargeResult.Message == "AUTH_SUGGESTION" && chargeResult.Data.SuggestedAuth == "PIN")
            {
                PINEntryPanel.IsVisible = true;
            }
            else
            {
                await DisplayAlert(chargeResult.Status, chargeResult.Message, "OK");
                CardEntryPanel.IsVisible = true;
                PINEntryPanel.IsVisible = false;
                OTPEntryPanel.IsVisible = false;

            }
        }
        private async void OnSubmitPIN(object sender, EventArgs e)
        {
            CardEntryPanel.IsVisible = false;
            PINEntryPanel.IsVisible = false;
            payload.Pin = "3310";
            payload.SuggestedAuth = "PIN";
            chargeResult = await cardCharge.Charge(payload);
            if (chargeResult.Message == "AUTH_SUGGESTION" && chargeResult.Data.SuggestedAuth == "OTP")
            {
                OTPEntryPanel.IsVisible = true;
            }
            else
            {
                await DisplayAlert(chargeResult.Status, chargeResult.Message, "OK");
                CardEntryPanel.IsVisible = true;
                PINEntryPanel.IsVisible = false;
                OTPEntryPanel.IsVisible = false;
            }
        }


        private async void OnSubmitOTP(object sender, EventArgs e)
        {
            payload.SuggestedAuth = "OTP";
            payload.Otp = "12345";
            chargeResult = await cardCharge.Charge(payload);
            await DisplayAlert(chargeResult.Status, chargeResult.Message, "OK");
            CardEntryPanel.IsVisible = true;
            PINEntryPanel.IsVisible = false;
            OTPEntryPanel.IsVisible = false;
        }
    }
}
