using System.Windows;
using System.Windows.Controls;

namespace ServerTest
{
    public partial class MainWindow : Window
    {
        public static string Username;

        public MainWindow()
        {
            InitializeComponent();
            Client.Init();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e) { Client.Instance.ConnectToServer(); }

        private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e) { Username = UsernameBox.Text; }
    }
}