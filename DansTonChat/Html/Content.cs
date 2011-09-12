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
using System.Collections.Generic;

namespace DansTonChat.Html
{
    public class Content
    {
        public string Text { get; set; }
        public List<string> Images {get; private set;}

        public Content()
        {
            Images = new List<string>();
        }
    }
}
