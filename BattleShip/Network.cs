using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BattleShip
{
    class Network
    {
        public Network()
        {
            IPHostEntry host;
            string localIp = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIp = ip.ToString();
                    break;
                }
            }
            MyIP=localIp;
        }

        string MyIP="";
        IPEndPoint _endPoint;
        Socket _connector;

        public string Data
        {
            get;
            set;
        }

        public string Ip
        {
            get;
            set;
        }
        
        public void Receiver()
        {
            //Создаем Listener на порт "по умолчанию"
            TcpListener listen = new TcpListener(7000);
            //Начинаем прослушку
            listen.Start();
            //и заведем заранее сокет
            Socket ReceiveSocket;
            
            while (true)
            {
                try
                {
                    //Пришло сообщение
                    ReceiveSocket = listen.AcceptSocket();
                    Byte[] receive = new Byte[256];
                    //Читать сообщение будем в поток
                    using (MemoryStream messageR = new MemoryStream())
                    {
                        //Количество считанных байт
                        Int32 ReceivedBytes;
                        do
                        {//Собственно читаем
                            ReceivedBytes = ReceiveSocket.Receive(receive, receive.Length, 0);
                            //и записываем в поток
                            messageR.Write(receive, 0, ReceivedBytes);
                            //Читаем до тех пор, пока в очереди не останется данных
                        } while (ReceiveSocket.Available > 0);
                        //Добавляем изменения в Data or IP
                        if (IPAddress.Parse(Encoding.Default.GetString(messageR.ToArray())) is IPAddress)
                        {
                            Ip=Encoding.Default.GetString(messageR.ToArray());
                        }
                        Data=Encoding.Default.GetString(messageR.ToArray());
                        MainForm.X = Int32.Parse(Data.Substring(0, 1));
                        MainForm.Y = Int32.Parse(Data.Substring(1, 1));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        public void ThreadSend(object message)
        {
            try
            {
                _endPoint = new IPEndPoint(IPAddress.Parse(Ip), 7000);
                _connector = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _connector.Connect(_endPoint);
                //Проверяем входной объект на соответствие строке
                String messageText;
                if (message is String) { messageText = message as String; }
                else { throw new Exception("На вход необходимо подавать строку"); }
                Byte[] sendBytes = Encoding.Default.GetBytes(messageText);
                _connector.Send(sendBytes);
                _connector.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Disconnect()
        {

            _connector.Close();
        }

        public void Connect()
        {
            try
            {
                _endPoint = new IPEndPoint(IPAddress.Parse(Ip), 7000);
                _connector = new Socket(_endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _connector.Connect(_endPoint);
                Byte[] sendBytes = Encoding.Default.GetBytes(MyIP);
                _connector.Send(sendBytes);
                _connector.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
