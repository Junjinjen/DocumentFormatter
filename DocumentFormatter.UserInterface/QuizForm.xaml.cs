using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;

namespace DocumentFormatter.UserInterface
{
    /// <summary>
    /// Interaction logic for QuizForm.xaml
    /// </summary>
    public partial class QuizForm : Window
    {
        public QuizForm()
        {
            InitializeComponent();
            Data.ItemsSource = new List<QuizInfo>();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var list = Data.ItemsSource as List<QuizInfo>;
            var dictionarty = list.Where(x => !string.IsNullOrEmpty(x.TestId))
                .Select(x => new
                {
                    Time = int.Parse(x.Minutes) * 60 + int.Parse(x.Seconds),
                    x.TestId
                }).ToDictionary(x => x.Time, x => x.TestId);

            var json = JsonSerializer.Serialize(dictionarty);

            Clipboard.SetDataObject(json);
        }
    }
}
