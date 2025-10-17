using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02_messenger_client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IPEndPoint serverEndPoint;
        string serverIp = "127.0.0.1";
        int serverPort = 4040;
        UdpClient client;
        static ObservableCollection<MessageInfo> messages = new ObservableCollection<MessageInfo>();
        public MainWindow()
        {
            InitializeComponent();
            serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp),serverPort);
            client = new UdpClient();
            
            //list.ItemsSource = messages;    
            this.DataContext = messages;
            //MainWindow mainWindow = new MainWindow();   
           // mainWindow.Show();
            //mainWindow.ShowDialog();
            //this.Hide();

        }

        private  void SendButton_Click(object sender, RoutedEventArgs e)
        {
           
            string msg = msgText.Text;
            SendMessage(msg);
            Listen();
        }

        private  void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            string msg = "$<join>";
            SendMessage(msg);
          
            MessageBox.Show(messages.Count.ToString());
            Listen();
        }
        private async void SendMessage(string msg)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            await client.SendAsync(data, data.Length, serverEndPoint);
        }
        private async Task Listen()
        {
            
            var res =  await client.ReceiveAsync();
            string message = Encoding.UTF8.GetString(res.Buffer);
            messages.Add(new MessageInfo(message, DateTime.Now) );
            //MessageBox.Show(message);
        }
    }
    public class MessageInfo
    {
        public string Message { get; set; }
        public DateTime Time { get; set; }
        public MessageInfo(string M, DateTime T)
        {
            Message = M;
            Time = T;
        }
        public override string ToString()
        {
            return $"{Message}. Time : {Time.ToLocalTime()}";
        }
    }
}