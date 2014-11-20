using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace BattleShip
{
    class Network
    {
        public Network()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            MyIP=localIP;
        }

        string MyIP="";
        IPEndPoint EndPoint;
        Socket Connector;

        public string data
        {
            get;
            set;
        }

        public string IP
        {
            get;
            set;
        }
        
        public void Receiver()
        {
            //Создаем Listener на порт "по умолчанию"
            TcpListener Listen = new TcpListener(7000);
            //Начинаем прослушку
            Listen.Start();
            //и заведем заранее сокет
            Socket ReceiveSocket;
            
            while (true)
            {
                try
                {
                    //Пришло сообщение
                    ReceiveSocket = Listen.AcceptSocket();
                    Byte[] Receive = new Byte[256];
                    //Читать сообщение будем в поток
                    using (MemoryStream MessageR = new MemoryStream())
                    {
                        //Количество считанных байт
                        Int32 ReceivedBytes;
                        do
                        {//Собственно читаем
                            ReceivedBytes = ReceiveSocket.Receive(Receive, Receive.Length, 0);
                            //и записываем в поток
                            MessageR.Write(Receive, 0, ReceivedBytes);
                            //Читаем до тех пор, пока в очереди не останется данных
                        } while (ReceiveSocket.Available > 0);
                        //Добавляем изменения в Data or IP
                        if (IPAddress.Parse(Encoding.Default.GetString(MessageR.ToArray())) is IPAddress)
                        {
                            IP=Encoding.Default.GetString(MessageR.ToArray());
                        }
                        data=Encoding.Default.GetString(MessageR.ToArray());
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
        }

        public void ThreadSend(object Message)
        {
            try
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(IP), 7000);
                Connector = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Connector.Connect(EndPoint);
                //Проверяем входной объект на соответствие строке
                String MessageText = "";
                if (Message is String) { MessageText = Message as String; }
                else { throw new Exception("На вход необходимо подавать строку"); }
                Byte[] SendBytes = Encoding.Default.GetBytes(MessageText);
                Connector.Send(SendBytes);
                Connector.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void disconnect()
        {

            //Connector.Close();
        }

        public void Connect()
        {
            try
            {
                EndPoint = new IPEndPoint(IPAddress.Parse(IP), 7000);
                Connector = new Socket(EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                Connector.Connect(EndPoint);
                Byte[] SendBytes = Encoding.Default.GetBytes(MyIP);
                Connector.Send(SendBytes);
                Connector.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
