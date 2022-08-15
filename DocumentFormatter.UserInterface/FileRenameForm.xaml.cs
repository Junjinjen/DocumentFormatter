using System;
using System.Globalization;
using System.IO;
using System.Windows;

namespace DocumentFormatter.UserInterface
{
    /// <summary>
    /// Interaction logic for FileRenameForm.xaml
    /// </summary>
    public partial class FileRenameForm : Window
    {
        private const string SourcesFileFormat = "{0}. Джерела. ({1})";
        private const string AbstractFileFormat = "{0}. Конспект. ({1})";
        private const string SubtitlesFileFormat = "{0}. Субтитри. ({1})";
        private const string PictureWithNumberFileFormat = "{0}. Тест {1}. ({2})";
        private const string DateFormatCulture = "uk-UA";

        private readonly string _filename;
        private Func<string> _filenameProvider;

        public FileRenameForm(string filename)
        {
            InitializeComponent();

            _filename = filename;

            PictureFormatUnchecked(null, null);
        }

        private void FileRenameCommand(object sender, RoutedEventArgs e)
        {
            var extension = Path.GetExtension(_filename);
            var path = Path.GetDirectoryName(_filename);
            var formattedFilename = _filenameProvider.Invoke();

            
            var newFilename = Path.Combine(path, $"{formattedFilename}{extension}");

            File.Move(_filename, newFilename);

            Close();
        }

        private void SourcesFormatChecked(object sender, RoutedEventArgs e)
        {
            _filenameProvider = () =>
            {
                var date = GetCurrentDateString();
                var title = TitelTextBox.Text;
                return string.Format(SourcesFileFormat, title, date);
            };
        }

        private void AbstractFormatChecked(object sender, RoutedEventArgs e)
        {
            _filenameProvider = () =>
            {
                var date = GetCurrentDateString();
                var title = TitelTextBox.Text;
                return string.Format(AbstractFileFormat, title, date);
            };
        }

        private void SubtitlesFormatChecked(object sender, RoutedEventArgs e)
        {
            _filenameProvider = () =>
            {
                var date = GetCurrentDateString();
                var title = TitelTextBox.Text;
                return string.Format(SubtitlesFileFormat, title, date);
            };
        }

        private void PictureFormatChecked(object sender, RoutedEventArgs e)
        {
            _filenameProvider = () =>
            {
                var date = GetCurrentDateString();
                var title = TitelTextBox.Text;
                var testNumber = TestNumberTextBox.Text;
                return string.Format(PictureWithNumberFileFormat, title, testNumber, date);
            };

            TestNumberTextBox.IsEnabled = true;
            UpdateValidation(sender, e);
        }

        private void PictureFormatUnchecked(object sender, RoutedEventArgs e)
        {
            TestNumberTextBox.IsEnabled = false;
            TestNumberTextBox.Text = string.Empty;
            UpdateValidation(sender, e);
        }

        private void UpdateValidation(object sender, EventArgs e)
        {
            if (IsTestNumberTextBoxValid() && IsTitelTextBoxValid())
            {
                RenameButton.IsEnabled = true;
                return;
            }

            RenameButton.IsEnabled = false;
        }

        private static string GetCurrentDateString()
        {
            return DateTime.Now.ToString("HHmmss, dddd dd, MMMM yyyy", CultureInfo.CreateSpecificCulture(DateFormatCulture));
        }

        private bool IsTestNumberTextBoxValid()
        {
            return !TestNumberTextBox.IsEnabled
                || (!string.IsNullOrEmpty(TestNumberTextBox.Text) && TestNumberTextBox.Text.IndexOfAny(Path.GetInvalidFileNameChars()) < 0);
        }

        private bool IsTitelTextBoxValid()
        {
            return !string.IsNullOrEmpty(TitelTextBox.Text) && TitelTextBox.Text.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
        }
    }
}
