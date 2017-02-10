using System.Collections.Generic;

namespace next.client
{
    internal class MainJson
    {
        private static string ip = "192.168.31.227";
        private static int port = 9527;
        private static int countOfClientList = 500;
        private static int countOfClientUnit = 20;

        private static List<JsonClientList> clientLists = new List<JsonClientList>();

        private static void Main(string[] args)
        {
            for (int i = 0; i < countOfClientList; ++i)
            {
                JsonClientList clientList = new JsonClientList();

                clientList.start(ip, port, countOfClientUnit);

                clientLists.Add(clientList);
            }//for
        }
    }
}