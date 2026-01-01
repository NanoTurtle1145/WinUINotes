using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace WinUINotes.Models
{
	public class AllNotes
	{
		public ObservableCollection<Note> Notes { get; set; } =
									new ObservableCollection<Note>();

		public AllNotes()
	{
	}

	public async Task LoadNotes()
		{
			Notes.Clear();
			StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
			await GetFilesInFolderAsync(storageFolder);
		}

	private async Task GetFilesInFolderAsync(StorageFolder folder)
	{
		try
		{
			IReadOnlyList<IStorageItem> storageItems = await folder.GetItemsAsync();
			foreach (IStorageItem item in storageItems)
			{
				if (item.IsOfType(StorageItemTypes.File))
				{
					StorageFile file = (StorageFile)item;
					
					if (!file.Name.StartsWith("notes", StringComparison.OrdinalIgnoreCase))
					{
						System.Diagnostics.Debug.WriteLine($"Skipping non-note file: {file.Name}");
						continue;
					}
					
					Note note = new Note(file.Name)
					{
						Text = await FileIO.ReadTextAsync(file),
						Date = file.DateCreated.DateTime
					};
					Notes.Add(note);
					System.Diagnostics.Debug.WriteLine($"Loaded note: {file.Name}");
				}
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error loading files: {ex.Message}");
		}
	}
	}
}