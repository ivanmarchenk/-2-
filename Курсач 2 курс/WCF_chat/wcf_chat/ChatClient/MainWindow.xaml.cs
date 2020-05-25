using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ChatClient
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ServiceChat.IServerChatCallback
        //реализация отправки ответов со стороны сервера
    {

        bool isConnected = false;
        //эта переменная отвечает за то, подключен ли клиент на данный момент

        ServiceChat.ServiceChatClient client;
        //объект нашего хоста, чтобы можно было взаимодействовать с ним (хостом) в клиенте

        int ID;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        //метод для инициализации после загрузки основного окна
        {
            
        }


        void ConnectUser() 
        {
            if (!isConnected)
            {
                client = new ServiceChat.ServiceChatClient(new System.ServiceModel.InstanceContext(this));
                ID = client.Connect(tbUserName.Text);
                //подключение юзера с указанным ID
                tbUserName.IsEnabled = false;
                //блокировка возможности изменить имя юзера 
                Button_Connect_Disconnect.Content = "Disconnect"; 
                //что будет написано на кнопке, когда пользователь подключился
                //при нажатии на эту кнопку юзер будет отключен
                isConnected = true;
            }
        }

        void DisconnectUser() 
        {
            if (isConnected) 
            {
                client.Disconnect(ID);
                //отключение юзера с указанным ID
                client = null;
                //после отключения сбрасываем ID отключенного пользователя
                tbUserName.IsEnabled = true;
                //разблокировка возможности изменить имя юзера
                Button_Connect_Disconnect.Content = "Connect";
                //что будет написано на кнопке, если пользователь отключен
                //при нажатии на кнопку пользователь будет подключен
                isConnected = false;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) 
        //подключение-отключение юзера при нажатии на кнопку
        {
            if (isConnected)
            {
                DisconnectUser();
            }
            else 
            {
                ConnectUser();
            }
        }

        public void MessageCallback(string msg)
        {
            lbChat.Items.Add(msg);
            //метод добавления нового сообщения в чат
            lbChat.ScrollIntoView(lbChat.Items[lbChat.Items.Count-1]);
            //этот метод автоматически прокручивает полосу прокрутки до последней записи в чате
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        //когда юзер закрыл окно программы сервер автоматически его отключит
        {
            DisconnectUser();
        }

        private void tbMessage_KeyDown(object sender, KeyEventArgs e)
        //обработка нажатия клавиш при вводе сообщения польщователем и отправке
        {
            if (e.Key == Key.Enter)
            //если пользователь нажал Enter
            {
                if (client!=null) 
                //проверяем что пользователь законнектился, если да, то он может отправлять сообщения
                {
                    client.SendMessage(tbMessage.Text, ID);
                    //передаем сообщение, написанное в textBox и id пользователя
                    tbMessage.Text = string.Empty;
                    //помещаем в textBox пустую строку
                }
            }
        }

    }
}
