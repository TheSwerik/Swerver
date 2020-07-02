using System.Windows;
using Swerver.Client;

namespace SwerverTestClient
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Client.Init(new TcpImpl(), new UdpImpl());
        }
    }
}