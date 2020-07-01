using System.Windows;

namespace ServerTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Client.Init();
        }

        private void Button_OnClick(object sender, RoutedEventArgs e) { Client.Instance.ConnectToServer(); }
    }
}