using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace _3
{
    class Server
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private UdpClient udpClient;
        private int port;
        private List<Train> timetable = new List<Train>();
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static List<string> saveLines = new List<string>();
        private static int countSaveLines;

        public Server(int port)
        {
            udpClient = new UdpClient(port);
            this.port = port;

            ReadFile(ref timetable);
        }

        public void StartListenAsync()
        {
            while (true)
            {
                allDone.Reset();
                udpClient.BeginReceive(RequestCallback, udpClient);
                allDone.WaitOne();
            }
        }

        private void RequestCallback(IAsyncResult ar)
        {
            allDone.Set();
            var listener = (UdpClient)ar.AsyncState;
            IPEndPoint ep = new IPEndPoint(IPAddress.Any, port);

            byte[] recBytes = listener.EndReceive(ar, ref ep);
            string recData = Encoding.UTF8.GetString(recBytes);
            string[] splitData = recData.Split(new Char[] { '_' });
            byte[] sendByte;

            UdpClient sender = new UdpClient();

            string sendData = string.Empty;
            int q = 0;

            switch (splitData[0])
            {
                case "Кол-во записей":
                    Console.WriteLine("Пользователь отправил запрос: " + recData.ToString());
                    logger.Info("Пользователь отправил запрос: " + recData.ToString());

                    sendData = timetable.Count.ToString();
                    sendByte = Encoding.UTF8.GetBytes(sendData);
                    sender.Send(sendByte, sendByte.Length, ep);
                    break;
                case "Запись":
                    Console.WriteLine("Пользователь отправил запрос: " + recData.ToString());
                    logger.Info("Пользователь отправил запрос: " + recData.ToString());

                    q = int.Parse(splitData[1]);
                    sendData = timetable[q].startingPoint + ',' + timetable[q].destination + ',' + timetable[q].carriageType + ',' + timetable[q].travelTime.ToString() + ',' + timetable[q].delay.ToString();
                    sendByte = Encoding.UTF8.GetBytes(sendData);
                    sender.Send(sendByte, sendByte.Length, ep);
                    break;
                case "Сохранить кол-во":
                    Console.WriteLine("Пользователь отправил запрос: " + recData.ToString());
                    logger.Info("Пользователь отправил запрос: " + recData.ToString());

                    countSaveLines = int.Parse(splitData[1]);
                    sendData = "Успех";
                    sendByte = Encoding.UTF8.GetBytes(sendData);
                    sender.Send(sendByte, sendByte.Length, ep);
                    break;
                case "Сохранить":
                    Console.WriteLine("Пользователь отправил запрос: " + recData.ToString());
                    logger.Info("Пользователь отправил запрос: " + recData.ToString());

                    q = int.Parse(splitData[1]);
                    saveLines.Add(splitData[2]);
                    if (q == (countSaveLines - 1))
                    {
                        using (var writer = new StreamWriter("222.csv"))
                        {
                            for (int i = 0; i < countSaveLines; i++)
                            {
                                writer.WriteLine(saveLines[i], Encoding.UTF8);
                            }
                            Console.WriteLine("Сервер: все данные сохранены");
                            logger.Info("Сервер: все данные сохранены");
                        }
                    }

                    sendData = "Успех";
                    sendByte = Encoding.UTF8.GetBytes(sendData);
                    sender.Send(sendByte, sendByte.Length, ep);
                    break;
            }
        }

        static void ReadFile(ref List<Train> timetable)
        {
            string line;
            using (var reader = new StreamReader("111.csv", Encoding.Default))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] split = line.Split(new Char[] { ',' });
                    try
                    {
                        timetable.Add(new Train(split[0], split[1], split[2], int.Parse(split[3]), bool.Parse(split[4])));
                    }
                    catch (FormatException)
                    {
                        timetable.Add(new Train(split[0], split[1], split[2], 0, true));
                    };
                }
            }
        }
    }
}
