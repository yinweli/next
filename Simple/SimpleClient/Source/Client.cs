using next.net;
using packet._core;
using packet.simple;
using System;

namespace SimpleClient
{
    public class Client
    {
        private const int MAX_VALUE = 10000;
        private const int SEND_INTERVAL = 1000;

        private int clientId = 0;
        private long lastTime = 0;
        private Random random = null;
        private ClientNetork clientNetwork = new ClientNetork();
        private HandlerProto<CorePacket> handler = new HandlerProto<CorePacket>();

        public Client(int id, string ip, int port)
        {
            random = new Random(clientId = id);
            clientNetwork.ip = ip;
            clientNetwork.port = port;
            clientNetwork.handler = handler;

            handler.add<Simple1>(receiveSimple1);
            handler.add<Simple2>(receiveSimple2);

            try
            {
                clientNetwork.connect();
            }
            catch (Exception e)
            {
                Console.WriteLine("connect failed");
                Console.WriteLine(e);
            }
        }

        public void update()
        {
            try
            {
                if (clientNetwork.isConnect)
                {
                    clientNetwork.update();

                    long nowTime = TimeUtil.currentMillisecond();

                    if (nowTime - lastTime >= SEND_INTERVAL)
                    {
                        lastTime = nowTime;
                        sendSimple1();
                    }//if
                }//if
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void sendSimple1()
        {
            long startTime = TimeUtil.currentMillisecond();
            int value1 = random.Next(MAX_VALUE);
            int value2 = random.Next(MAX_VALUE);
            String title = "client_" + clientId;

            clientNetwork.send(new Simple1() { StartTime = startTime, Value1 = value1, Value2 = value2, Title = title });
        }

        private void receiveSimple1(System.Object obj)
        {
            Simple1 packet = obj as Simple1;

            Reporter.report(TimeUtil.currentMillisecond() - packet.StartTime);
        }

        private void receiveSimple2(System.Object obj)
        {
        }
    }
}