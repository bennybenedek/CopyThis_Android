using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.Clipboard;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CopyThis_Mobile.ViewModels
{
    [Preserve(AllMembers = true)]
    internal class MainViewModel : INotifyPropertyChanged
    {
        private const string ClipboardDto = "{{\"content\": {0} }}";
        private const string PictureDto = "{{ \"filename\": {0}, \"content\": {1} }}";

        public MainViewModel()
        {
            SendClipboardCommand = new Command(async () => await OnSendClipboardClicked());
            SendPictureCommand = new Command(async () => await OnSendPictureClicked());
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

        public ICommand SendClipboardCommand { get; }
        public ICommand SendPictureCommand { get; }

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
                var clipboardJson = string.Format(ClipboardDto, s);
                
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

        private async Task OnSendPictureClicked()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                var picture = await CrossMedia.Current.PickPhotoAsync();
                var fileName = Path.GetFileName(picture.Path);

                string content;

                using (var memstream = new MemoryStream())
                {
                    await picture.GetStreamWithImageRotatedForExternalStorage().CopyToAsync(memstream);
                    content = Convert.ToBase64String(memstream.ToArray());
                }

                fileName = JsonConvert.SerializeObject(fileName);
                content = JsonConvert.SerializeObject(content);

                var json = string.Format(PictureDto, fileName, content);
                
                using (var http = Http())
                {
                    await http.PostAsync("/picture", new StringContent(json, Encoding.UTF8));
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