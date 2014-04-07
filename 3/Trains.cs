using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace _3
{
    class Trains
    {
        public List<Train> trains { get; private set; }
        UdpClient udpClient = new UdpClient();

        string data;
        string recData;
        
        public void LoadData()
        {
            trains = new List<Train>();

            data = "Кол-во записей";
            recData = Connect(ref udpClient, ref data);
            int timetableCount = int.Parse(recData);

            for (int i = 0; i < timetableCount; i++)
            {
                data = "Запись_" + (i).ToString();
                recData = Connect(ref udpClient, ref data);
                string[] split = recData.Split(new Char[] { ',' });
                try
                {
                    trains.Add(new Train(split[0], split[1], split[2], int.Parse(split[3]), bool.Parse(split[4])));
                }
                catch (FormatException)
                {
                    trains.Add(new Train(split[0], split[1], split[2], 0, true));
                };
            }
        }

        public void SaveData(List<Train> moreTrains)
        {
            data = "Сохранить кол-во_" + moreTrains.Count.ToString();
            Connect(ref udpClient, ref data);

            string line;

            for (int i = 0; i < moreTrains.Count; i++)
            {
                line = moreTrains[i].startingPoint + ',' + moreTrains[i].destination + ',' +
                            moreTrains[i].carriageType + ',' + moreTrains[i].travelTime.ToString() + ',' +
                            moreTrains[i].delay.ToString();
                data = "Сохранить_" + (i).ToString() + '_' + line;
                Connect(ref udpClient, ref data);
            }
        }

        static string Connect(ref UdpClient udpClient, ref string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            udpClient.Send(bytes, bytes.Length, new IPEndPoint(IPAddress.Parse("127.0.0.1"), 11000));
            IPEndPoint endPoint = (IPEndPoint)udpClient.Client.LocalEndPoint;
            byte[] recBytes = udpClient.Receive(ref endPoint); ;
            string recData = Encoding.UTF8.GetString(recBytes);
            return recData;
        }
    }
}
