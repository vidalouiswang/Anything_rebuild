﻿#pragma checksum "..\..\..\UserControls\Item.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4F36041C3A01CED75DC76F49B12336E8"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Anything.UserControls;
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


namespace Anything.UserControls {
    
    
    /// <summary>
    /// Item
    /// </summary>
    public partial class Item : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 8 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Anything.UserControls.Item ME;
        
        #line default
        #line hidden
        
        
        #line 13 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Border Bdr;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Media.SolidColorBrush scb;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel dock;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image Img;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas CanvasX;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock Txt;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\..\UserControls\Item.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TxtWrite;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Anything;component/usercontrols/item.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\Item.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.ME = ((Anything.UserControls.Item)(target));
            
            #line 10 "..\..\..\UserControls\Item.xaml"
            this.ME.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.BaseExecute);
            
            #line default
            #line hidden
            
            #line 11 "..\..\..\UserControls\Item.xaml"
            this.ME.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.ME_MouseLeftButtonUp);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\UserControls\Item.xaml"
            this.ME.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.ME_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\UserControls\Item.xaml"
            this.ME.MouseMove += new System.Windows.Input.MouseEventHandler(this.ME_MouseMove);
            
            #line default
            #line hidden
            return;
            case 2:
            this.Bdr = ((System.Windows.Controls.Border)(target));
            return;
            case 3:
            this.scb = ((System.Windows.Media.SolidColorBrush)(target));
            return;
            case 4:
            this.dock = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 5:
            this.Img = ((System.Windows.Controls.Image)(target));
            return;
            case 6:
            
            #line 35 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.OpenMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 36 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AdminOpenMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 37 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.DeleteMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 38 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.LocationMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            
            #line 39 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.CreateShortcutMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            
            #line 40 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ArgumentsMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 12:
            
            #line 41 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.MoveOutMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 42 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.AttributeMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 14:
            this.CanvasX = ((System.Windows.Controls.Canvas)(target));
            return;
            case 15:
            this.Txt = ((System.Windows.Controls.TextBlock)(target));
            
            #line 64 "..\..\..\UserControls\Item.xaml"
            this.Txt.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Txt_MouseEnter);
            
            #line default
            #line hidden
            
            #line 64 "..\..\..\UserControls\Item.xaml"
            this.Txt.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Txt_MouseLeave);
            
            #line default
            #line hidden
            return;
            case 16:
            
            #line 68 "..\..\..\UserControls\Item.xaml"
            ((System.Windows.Controls.MenuItem)(target)).Click += new System.Windows.RoutedEventHandler(this.ReNameMenuItem_Click);
            
            #line default
            #line hidden
            return;
            case 17:
            this.TxtWrite = ((System.Windows.Controls.TextBox)(target));
            
            #line 82 "..\..\..\UserControls\Item.xaml"
            this.TxtWrite.LostFocus += new System.Windows.RoutedEventHandler(this.TxtWrite_LostFocus);
            
            #line default
            #line hidden
            
            #line 83 "..\..\..\UserControls\Item.xaml"
            this.TxtWrite.MouseMove += new System.Windows.Input.MouseEventHandler(this.TxtWrite_MouseMove);
            
            #line default
            #line hidden
            
            #line 83 "..\..\..\UserControls\Item.xaml"
            this.TxtWrite.KeyUp += new System.Windows.Input.KeyEventHandler(this.TxtWrite_KeyUp);
            
            #line default
            #line hidden
            
            #line 83 "..\..\..\UserControls\Item.xaml"
            this.TxtWrite.KeyDown += new System.Windows.Input.KeyEventHandler(this.TxtWrite_KeyDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

