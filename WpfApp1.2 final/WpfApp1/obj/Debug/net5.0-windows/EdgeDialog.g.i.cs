﻿#pragma checksum "..\..\..\EdgeDialog.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "CB3285A2D56B31B9042BE6E66B3CABBE207C1A73"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
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
using System.Windows.Controls.Ribbon;
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
using WpfApp1;


namespace WpfApp1 {
    
    
    /// <summary>
    /// EdgeDialog
    /// </summary>
    public partial class EdgeDialog : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 21 "..\..\..\EdgeDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox specifiedLengthCheckBox;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\EdgeDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox equalLengthCheckBox;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\EdgeDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox tangencyCheckBox;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\EdgeDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox perpendicularityCheckBox;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\..\EdgeDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removeRelationButton;
        
        #line default
        #line hidden
        
        
        #line 39 "..\..\..\EdgeDialog.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancelButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WpfApp1;V1.0.0.0;component/edgedialog.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\EdgeDialog.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.specifiedLengthCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 21 "..\..\..\EdgeDialog.xaml"
            this.specifiedLengthCheckBox.Click += new System.Windows.RoutedEventHandler(this.specifiedLengthCheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.equalLengthCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 22 "..\..\..\EdgeDialog.xaml"
            this.equalLengthCheckBox.Click += new System.Windows.RoutedEventHandler(this.equalLengthCheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tangencyCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 23 "..\..\..\EdgeDialog.xaml"
            this.tangencyCheckBox.Click += new System.Windows.RoutedEventHandler(this.tangencyCheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.perpendicularityCheckBox = ((System.Windows.Controls.CheckBox)(target));
            
            #line 24 "..\..\..\EdgeDialog.xaml"
            this.perpendicularityCheckBox.Click += new System.Windows.RoutedEventHandler(this.perpendicularityCheckBox_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.removeRelationButton = ((System.Windows.Controls.Button)(target));
            
            #line 38 "..\..\..\EdgeDialog.xaml"
            this.removeRelationButton.Click += new System.Windows.RoutedEventHandler(this.removeRelationButton_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.cancelButton = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\..\EdgeDialog.xaml"
            this.cancelButton.Click += new System.Windows.RoutedEventHandler(this.cancelButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}
