﻿#pragma checksum "..\..\CommandsInfo.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "077ABF1ED01C72D81F5E60C433B18E065A7703B32921D44B185198AA85D0BFB7"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using BotNet_Server_UI;
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


namespace BotNet_Server_UI {
    
    
    /// <summary>
    /// CommandsInfo
    /// </summary>
    public partial class CommandsInfo : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\CommandsInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel CommandsPanel;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\CommandsInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Command;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\CommandsInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label Arguments;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\CommandsInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock CmdDescription;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\CommandsInfo.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock ArgListBox;
        
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
            System.Uri resourceLocater = new System.Uri("/BotNet Server UI;component/commandsinfo.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CommandsInfo.xaml"
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
            
            #line 8 "..\..\CommandsInfo.xaml"
            ((BotNet_Server_UI.CommandsInfo)(target)).Loaded += new System.Windows.RoutedEventHandler(this.CommInf_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.CommandsPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.Command = ((System.Windows.Controls.Label)(target));
            return;
            case 4:
            this.Arguments = ((System.Windows.Controls.Label)(target));
            return;
            case 5:
            this.CmdDescription = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.ArgListBox = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

