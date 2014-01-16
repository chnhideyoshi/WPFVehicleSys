﻿#pragma checksum "..\..\..\View\MusicListView.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E333F119A7F7DA0A269D30D0E2B7E8CB"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Expression.Interactivity.Core;
using Microsoft.Expression.Interactivity.Input;
using Microsoft.Expression.Interactivity.Layout;
using Microsoft.Expression.Interactivity.Media;
using MusicPlayer.View;
using MusicPlayer.ViewModel;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace MusicPlayer {
    
    
    /// <summary>
    /// MusicListView
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class MusicListView : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MusicPlayer.MusicListView UserControl;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid LayoutRoot;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualStateGroup ListVisible;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualState DirList;
        
        #line default
        #line hidden
        
        
        #line 44 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualState ArtistList;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualState AllMusicList;
        
        #line default
        #line hidden
        
        
        #line 88 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.VisualState AlbumView;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MusicPlayer.View.DirListView dirListView;
        
        #line default
        #line hidden
        
        
        #line 120 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MusicPlayer.View.ArtistListView artistListView;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MusicPlayer.View.AllMusicListView allMusicListView;
        
        #line default
        #line hidden
        
        
        #line 126 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MusicPlayer.View.AlbumView albumView;
        
        #line default
        #line hidden
        
        
        #line 143 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AllMusciCB;
        
        #line default
        #line hidden
        
        
        #line 158 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox DirCB;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox AlbumCB;
        
        #line default
        #line hidden
        
        
        #line 193 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox ArtistCB;
        
        #line default
        #line hidden
        
        
        #line 211 "..\..\..\View\MusicListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox CurrentCB;
        
        #line default
        #line hidden
        
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
            System.Uri resourceLocater = new System.Uri("/MusicPlayer;component/view/musiclistview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\View\MusicListView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.UserControl = ((MusicPlayer.MusicListView)(target));
            return;
            case 2:
            this.LayoutRoot = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.ListVisible = ((System.Windows.VisualStateGroup)(target));
            return;
            case 4:
            this.DirList = ((System.Windows.VisualState)(target));
            return;
            case 5:
            this.ArtistList = ((System.Windows.VisualState)(target));
            return;
            case 6:
            this.AllMusicList = ((System.Windows.VisualState)(target));
            return;
            case 7:
            this.AlbumView = ((System.Windows.VisualState)(target));
            return;
            case 8:
            this.dirListView = ((MusicPlayer.View.DirListView)(target));
            return;
            case 9:
            this.artistListView = ((MusicPlayer.View.ArtistListView)(target));
            return;
            case 10:
            this.allMusicListView = ((MusicPlayer.View.AllMusicListView)(target));
            return;
            case 11:
            this.albumView = ((MusicPlayer.View.AlbumView)(target));
            return;
            case 12:
            this.AllMusciCB = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 13:
            this.DirCB = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 14:
            this.AlbumCB = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 15:
            this.ArtistCB = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 16:
            this.CurrentCB = ((System.Windows.Controls.CheckBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

