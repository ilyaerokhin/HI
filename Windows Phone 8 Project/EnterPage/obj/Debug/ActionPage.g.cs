﻿#pragma checksum "C:\Users\user\Desktop\HI\Windows Phone 8 Project\EnterPage\ActionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "9E1343400BC1C812078E3FC86B2F4E33"
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
    
    
    public partial class ActionPage : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock Title;
        
        internal System.Windows.Controls.TextBox Friend_box;
        
        internal System.Windows.Controls.ListBox list1;
        
        internal System.Windows.Controls.Image MyPhoto;
        
        internal System.Windows.Controls.StackPanel Camera;
        
        internal System.Windows.Controls.StackPanel Galery;
        
        internal System.Windows.Controls.Image cam;
        
        internal System.Windows.Controls.StackPanel Delete;
        
        internal System.Windows.Controls.StackPanel Load;
        
        internal System.Windows.Controls.TextBox status;
        
        internal System.Windows.Controls.ListBox list_potential;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/EnterPage;component/ActionPage.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.Title = ((System.Windows.Controls.TextBlock)(this.FindName("Title")));
            this.Friend_box = ((System.Windows.Controls.TextBox)(this.FindName("Friend_box")));
            this.list1 = ((System.Windows.Controls.ListBox)(this.FindName("list1")));
            this.MyPhoto = ((System.Windows.Controls.Image)(this.FindName("MyPhoto")));
            this.Camera = ((System.Windows.Controls.StackPanel)(this.FindName("Camera")));
            this.Galery = ((System.Windows.Controls.StackPanel)(this.FindName("Galery")));
            this.cam = ((System.Windows.Controls.Image)(this.FindName("cam")));
            this.Delete = ((System.Windows.Controls.StackPanel)(this.FindName("Delete")));
            this.Load = ((System.Windows.Controls.StackPanel)(this.FindName("Load")));
            this.status = ((System.Windows.Controls.TextBox)(this.FindName("status")));
            this.list_potential = ((System.Windows.Controls.ListBox)(this.FindName("list_potential")));
        }
    }
}

