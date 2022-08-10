using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

        private readonly string _filename;
        private string _selectedFormat;

        public FileRenameForm(string filename)
        {
            InitializeComponent();

            _filename = filename;
            ValidateState(null, null);
            TestNumberTextBox.IsEnabled = false;
        }

        private void FileRenameCommand(object sender, RoutedEventArgs e)
        {
            var date = DateTime.Now.ToString("HHmmss, dddd dd, MMMM yyyy", CultureInfo.CreateSpecificCulture("uk-UA"));

            string newFilename;
            if (TestNumberTextBox.IsEnabled)
            {
                newFilename = string.Format(_selectedFormat, TitelTextBox.Text, TestNumberTextBox.Text, date);
            }
            else
            {
                newFilename = string.Format(_selectedFormat, TitelTextBox.Text, date);
            }

            var extension = Path.GetExtension(_filename);
            var path = Path.GetDirectoryName(_filename);
            var file = Path.Combine(path, $"{newFilename}{extension}");

            File.Move(_filename, file);

            Close();
        }

        private void SourcesFormatChecked(object sender, RoutedEventArgs e)
        {
            _selectedFormat = SourcesFileFormat;
        }

        private void AbstractFormatChecked(object sender, RoutedEventArgs e)
        {
            _selectedFormat = AbstractFileFormat;
        }

        private void SubtitlesFormatChecked(object sender, RoutedEventArgs e)
        {
            _selectedFormat = SubtitlesFileFormat;
        }

        private void PictureFormatChecked(object sender, RoutedEventArgs e)
        {
            _selectedFormat = PictureWithNumberFileFormat;
            TestNumberTextBox.IsEnabled = true;
            ValidateState(null, null);
        }

        private void PictureFormatUnchecked(object sender, RoutedEventArgs e)
        {
            TestNumberTextBox.IsEnabled = false;
            TestNumberTextBox.Text = string.Empty;
            ValidateState(null, null);
        }

        private void ValidateState(object sender, TextChangedEventArgs e)
        {
            if (IsTestNumberTextBoxValid() && IsTitelTextBoxValid())
            {
                RenameButton.IsEnabled = true;
                return;
            }

            RenameButton.IsEnabled = false;
        }

        private bool IsTestNumberTextBoxValid()
        {
            return !TestNumberTextBox.IsEnabled
                || (!string.IsNullOrEmpty(TestNumberTextBox.Text) && TestNumberTextBox.Text.IndexOfAny(Path.GetInvalidPathChars()) < 0);
        }

        private bool IsTitelTextBoxValid()
        {
            return !string.IsNullOrEmpty(TitelTextBox.Text) && TitelTextBox.Text.IndexOfAny(Path.GetInvalidPathChars()) < 0;
        }
    }
}
