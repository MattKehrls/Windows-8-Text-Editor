using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//!!!TO USE THE COLOUR PICKER YOU MUST USE THE USING BELOW/ AND DOWNLOAD NUGETPACKAGE!!!
using Coding4Fun.Toolkit.Controls;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InClassApp_2_
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            //Setting the size of the application at OnLaunch
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.PreferredLaunchViewSize = new Size(520,515);
            //Set the Title of the App(top left corner)
            ApplicationView appView = ApplicationView.GetForCurrentView();
            appView.Title = "TextEditor";
            


        }

        //Couldn't get this button to clear the text for a new document in the richeditbox.
        private void new_btn_Click(object sender, RoutedEventArgs e)
        {
            
            //editBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, null);
        }

        private async void open_btn_Click(object sender, RoutedEventArgs e)
        {
            // Open a text file.
            Windows.Storage.Pickers.FileOpenPicker open = new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            open.FileTypeFilter.Add(".rtf");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    Windows.Storage.Streams.IRandomAccessStream randAccStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    // Load the file into the Document property of the RichEditBox.
                    editBox.Document.LoadFromStream(Windows.UI.Text.TextSetOptions.FormatRtf, randAccStream);
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }

        }

        private async void save_btn_Click(object sender, RoutedEventArgs e)
        {
            string document;
            //Get the text and put it in the "document" var, can then use it as a string of text!
            editBox.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out document);
            if (!string.IsNullOrEmpty(document))
            {

                Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;

                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("Rich Text", new List<string>() { ".rtf" });

                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "New Document";

                Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
                if (file != null)
                {
                    // Prevent updates to the remote version of the file until we
                    // finish making changes and call CompleteUpdatesAsync.
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    // write to file
                    Windows.Storage.Streams.IRandomAccessStream randAccStream =
                        await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);

                    editBox.Document.SaveToStream(Windows.UI.Text.TextGetOptions.FormatRtf, randAccStream);

                    // Let Windows know that we're finished changing the file so the
                    // other app can update the remote version of the file.
                    Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                    if (status != Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        Windows.UI.Popups.MessageDialog errorBox =
                            new Windows.UI.Popups.MessageDialog("File " + file.Name + " couldn't be saved.");
                        await errorBox.ShowAsync();
                    }
                }
            } else
            {
                Windows.UI.Popups.MessageDialog error = new Windows.UI.Popups.MessageDialog("There is nothing to save!!!");
                await error.ShowAsync();
            }
        }
      
        //BOLD Button
        private void boldbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Bold = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }
        //Italic Button
        private void italicbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.Italic = Windows.UI.Text.FormatEffect.Toggle;
                selectedText.CharacterFormat = charFormatting;
            }
        }
        //UnderLine Button
        private void underlinebtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.Underline == Windows.UI.Text.UnderlineType.None)
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.Single;
                }
                else
                {
                    charFormatting.Underline = Windows.UI.Text.UnderlineType.None;
                }
                selectedText.CharacterFormat = charFormatting;
            }
        }
        //Left Align Button
        private void leftbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextParagraphFormat charFormatting = selectedText.ParagraphFormat;
                if (charFormatting.Alignment != Windows.UI.Text.ParagraphAlignment.Left)
                {
                    charFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Left;
                }
                else
                {
                    //charFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
                }
                selectedText.ParagraphFormat = charFormatting;
            }
        }
        //Center Align Button
        private void centerbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextParagraphFormat charFormatting = selectedText.ParagraphFormat;
                if (charFormatting.Alignment != Windows.UI.Text.ParagraphAlignment.Center)
                {
                    charFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Center;
                }
                else
                {
                    //charFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
                }
                selectedText.ParagraphFormat = charFormatting;
            }
        }
        //Right Align Button
        private void rightbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextParagraphFormat charFormatting = selectedText.ParagraphFormat;
                if (charFormatting.Alignment != Windows.UI.Text.ParagraphAlignment.Right)
                {
                    charFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
                }
                else
                {
                    //charFormatting.Alignment = Windows.UI.Text.ParagraphAlignment.Right;
                }
                selectedText.ParagraphFormat = charFormatting;
            }
        }
        //ALLCAPS Button
        private void allCapsbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                if (charFormatting.AllCaps == Windows.UI.Text.FormatEffect.Off)
                {
                    charFormatting.AllCaps = Windows.UI.Text.FormatEffect.On;
                }
                else
                {
                    charFormatting.AllCaps = Windows.UI.Text.FormatEffect.Off;
                }
                selectedText.CharacterFormat = charFormatting;
              
            }
        }
        //Color Picker Colour OnColour Changed Method
        private void ColorPicker_ColorChanged(object sender, Windows.UI.Color color)
        {
            Windows.UI.Text.ITextSelection selectedText = editBox.Document.Selection;
            if (selectedText != null)
            {
                Windows.UI.Text.ITextCharacterFormat charFormatting = selectedText.CharacterFormat;
                charFormatting.ForegroundColor = color;
            }
                
        }
        //Open and Close SplitView(ColourPicker) Button
        private void colorpickerbtn_Click(object sender, RoutedEventArgs e)
        {
            colorpickerSplitView.IsPaneOpen = !colorpickerSplitView.IsPaneOpen;
            
        }

       //Method that counts the words in the RichTextBox
        private void editBox_TextChanged(object sender, RoutedEventArgs e)
        {
            string document;
            int count = 0;
            string[] finaldoc;
            editBox.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out document);
            if (!String.IsNullOrEmpty(document))
            {
                finaldoc = document.Trim().Split(' ', '.', '?', '!', '\r');
                foreach (var item in finaldoc)
                {
                    count++;
                }

            }
            else
            {
                count = 0;
            }
            displayCount(count);
        }
       
        //Displays the word count at the bottom in a textBlock 
       public void displayCount(int Count)
        {
            string stringcount = Count.ToString(); ;
            TesttextBlock.Text = stringcount;
        }

        //Counts words from input into search box
        public void countwords()
        {
            string document;
            int matchcount = 0;
            string[] words;
            string keyword = SearchBox.Text.Trim().ToLower();
            editBox.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out document);
            if (!String.IsNullOrEmpty(document)) { 
               words = document.Trim().ToLower().Split(' ', '.', '!', '?', '"', '\'', '\n', '\r'); 

            foreach (var item in words)
            {
                if (item == keyword)
                {
                   matchcount++;
                }

            }
            }
            displaySearchCount(matchcount);


        }

        //Displays SearchBox Results found
        public void displaySearchCount(int Count)
        {
            string stringcount = Count.ToString(); ;
            SearchResultsBlock.Text = stringcount;
        }

        //What happens when you start typing into the SearchBox
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchBox.Text == "")
            {
                searchtxtBlock.Visibility = Visibility.Collapsed;
                SearchResultsBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                searchtxtBlock.Visibility = Visibility.Visible;
                SearchResultsBlock.Visibility = Visibility.Visible;
            }
            
            countwords();
        }

        //Button to add an Image to the document
        private async void  imgbtn_Click(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileOpenPicker open = new Windows.Storage.Pickers.FileOpenPicker();
            open.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            open.FileTypeFilter.Add(".jpg");
            open.FileTypeFilter.Add(".png");

            Windows.Storage.StorageFile file = await open.PickSingleFileAsync();

            if (file != null)
            {
                try
                {
                    using (IRandomAccessStream imagestram = await file.OpenReadAsync())
                    {
                        BitmapImage image = new BitmapImage();
                        await image.SetSourceAsync(imagestram);
                        editBox.Document.Selection.InsertImage(image.PixelWidth, image.PixelHeight, 0, Windows.UI.Text.VerticalCharacterAlignment.Baseline, "Image", imagestram);
                    }
                                    
                }
                catch (Exception)
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "File open error",
                        Content = "Sorry, I couldn't open the file.",
                        PrimaryButtonText = "Ok"
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }
    }
}



