﻿#pragma checksum "C:\Users\Илья\Desktop\HI 06.01.15\EnterPage\ActionPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2D6BEB30A7A841BE730572E6A6E286A3"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34014
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
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBox Friend_box;
        
        internal System.Windows.Controls.ListBox list1;
        
        internal System.Windows.Controls.Image MyPhoto;
        
        internal System.Windows.Controls.StackPanel Camera;
        
        internal System.Windows.Controls.StackPanel Galery;
        
        internal System.Windows.Controls.Image cam;
        
        internal System.Windows.Controls.StackPanel Delete;
        
        internal System.Windows.Controls.StackPanel Exit;
        
        internal System.Windows.Controls.TextBox status;
        
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
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.Friend_box = ((System.Windows.Controls.TextBox)(this.FindName("Friend_box")));
            this.list1 = ((System.Windows.Controls.ListBox)(this.FindName("list1")));
            this.MyPhoto = ((System.Windows.Controls.Image)(this.FindName("MyPhoto")));
            this.Camera = ((System.Windows.Controls.StackPanel)(this.FindName("Camera")));
            this.Galery = ((System.Windows.Controls.StackPanel)(this.FindName("Galery")));
            this.cam = ((System.Windows.Controls.Image)(this.FindName("cam")));
            this.Delete = ((System.Windows.Controls.StackPanel)(this.FindName("Delete")));
            this.Exit = ((System.Windows.Controls.StackPanel)(this.FindName("Exit")));
            this.status = ((System.Windows.Controls.TextBox)(this.FindName("status")));
        }
    }
}

