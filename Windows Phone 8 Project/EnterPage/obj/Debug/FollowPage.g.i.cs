﻿#pragma checksum "C:\Users\Илья\Desktop\Windows Phone 8 Project\EnterPage\FollowPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5FF995D37B736EA46F1E6209C3A1E628"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34209
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace EnterPage {
    
    
    public partial class FollowPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid Top;
        
        internal System.Windows.Controls.TextBlock NAME;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock distance;
        
        internal System.Windows.Controls.Image Image_friend;
        
        internal System.Windows.Controls.TextBlock time;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/EnterPage;component/FollowPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Top = ((System.Windows.Controls.Grid)(this.FindName("Top")));
            this.NAME = ((System.Windows.Controls.TextBlock)(this.FindName("NAME")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.distance = ((System.Windows.Controls.TextBlock)(this.FindName("distance")));
            this.Image_friend = ((System.Windows.Controls.Image)(this.FindName("Image_friend")));
            this.time = ((System.Windows.Controls.TextBlock)(this.FindName("time")));
        }
    }
}

