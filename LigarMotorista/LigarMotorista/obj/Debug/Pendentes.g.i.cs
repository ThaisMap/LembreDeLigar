﻿#pragma checksum "..\..\Pendentes.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "C3695181142D845DD01FB7ECE6C57283853DA84EAC145B8EAFCAE6782379B0BC"
//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

using LigarMotorista;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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


namespace LigarMotorista {
    
    
    /// <summary>
    /// Pendentes
    /// </summary>
    public partial class Pendentes : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 9 "..\..\Pendentes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal LigarMotorista.Pendentes Pendente;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\Pendentes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel dock;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\Pendentes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Exportar;
        
        #line default
        #line hidden
        
        
        #line 31 "..\..\Pendentes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FinalizarTudo;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\Pendentes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgLista;
        
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
            System.Uri resourceLocater = new System.Uri("/Lembre de ligar;component/pendentes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\Pendentes.xaml"
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
            this.Pendente = ((LigarMotorista.Pendentes)(target));
            return;
            case 2:
            this.dock = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 3:
            this.Exportar = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\Pendentes.xaml"
            this.Exportar.Click += new System.Windows.RoutedEventHandler(this.Exportar_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.FinalizarTudo = ((System.Windows.Controls.Button)(target));
            
            #line 35 "..\..\Pendentes.xaml"
            this.FinalizarTudo.Click += new System.Windows.RoutedEventHandler(this.FinalizarTudo_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.dgLista = ((System.Windows.Controls.DataGrid)(target));
            
            #line 41 "..\..\Pendentes.xaml"
            this.dgLista.PreviewKeyDown += new System.Windows.Input.KeyEventHandler(this.DgLista_PreviewKeyDown);
            
            #line default
            #line hidden
            
            #line 45 "..\..\Pendentes.xaml"
            this.dgLista.BeginningEdit += new System.EventHandler<System.Windows.Controls.DataGridBeginningEditEventArgs>(this.DgLista_BeginningEdit);
            
            #line default
            #line hidden
            
            #line 46 "..\..\Pendentes.xaml"
            this.dgLista.CellEditEnding += new System.EventHandler<System.Windows.Controls.DataGridCellEditEndingEventArgs>(this.Registrar_Alteracao);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 6:
            
            #line 61 "..\..\Pendentes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Finalizar);
            
            #line default
            #line hidden
            break;
            case 7:
            
            #line 70 "..\..\Pendentes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.NovaDiaria);
            
            #line default
            #line hidden
            break;
            case 8:
            
            #line 90 "..\..\Pendentes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Registrar_Ligacao);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
