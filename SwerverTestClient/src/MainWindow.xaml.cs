using System.ComponentModel;
using System.Windows;
using Swerver.Client;
using Swerver.Util;

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
            Constants.Port = 17777;
            Client.Init(new TcpImpl(), new UdpImpl());
            Client.Instance.ConnectToServer();
            Client.PacketHandlers.Add((int) PacketEnum.Lol, SendAndHandel.ReceiveLol);
        }

        private void Window_OnClosing(object sender, CancelEventArgs e) { Client.Instance.Disconnect(); }
        private void Button_Disconnect_OnClick(object sender, RoutedEventArgs e) { Client.Instance.Disconnect(); }
        private void Button_Connect_OnClick(object sender, RoutedEventArgs e) { Client.Instance.ConnectToServer(); }
        private void Button_lol_OnClick(object sender, RoutedEventArgs e) { SendAndHandel.SendLol(); }
    }
}