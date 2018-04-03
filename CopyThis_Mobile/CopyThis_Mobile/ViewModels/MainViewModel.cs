using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CopyThis_Mobile.DTOs;
using Newtonsoft.Json;
using Plugin.Clipboard;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CopyThis_Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    internal class MainViewModel : INotifyPropertyChanged
    {
        private const string ClipboadDto = "{{\"content\": {0} }}";

        public MainViewModel()
        {
            SendClipboardCommand = new Command(async () => await OnSendClipboardClicked());
        }
        
        private string ip;
        private string port;
        private string secret;

        public string Ip
        {
            get => ip;
            set => Set(ref ip, value);
        }

        public string Port
        {
            get => port;
            set => Set(ref port, value);
        }

        public string Secret
        {
            get => secret;
            set => Set(ref secret, value);
        }

        public ICommand SendClipboardCommand { get; private set; }

        private HttpClient Http()
        {
            var uri = new UriBuilder
            {
                Scheme = "http",
                Host = Ip.Trim(),
                Port = int.Parse(Port.Trim())
            }.Uri;

            var http = new HttpClient
            {
                BaseAddress = uri
            };

            http.DefaultRequestHeaders.TryAddWithoutValidation("ApiAccess", Secret);
            http.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

            return http;
        }
        
        private async Task OnSendClipboardClicked()
        {
            try
            {
                var text = await CrossClipboard.Current.GetTextAsync();

                var s = JsonConvert.SerializeObject(text);
                var clipboardJson = string.Format(ClipboadDto, s);
                
                using (var http = Http())
                {
                    await http.PostAsync("/clipboard", new StringContent(clipboardJson, Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                Debug.Write(e.Message);
                Debug.Write(e.StackTrace);
                throw;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}