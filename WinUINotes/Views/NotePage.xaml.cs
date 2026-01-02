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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using WinUINotes.Models;
using Markdig;

namespace WinUINotes.Views
{
    public sealed partial class NotePage : Page
	{
		public Note? noteModel;
		public NotePage()
        {
            this.InitializeComponent();
			noteModel = new Note("temp.txt");
			NoteEditor.TextChanged += NoteEditor_TextChanged;
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter is Note note)
			{
				noteModel = note;
			}
			else
			{
				noteModel = new Note("notes" + DateTime.Now.ToBinary().ToString() + ".txt");
			}

			NoteEditor.Header = noteModel.Date.ToString();
			
			AppSettings.Instance.PropertyChanged += OnSettingsChanged;
			ApplySettings();
		}

		private void OnSettingsChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(AppSettings.IsDarkMode))
			{
				ApplyTheme();
				if (AppSettings.Instance.EnableMarkdownPreview)
				{
					UpdateMarkdownPreview();
				}
			}
			else if (e.PropertyName == nameof(AppSettings.EnableMarkdownPreview))
			{
				if (AppSettings.Instance.EnableMarkdownPreview)
				{
					UpdateMarkdownPreview();
				}
			}
		}

		private void ApplySettings()
		{
			ApplyTheme();
		}

		private void ApplyTheme()
		{
			var theme = AppSettings.Instance.IsDarkMode ? ElementTheme.Dark : ElementTheme.Light;
			RequestedTheme = theme;
		}

		private void NoteEditor_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (AppSettings.Instance.EnableMarkdownPreview)
			{
				UpdateMarkdownPreview();
			}
		}

		private async void UpdateMarkdownPreview()
		{
			if (noteModel is null || string.IsNullOrEmpty(noteModel.Text))
			{
				await MarkdownPreview.EnsureCoreWebView2Async();
				await MarkdownPreview.ExecuteScriptAsync("document.body.innerHTML = ''");
				return;
			}

			var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
			var html = Markdown.ToHtml(noteModel.Text, pipeline);

			var isDarkMode = AppSettings.Instance.IsDarkMode;
			var textColor = isDarkMode ? "#ffffff" : "#000000";
			var codeBgColor = isDarkMode ? "#3a3a3a" : "#f5f5f5";
			var preBgColor = isDarkMode ? "#2d2d2d" : "#f5f5f5";
			var blockquoteColor = isDarkMode ? "#cccccc" : "#666666";
			var tableBorderColor = isDarkMode ? "#444444" : "#dddddd";
			var thBgColor = isDarkMode ? "#3a3a3a" : "#f0f0f0";

			var htmlTemplate = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            margin: 16px;
            line-height: 1.6;
            color: {textColor};
            background-color: transparent;
        }}
        h1, h2, h3, h4, h5, h6 {{
            margin-top: 24px;
            margin-bottom: 16px;
            font-weight: 600;
        }}
        code {{
            background-color: {codeBgColor};
            padding: 2px 6px;
            border-radius: 3px;
            font-family: 'Consolas', 'Monaco', monospace;
        }}
        pre {{
            background-color: {preBgColor};
            padding: 16px;
            border-radius: 6px;
            overflow-x: auto;
        }}
        pre code {{
            background-color: transparent;
            padding: 0;
        }}
        blockquote {{
            border-left: 4px solid #0078d4;
            padding-left: 16px;
            margin-left: 0;
            color: {blockquoteColor};
        }}
        table {{
            border-collapse: collapse;
            width: 100%;
            margin: 16px 0;
        }}
        th, td {{
            border: 1px solid {tableBorderColor};
            padding: 8px 12px;
            text-align: left;
        }}
        th {{
            background-color: {thBgColor};
        }}
        a {{
            color: #0078d4;
            text-decoration: none;
        }}
        a:hover {{
            text-decoration: underline;
        }}
        img {{
            max-width: 100%;
            height: auto;
        }}
    </style>
</head>
<body>
    {html}
</body>
</html>";

			await MarkdownPreview.EnsureCoreWebView2Async();
			MarkdownPreview.NavigateToString(htmlTemplate);
		}

		private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
			if (noteModel is not null)
			{
				await noteModel.SaveAsync();
			}
			await Task.Delay(100);
			if (Frame.CanGoBack == true)
			{
				Frame.GoBack();
			}
		}

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
			if (noteModel is not null)
			{
				await noteModel.DeleteAsync();
			}
			await Task.Delay(100);
			if (Frame.CanGoBack == true)
			{
				Frame.GoBack();
			}
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			AppSettings.Instance.PropertyChanged -= OnSettingsChanged;
		}
	}
}
