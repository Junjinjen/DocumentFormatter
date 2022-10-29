using DocumentFormatter.Core;
using DocumentFormatter.Core.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        private const string SymbolCodeReplacements = "SymbolCodeReplacements";
        private const string OpenFileDialogFilter = @"Word File|*.docx;*.doc";
        private const string SettingsFilename = "appsettings.json";
        private readonly IConfigurationRoot _configuration;
        private readonly IDocumentFormatter _documentFormatter;

        public MainWindow()
        {
            InitializeComponent();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(SettingsFilename, optional: false, reloadOnChange: true);

            _configuration = builder.Build();
            _documentFormatter = GetDocumentFormatter();
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

        private IDocumentFormatter GetDocumentFormatter()
        {
            var formatters = GetElementFormatters();
            return new MicrosoftWordFormatter(formatters);
        }

        private List<IElementFormatter> GetElementFormatters()
        {
            var formatters = GetDefaultConstructibleElementFormatters();

            var textReplacements = GetTextReplacements();
            formatters.Add(new TextFormatter(textReplacements));

            var symbolReplacements = GetSymbolReplacements();
            formatters.Add(new SymbolCodeFormatter(symbolReplacements));

            return formatters;
        }

        private List<Replacement> GetTextReplacements()
        {
            return _configuration.GetSection(TextFormatterReplacements).Get<List<Replacement>>();
        }

        private Dictionary<string, string> GetSymbolReplacements()
        {
            return _configuration.GetSection(SymbolCodeReplacements).Get<Dictionary<string, string>>();
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
