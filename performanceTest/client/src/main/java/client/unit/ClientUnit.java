package client.unit;

import com.google.protobuf.Descriptors.Descriptor;
import com.google.protobuf.Message.Builder;

import packet._core.CorePacketWapper;
import surfer.net.netty.NettyClient;
import surfer.net.netty.handler.HandlerProto;
import surfer.net.netty.handler.HandlerProto.CoreProto;

public class ClientUnit
{
    /** 客戶端物件 */
    private NettyClient nettyClient = new NettyClient();
    /** 封包處理物件 */
    private HandlerProto handlerProto = new HandlerProto(new CoreProto() {
        @Override
        public Descriptor getDescriptor()
        {
            return CorePacketWapper.CorePacket.getDescriptor();
        }
        
        @Override
        public Object getDefaultInstance()
        {
            return CorePacketWapper.CorePacket.getDefaultInstance();
        }
        
        @Override
        public Builder getBuilder()
        {
            return CorePacketWapper.CorePacket.newBuilder();
        }
    });
}
