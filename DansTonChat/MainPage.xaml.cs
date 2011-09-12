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
    }
}