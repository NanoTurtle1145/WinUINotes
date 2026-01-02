using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUINotes.Models;

namespace WinUINotes.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            VersionTextBlock.Text = $"版本 {Constants.Version}";
            
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
            RequestedTheme = theme;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            AppSettings.Instance.PropertyChanged -= OnSettingsChanged;
        }
    }
}
