using System.Collections.Generic;
using System.Threading;

namespace SimpleClient
{
    public class ClientList
    {
        private int id = 0;
        private int count = 0;
        private string ip = "";
        private int port = 0;
        private List<Client> clients = new List<Client>();
        private Thread thread = null;

        public ClientList(int id, int count, string ip, int port)
        {
            this.id = id;
            this.count = count;
            this.ip = ip;
            this.port = port;

            new Thread(start).Start();
        }

        private void start()
        {
            for (int i = 0; i < count; ++i)
            {
                clients.Add(new Client((id * count) + i, ip, port));
                Thread.Sleep(10);
            }//for

            thread = new Thread(update);
            thread.Start();
        }

        private void update()
        {
            while (true)
            {
                foreach (Client itor in clients)
                    itor.update();

                Thread.Sleep(10);
            }//while
        }
    }
}