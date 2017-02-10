package next.net.netty.test.json;

import next.net.netty.NettyServer;
import next.net.netty.handler.HandlerGson;

public class Main
{
    private static HandlerGson handlerGson = new HandlerGson();
    private static NettyServer nettyServer = new NettyServer();
    
    public static void main(String[] args) throws Exception
    {
        handlerGson.addConnectProcessor(new ProcessorConnect());
        handlerGson.addDisconnectProcessor(new ProcessorDisconnect());
        handlerGson.addPacketProcessor(new ProcessorPacket1());
        handlerGson.addPacketProcessor(new ProcessorPacket2());
        
        nettyServer.port = 9527;
        nettyServer.countOfThread = 8;
        nettyServer.handler = handlerGson;
        nettyServer.start();
    }
}
