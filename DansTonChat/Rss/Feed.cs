using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;
using System.IO.IsolatedStorage;
using System.IO;
using System.Runtime.Serialization;

namespace DansTonChat.Rss
{
    [CollectionDataContract]
    public class Feed : ObservableCollection<FeedItem>
    {
        public string Url { get; set; }

        public Feed()
        {

        }

        public void Save()
        {
            var file = IsolatedStorageFile.GetUserStoreForApplication().CreateFile("dtc.xml");
            DataContractSerializer serializer = new DataContractSerializer(typeof(Feed), new Type[]{typeof(FeedItem)});
            serializer.WriteObject(file, this);
            file.Close();

            //var channel = new XElement("channel");
            //foreach (var item in this)
            //{
            //    channel.Add(item.GetXML());
            //}
            
            //var writer = new StreamWriter(file);
            //writer.Write(channel);
            //writer.Close();
        }

        public void Load()
        {
            if (IsolatedStorageFile.GetUserStoreForApplication().FileExists("dtc.xml"))
            {
                var file = IsolatedStorageFile.GetUserStoreForApplication().OpenFile("dtc.xml", FileMode.Open);
                DataContractSerializer serializer = new DataContractSerializer(typeof(Feed), new Type[] { typeof(FeedItem) });
                var loadedFeed = serializer.ReadObject(file) as Feed;
                file.Close();
                if (loadedFeed != null)
                {
                    this.Clear();
                    foreach (var item in loadedFeed)
                    {
                        this.Add(item);
                    }
                }
            }
        }

        public void Update()
        {
            WebClient client = new WebClient();
            client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
            client.DownloadStringAsync(new Uri(Url, UriKind.Absolute));
        }

        void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Debug.WriteLine(e.Error.Message);
                Debug.WriteLine(e.Error.StackTrace);
                Error(this, new EventArgs());
            }
            else
            {
                Update(XElement.Parse(e.Result));
                Updated(this, new EventArgs());
            }
        }

        private void Update(XElement channel)
        {
            this.Clear();
            foreach (var item in channel.Descendants("item"))
            {
                this.Add(new FeedItem(item));
            }            
        }


        public event EventHandler Error;
        public event EventHandler Updated;
    }
}
