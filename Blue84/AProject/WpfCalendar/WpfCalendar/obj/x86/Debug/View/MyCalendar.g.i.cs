﻿#pragma checksum "..\..\..\..\View\MyCalendar.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5403AB60AFC3CB7C588F1E26507F8293"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.235
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace WpfCalendar.View {
    
    
    /// <summary>
    /// MyCalendar
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
    public partial class MyCalendar : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 258 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid myCalendar;
        
        #line default
        #line hidden
        
        
        #line 259 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel CalendarStack;
        
        #line default
        #line hidden
        
        
        #line 268 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel TitleStackPanel;
        
        #line default
        #line hidden
        
        
        #line 273 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Backward;
        
        #line default
        #line hidden
        
        
        #line 281 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button YearAndMonth;
        
        #line default
        #line hidden
        
        
        #line 288 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Forward;
        
        #line default
        #line hidden
        
        
        #line 295 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid CalendarGrid;
        
        #line default
        #line hidden
        
        
        #line 307 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CalendarDays;
        
        #line default
        #line hidden
        
        
        #line 315 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CalendarMonths;
        
        #line default
        #line hidden
        
        
        #line 328 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ShowLunar;
        
        #line default
        #line hidden
        
        
        #line 336 "..\..\..\..\View\MyCalendar.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ShowGenerate;
        
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
            System.Uri resourceLocater = new System.Uri("/WpfCalendar;component/view/mycalendar.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\MyCalendar.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
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
            this.myCalendar = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.CalendarStack = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.TitleStackPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 4:
            this.Backward = ((System.Windows.Controls.Button)(target));
            
            #line 276 "..\..\..\..\View\MyCalendar.xaml"
            this.Backward.Click += new System.Windows.RoutedEventHandler(this.Backward_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.YearAndMonth = ((System.Windows.Controls.Button)(target));
            
            #line 284 "..\..\..\..\View\MyCalendar.xaml"
            this.YearAndMonth.Click += new System.Windows.RoutedEventHandler(this.YearAndMonth_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.Forward = ((System.Windows.Controls.Button)(target));
            
            #line 291 "..\..\..\..\View\MyCalendar.xaml"
            this.Forward.Click += new System.Windows.RoutedEventHandler(this.Forword_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.CalendarGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 8:
            this.CalendarDays = ((System.Windows.Controls.Canvas)(target));
            return;
            case 9:
            this.CalendarMonths = ((System.Windows.Controls.Canvas)(target));
            return;
            case 10:
            this.ShowLunar = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 11:
            this.ShowGenerate = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

