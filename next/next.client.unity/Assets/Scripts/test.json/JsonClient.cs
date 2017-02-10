using LitJson;
using next.client.json;
using next.net;
using System;
using UnityEngine;

namespace next.client
{
    public class JsonClient
    {
        private ClientNetork clientNetwork = new ClientNetork();
        private HandlerJson handler = new HandlerJson(new JsonImplement());

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
            if (clientNetwork.isConnect)
                clientNetwork.update();
        }

        private void receivePacket1(System.Object obj)
        {
            Packet1 packet = obj as Packet1;

            Debug.Log(string.Format("playerId={0}, money={1}, time={2}, vip={3}", packet.playerId, packet.money, packet.time, packet.vip));
        }

        private void receivePacket2(System.Object obj)
        {
            Packet2 packet = obj as Packet2;

            Debug.Log(string.Format("name={0}, message={1}", packet.name, packet.message));

            clientNetwork.send(
                new Packet1() { playerId = 2048, money = 10, time = 1234, vip = false },
                new Packet2() { name = "回報人", message = "收到很多錢" });
        }
    }

    internal class JsonImplement : JsonProcess
    {
        public string toJson(System.Object obj)
        {
            return JsonMapper.ToJson(obj);
        }

        public T toObject<T>(string json)
        {
            return JsonMapper.ToObject<T>(json);
        }
    }
}