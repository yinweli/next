using System.Collections.Generic;
using System.Threading;

namespace next.client
{
    public class ProtoClientList
    {
        private List<ProtoClient> clients = new List<ProtoClient>();
        private Thread thread = null;

        public void start(string ip, int port, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                ProtoClient client = new ProtoClient();

                client.start(ip, port);

                clients.Add(client);
            }//for

            thread = new Thread(update);
            thread.Start();
        }

        private void update()
        {
            while (true)
            {
                foreach (ProtoClient itor in clients)
                    itor.update();

                Thread.Sleep(10);
            }//while
        }
    }
}