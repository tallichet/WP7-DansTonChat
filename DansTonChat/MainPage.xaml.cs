using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using DansTonChat.Rss;
using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;

namespace DansTonChat
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private Feed Feed
        {
            get
            {
                return this.Resources["Feed"] as Feed;
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            SetProgressBar(true);
            Feed.Update();
        }

        private void Feed_Error(object sender, EventArgs e)
        {
            SetProgressBar(false);
            MessageBox.Show("Il est actuellement impossible de voir les derniers messages");
        }

        private void Feed_Updated(object sender, EventArgs e)
        {
            SetProgressBar(false);
            Feed.Save();
        }

        private void SetProgressBar(bool active)
        {
            ProgressGrid.Visibility = (active ? Visibility.Visible : Visibility.Collapsed);
            ProgressBar.IsIndeterminate = active;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Feed.Load();
        }

        private void Browse(object sender, EventArgs e)
        {
            var item = ListQuotes.SelectedItem as FeedItem;
            if (item == null)
            {
                MessageBox.Show("Vous devez selectionner une quote avant de choisir cette action");
                return;
            }

            var task = new WebBrowserTask();
            task.Uri = new Uri(item.Link);
            task.Show();
        }

        private void Share(object sender, EventArgs e)
        {
            var item = ListQuotes.SelectedItem as FeedItem;
            if (item == null)
            {
                MessageBox.Show("Vous devez selectionner une quote avant de choisir cette action");
                return;
            }

            var task = new ShareStatusTask();
            task.Status = "Vu sur Dans ton chat...\r\n" + item.Description;
            task.Show();            
        }

        private void ListQuotes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (ApplicationBar.MenuItems[0] as ApplicationBarMenuItem).IsEnabled = ListQuotes.SelectedItem != null;
            (ApplicationBar.MenuItems[1] as ApplicationBarMenuItem).IsEnabled = ListQuotes.SelectedItem != null;
        }        
    }
}