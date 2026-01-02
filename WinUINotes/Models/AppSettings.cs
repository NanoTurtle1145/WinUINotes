using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Storage;

namespace WinUINotes.Models
{
    public class AppSettings : INotifyPropertyChanged
    {
        private static AppSettings? _instance;
        private readonly ApplicationDataContainer _localSettings;

        public static AppSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppSettings();
                }
                return _instance;
            }
        }

        private AppSettings()
        {
            _localSettings = ApplicationData.Current.LocalSettings;
            LoadSettings();
        }

        private bool _isDarkMode;
        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        private bool _autoSave;
        public bool AutoSave
        {
            get => _autoSave;
            set
            {
                if (_autoSave != value)
                {
                    _autoSave = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        private int _autoSaveInterval;
        public int AutoSaveInterval
        {
            get => _autoSaveInterval;
            set
            {
                if (_autoSaveInterval != value)
                {
                    _autoSaveInterval = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        private bool _enableMarkdownPreview;
        public bool EnableMarkdownPreview
        {
            get => _enableMarkdownPreview;
            set
            {
                if (_enableMarkdownPreview != value)
                {
                    _enableMarkdownPreview = value;
                    OnPropertyChanged();
                    SaveSettings();
                }
            }
        }

        private void LoadSettings()
        {
            try
            {
                _isDarkMode = _localSettings.Values.TryGetValue("IsDarkMode", out var darkMode) && darkMode is bool b && b;
                _autoSave = !_localSettings.Values.TryGetValue("AutoSave", out var autoSave) || (autoSave is bool a && a);
                _autoSaveInterval = _localSettings.Values.TryGetValue("AutoSaveInterval", out var interval) && interval is int i ? i : 30;
                _enableMarkdownPreview = _localSettings.Values.TryGetValue("EnableMarkdownPreview", out var markdown) && markdown is bool m && m;
            }
            catch (Exception)
            {
                _isDarkMode = false;
                _autoSave = true;
                _autoSaveInterval = 30;
                _enableMarkdownPreview = false;
            }
        }

        private void SaveSettings()
        {
            _localSettings.Values["IsDarkMode"] = IsDarkMode;
            _localSettings.Values["AutoSave"] = AutoSave;
            _localSettings.Values["AutoSaveInterval"] = AutoSaveInterval;
            _localSettings.Values["EnableMarkdownPreview"] = EnableMarkdownPreview;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
