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
using WinUINotes.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUINotes.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AllNotesPage : Page
	{
		private AllNotes notesModel = new AllNotes();
		private bool isFirstLoad = true;

		public AllNotesPage()
		{
			this.InitializeComponent();
			MainNav.ItemInvoked += MainNav_ItemInvoked;
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (isFirstLoad)
			{
				await notesModel.LoadNotes();
				isFirstLoad = false;
			}
		}

        private void NewNoteButton_Click(object sender, RoutedEventArgs e)
        {
			Frame.Navigate(typeof(NotePage));
		}
		private void ItemsView_ItemInvoked(ItemsView sender, ItemsViewItemInvokedEventArgs args)
		{
			if (args.InvokedItem is Note note)
			{
				Frame.Navigate(typeof(NotePage), note);
			}
		}

        private async Task AppBarButton_ClickAsync(object sender, RoutedEventArgs e)
        {
			
		}

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
		{
			AboutDialog aboutDialog = new AboutDialog()
			{
				XamlRoot = MainNav.XamlRoot
			};
			await aboutDialog.ShowAsync();
		}

        private void MainNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
		{
			if (args.InvokedItemContainer is NavigationViewItem item)
			{
				if (item.Content?.ToString() == "设置")
				{
					Frame.Navigate(typeof(SettingsPage));
				}
			}
		}
	}
}
