using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUINotes.Models;

namespace WinUINotes
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.InitializeComponent();
            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            
            ApplyTheme();
            AppSettings.Instance.PropertyChanged += OnSettingsChanged;
        }

        private void OnSettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AppSettings.IsDarkMode))
            {
                ApplyTheme();
            }
        }

        private void ApplyTheme()
        {
            var theme = AppSettings.Instance.IsDarkMode ? ElementTheme.Dark : ElementTheme.Light;
            if (Content is FrameworkElement rootElement)
            {
                rootElement.RequestedTheme = theme;
            }
        }

        private void AppTitleBar_BackRequested(TitleBar sender, object args)
        {
            if (rootFrame.CanGoBack == true)
            {
                rootFrame.GoBack();
            }
        }
    }
}
