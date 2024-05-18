using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PW7Cl
{
    public partial class Messenger : Window
    {
        private Socket socket;
        private ObservableCollection<Chat> chatMessages = new ObservableCollection<Chat>();

        private string login;
        private string ip;
        private string nickname;

        public Messenger(Socket socket, string login, string ip, string nickname)
        {
            InitializeComponent();
            this.socket = socket;
            this.login = login;
            this.ip = ip;
            this.nickname = nickname;
            Main_Lbx.ItemsSource = chatMessages;

            ListenMess();
        }

        private async void SendMessenge(string message)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось получить сообщение: {ex.Message}");
            }
        }

        private void But_Click(object sender, RoutedEventArgs e)
        {
            SendMessenge(Mess_Tbx.Text);
            Mess_Tbx.Clear();
        }

        private void Mess_Tbx_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                But_Click(But, new RoutedEventArgs());
            }
        }

        private async void ListenMess()
        {
            try
            {
                while (true)
                {
                    byte[] bytes = new byte[1024];
                    int bytesRead = await socket.ReceiveAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
                    if (bytesRead > 0)
                    {
                        string jsonString = Encoding.UTF8.GetString(bytes, 0, bytesRead);
                        Chat chatMessage = JsonConvert.DeserializeObject<Chat>(jsonString);
                        chatMessages.Add(chatMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Соединение разоравно: {ex.Message}");
            }
        }
    }
}