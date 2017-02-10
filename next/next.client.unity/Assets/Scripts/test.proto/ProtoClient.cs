using next.client.proto;
using next.net;
using System;
using UnityEngine;

namespace next.client
{
    public class ProtoClient
    {
        private ClientNetork clientNetwork = new ClientNetork();
        private HandlerProto<PacketCore> handler = new HandlerProto<PacketCore>();

        public void start(string ip, int port)
        {
            clientNetwork.ip = ip;
            clientNetwork.port = port;
            clientNetwork.handler = handler;

            handler.add<Packet1>(receivePacket1);
            handler.add<Packet2>(receivePacket2);

            try
            {
                clientNetwork.connect();
            }
            catch (Exception e)
            {
                Debug.Log("connect failed");
                Debug.Log(e);
            }
        }

        public void update()
        {
            try
            {
                if (clientNetwork.isConnect)
                    clientNetwork.update();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void receivePacket1(System.Object obj)
        {
            Packet1 packet = obj as Packet1;

            Debug.Log(string.Format("playerId={0}, money={1}, time={2}, vip={3}", packet.PlayerId, packet.Money, packet.Time, packet.Vip));
        }

        private void receivePacket2(System.Object obj)
        {
            Packet2 packet = obj as Packet2;

            Debug.Log(string.Format("name={0}, message={1}", packet.Name, packet.Message));

            clientNetwork.send(
                new Packet1() { PlayerId = 2048, Money = 10, Time = 1234, Vip = false },
                new Packet2() { Name = "回報人", Message = "收到很多錢" });
        }
    }
}