using System.Collections.Generic;
using System.Threading;

namespace next.client
{
    public class JsonClientList
    {
        private List<JsonClient> clients = new List<JsonClient>();
        private Thread thread = null;

        public void start(string ip, int port, int count)
        {
            for (int i = 0; i < count; ++i)
            {
                JsonClient client = new JsonClient();

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
                foreach (JsonClient itor in clients)
                    itor.update();

                Thread.Sleep(10);
            }//while
        }
    }
}