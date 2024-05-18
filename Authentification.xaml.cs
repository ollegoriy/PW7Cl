using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace PW7Cl
{
    public partial class Authentification : Window
    {
        private Socket socket;

        public Authentification()
        {
            InitializeComponent();
        }

        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string ip = IPTextBox.Text;
            string nickname = NicknameTextBox.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Пожалуйста, введите логин.");
                return;
            }

            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Пожалуйста, введите IP сервера.");
                return;
            }

            if (string.IsNullOrWhiteSpace(nickname))
            {
                MessageBox.Show("Пожалуйста, введите имя пользователя.");
                return;
            }

            bool isConnected = ConnectToServer(ip, nickname);

            if (isConnected)
            {
                Messenger mainWindow = new Messenger(socket, login, ip, nickname);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не удалось подключиться к серверу.");
            }
        }

        private bool ConnectToServer(string ip, string nickname)
        {
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ip, 7777);
                SendUser(new User(nickname));
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось подключиться к серверу: {ex.Message}");
                return false;
            }
        }

        private async void SendUser(User user)
        {
            try
            {
                string json = JsonConvert.SerializeObject(user);
                var bytes = Encoding.UTF8.GetBytes(json);
                await socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось отправить данные: {ex.Message}");
            }
        }
    }
}
