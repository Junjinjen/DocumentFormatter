using DocumentFormatter.Core;
using DocumentFormatter.Core.Formatters;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DocumentFormatter.UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string OpenFileDialogFilter = @"Word File|*.docx;*.doc";

        private readonly IDocumentFormatter _documentFormatter = GetDocumentFormatter();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileCommand(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = OpenFileDialogFilter,
                CheckFileExists = true,
            };

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        private static IDocumentFormatter GetDocumentFormatter()
        {
            var formatters = GetElementFormatters();
            return new MicrosoftWordFormatter(formatters);
        }

        private static List<IElementFormatter> GetElementFormatters()
        {
            return new List<IElementFormatter>
            {
                new ParagraphFormatter(),
                new EquationFormatter(),
                new FractionFormatter(),
                new ExponentFormatter(),
                new RadicalFormatter(),
                new TextFormatter(new Dictionary<string, string>
                {
                    { @"⋅", @"\cdot" },
                }),
            };
        }

        private void LoadFile(string filename)
        {
            using var memoryStream = new MemoryStream();
            _documentFormatter.Format(filename, memoryStream);

            memoryStream.Seek(0, SeekOrigin.Begin);
            using var streamReader = new StreamReader(memoryStream);
            OutputTextBox.Text = streamReader.ReadToEnd();
        }
    }
}
