using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content;
using Newtonsoft.Json;
using CopyThisServer.Model.Server.Data;
using System.ComponentModel;
using System.Net.Http;
using Org.Apache.Http.Client;
using System.Text;
using System.Net.Mime;

namespace CopyThis_Mobile.Droid
{
    [Activity(Label = "CopyThis_Mobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity , INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        String ip;
        String port;
        String secret;

        public string Ip
        {
            get
            {
                return ip;
            }
            set
            {
                if (ip != value)
                {
                    ip = value;
                    OnPropertyChanged(nameof(Ip));
                }
            }
        }

        public string Port
        {
            get
            {
                return port;
            }
            set
            {
                if (port != value)
                {
                    port = value;
                    OnPropertyChanged(nameof(Port));
                }
            }
        }

        public string Secret
        {
            get
            {
                return secret;
            }
            set
            {
                if (secret != value)
                {
                    secret = value;
                    OnPropertyChanged(nameof(Secret));
                }
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        protected override void OnCreate(Bundle bundle)
        {
            ip = "IP";
            port = "Port";
            secret = "Secret";

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        async void OnSendClipboardClicked(object sender, EventArgs args)
        {


            ClipboardManager clipboardManager = (ClipboardManager)GetSystemService(ClipboardService);
            ClipData clip = clipboardManager.PrimaryClip;
            String clipBoardText;


            if (clip != null && clip.ItemCount > 0)
            {
                clipBoardText = clip.GetItemAt(0).CoerceToText(Application.Context);

                var clipboardJson = JsonConvert.SerializeObject(new ClipboardDto { Content = clipBoardText });
                HttpClient oHttpClient = new HttpClient();

                String url = "http:\"" + ip;

                var oTaskPostAsync = oHttpClient.PostAsync(url, new StringContent(clipboardJson, Encoding.UTF8));
            }
        }
    }
}

