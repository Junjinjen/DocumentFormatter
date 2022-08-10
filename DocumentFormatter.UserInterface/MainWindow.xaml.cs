using DocumentFormatter.Core;
using DocumentFormatter.Core.Formatters;
using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;

namespace DocumentFormatter.UserInterface
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TextFormatterReplacements = "TextFormatterReplacements";
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

        private void FileRenameCommand(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                CheckFileExists = true,
            };

            var result = openFileDialog.ShowDialog();
            if (result != true)
            {
                return;
            }

            var fileRenameForm = new FileRenameForm(openFileDialog.FileName);
            fileRenameForm.ShowDialog();
        }

        private static IDocumentFormatter GetDocumentFormatter()
        {
            var formatters = GetElementFormatters();
            return new MicrosoftWordFormatter(formatters);
        }

        private static List<IElementFormatter> GetElementFormatters()
        {
            var formatters = GetDefaultConstructibleElementFormatters();

            var replacementDictionary = GetReplacementDictionary();
            formatters.Add(new TextFormatter(replacementDictionary));

            return formatters;
        }

        private static List<IElementFormatter> GetDefaultConstructibleElementFormatters()
        {
            var formatterInterfaceType = typeof(IElementFormatter);
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());
            var query = from type in types
                        where !type.IsInterface && !type.IsAbstract && !type.IsGenericType
                        where formatterInterfaceType.IsAssignableFrom(type)
                        where type.GetConstructor(Type.EmptyTypes) != null
                        select (IElementFormatter)Activator.CreateInstance(type);

            return query.ToList();
        }

        private static Dictionary<string, string> GetReplacementDictionary()
        {
            var table = ConfigurationManager.GetSection(TextFormatterReplacements) as Hashtable;
            return table.Cast<DictionaryEntry>().ToDictionary(x => (string)x.Key, x => (string)x.Value);
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
