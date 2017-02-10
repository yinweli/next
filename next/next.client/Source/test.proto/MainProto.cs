using System.Collections.Generic;

namespace next.client
{
    internal class MainProto
    {
        private static string ip = "192.168.31.227";
        private static int port = 9527;
        private static int countOfClientList = 1;
        private static int countOfClientUnit = 1;

        private static List<ProtoClientList> clientLists = new List<ProtoClientList>();

        private static void Main(string[] args)
        {
            for (int i = 0; i < countOfClientList; ++i)
            {
                ProtoClientList clientList = new ProtoClientList();

                clientList.start(ip, port, countOfClientUnit);

                clientLists.Add(clientList);
            }//for
        }
    }
}