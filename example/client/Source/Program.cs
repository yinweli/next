using System.Collections.Generic;

namespace SimpleClient
{
    internal class Program
    {
        private static string ip = "192.168.1.108";
        private static int port = 3001;
        private static int countOfClientList = 10;
        private static int countOfClientUnit = 500;

        private static List<ClientList> clientLists = new List<ClientList>();

        private static void Main(string[] args)
        {
            for (int i = 0; i < countOfClientList; ++i)
                clientLists.Add(new ClientList(i, countOfClientUnit, ip, port));
        }
    }
}