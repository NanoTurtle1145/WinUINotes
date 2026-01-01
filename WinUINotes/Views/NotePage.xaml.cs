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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUINotes.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NotePage : Page
    {
		private Note? noteModel;
		public NotePage()
        {
            this.InitializeComponent();
		
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

        private void EnableMarkdown_Checked(object sender, RoutedEventArgs e)
        {
			PreviewPanel.Visibility = Visibility.Visible;
			UpdateMarkdownPreview();
		}

        private void EnableMarkdown_Unchecked(object sender, RoutedEventArgs e)
        {
			PreviewPanel.Visibility = Visibility.Collapsed;
		}

        private void NoteEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
			if (EnableMarkdown.IsChecked == true)
			{
				UpdateMarkdownPreview();
			}
		}

        private async void UpdateMarkdownPreview()
        {
			if (noteModel is null) return;

			var markdownText = noteModel.Text ?? string.Empty;
			var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
			var html = Markdown.ToHtml(markdownText, pipeline);

			var htmlTemplate = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <style>
        body {{
            font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, sans-serif;
            line-height: 1.6;
            margin: 0;
            padding: 16px;
            color: #333;
            background: transparent;
        }}
        h1, h2, h3, h4, h5, h6 {{
            margin-top: 24px;
            margin-bottom: 16px;
            font-weight: 600;
        }}
        h1 {{ font-size: 2em; border-bottom: 1px solid #eaecef; padding-bottom: 0.3em; }}
        h2 {{ font-size: 1.5em; border-bottom: 1px solid #eaecef; padding-bottom: 0.3em; }}
        h3 {{ font-size: 1.25em; }}
        code {{
            background: #f6f8fa;
            padding: 0.2em 0.4em;
            border-radius: 3px;
            font-family: 'Courier New', monospace;
            font-size: 0.9em;
        }}
        pre {{
            background: #f6f8fa;
            padding: 16px;
            border-radius: 6px;
            overflow-x: auto;
        }}
        pre code {{
            background: transparent;
            padding: 0;
        }}
        blockquote {{
            border-left: 4px solid #dfe2e5;
            padding-left: 16px;
            color: #6a737d;
            margin: 0;
        }}
        ul, ol {{
            padding-left: 2em;
        }}
        a {{
            color: #0366d6;
            text-decoration: none;
        }}
        a:hover {{
            text-decoration: underline;
        }}
        table {{
            border-collapse: collapse;
            width: 100%;
            margin: 16px 0;
        }}
        th, td {{
            border: 1px solid #dfe2e5;
            padding: 8px 12px;
            text-align: left;
        }}
        th {{
            background: #f6f8fa;
            font-weight: 600;
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
	}
}
