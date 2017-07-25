package next.net.netty.handler;

import io.netty.buffer.ByteBuf;
import io.netty.buffer.Unpooled;
import io.netty.channel.ChannelHandlerContext;
import io.netty.channel.socket.SocketChannel;
import io.netty.handler.codec.MessageToMessageDecoder;
import io.netty.handler.codec.MessageToMessageEncoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32FrameDecoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32LengthFieldPrepender;

import java.nio.charset.StandardCharsets;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
import java.util.Queue;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentLinkedQueue;

import next.net.netty.connection.Connection;

import com.google.gson.Gson;
import com.google.gson.JsonObject;

/**
 * <pre>
 * 以google gson為基礎的封包處理類別
 * 
 * > 封包格式
 * <code>
 * jsonCode:
 * {
 *     "type":[ <-- 存放封包名稱的列表
 *         "packetName1", <-- 第一個封包的名稱
 *         "packetName2", <-- 第二個封包的名稱
 *         ...
 *     ],
 *     "data":[ <-- 存放封包內容的列表, 內容也是json格式
 *         "\"data1\":\"12345\", \"data2\":99875", <-- 第一個封包的內容
 *         "\"data1\":\"12345\", \"data2\":99875", <-- 第二個封包的內容
 *         ...
 *     ]
 * }
 * </code>
 * 
 * > 封包類別
 * 封包類別只要是單純合法的java類別即可, 但是注意封包成員最好別用Map系列容器
 * 
 * > 注意!一個封包名稱(以及其封包物件)只能在核心封包中出現一次
 * </pre>
 * 
 * @author yilin_lee
 */
public class HandlerGson extends BaseHandler
{
    /**
     * <pre>
     * 核心封包類別
     * </pre>
     */
    public static class CorePacket
    {
        /** 封包名稱列表 */
        private List<String> types = new ArrayList<>();
        /** 封包內容列表 */
        private List<String> datas = new ArrayList<>();
        
        /**
         * <pre>
         * 新增封包
         * </pre>
         * 
         * @param packetName 封包名稱
         * @param packetData 封包內容
         */
        public void add(String packetName, String packetData)
        {
            types.add(packetName);
            datas.add(packetData);
        }
        
        /**
         * <pre>
         * 取得封包數量
         * </pre>
         * 
         * @return 封包數量
         */
        public int size()
        {
            return types.size();
        }
        
        /**
         * <pre>
         * 取得封包名稱
         * </pre>
         * 
         * @param index 索引位置
         * @return 封包名稱
         */
        public String getPacketName(int index)
        {
            return types.get(index);
        }
        
        /**
         * <pre>
         * 取得封包內容
         * </pre>
         * 
         * @param index 索引位置
         * @return 封包內容
         */
        public String getPacketData(int index)
        {
            return datas.get(index);
        }
    }
    
    /**
     * <pre>
     * 處理者介面
     * </pre>
     */
    public interface Processor
    {
        /**
         * <pre>
         * 取得封包類別物件
         * </pre>
         * 
         * @return 封包類別物件
         */
        Class<?> packetClass();
        
        /**
         * <pre>
         * 封包處理函式
         * </pre>
         * 
         * @param connection 連線物件
         * @param object 封包物件
         * @throws Exception
         */
        void onEvent(Connection connection, final Object object) throws Exception;
    }
    
    /** gson物件 */
    private static final Gson INSTANCE_GSON = new Gson();
    
    /** 連線處理列表 */
    private Queue<Processor> connectProcessors = new ConcurrentLinkedQueue<>();
    /** 斷線處理列表 */
    private Queue<Processor> disconnectProcessors = new ConcurrentLinkedQueue<>();
    /** 封包處理列表 */
    private Map<String, Processor> packetProcessors = new ConcurrentHashMap<>();
    
    /**
     * <pre>
     * 新增連線處理者物件
     * </pre>
     * 
     * @param processor 處理者物件
     * @throws Exception
     */
    public void addConnectProcessor(final Processor processor) throws Exception
    {
        if (processor == null)
            throw new Exception("processor null");
        
        connectProcessors.add(processor);
    }
    
    /**
     * <pre>
     * 新增斷線處理者物件
     * </pre>
     * 
     * @param processor 處理者物件
     * @throws Exception
     */
    public void addDisconnectProcessor(final Processor processor) throws Exception
    {
        if (processor == null)
            throw new Exception("processor null");
        
        disconnectProcessors.add(processor);
    }
    
    /**
     * <pre>
     * 新增封包處理者物件
     * </pre>
     * 
     * @param processor 處理者物件
     * @throws Exception
     */
    public void addPacketProcessor(final Processor processor) throws Exception
    {
        if (processor == null)
            throw new Exception("processor null");
        
        String name = processor.packetClass().getSimpleName();
        
        if (packetProcessors.containsKey(name))
            throw new Exception("duplicate processor(" + name + ")");
        
        packetProcessors.put(name, processor);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public final void coder(SocketChannel soc) throws Exception
    {
        soc.pipeline().addLast("gson_decoder(frame)", new ProtobufVarint32FrameDecoder());
        soc.pipeline().addLast("gson_decoder", new MessageToMessageDecoder<ByteBuf>() {
            @Override
            protected void decode(ChannelHandlerContext ctx, ByteBuf input, List<Object> output) throws Exception
            {
                final byte[] array;
                final int offset;
                final int length = input.readableBytes();
                
                if (input.hasArray())
                {
                    array = input.array();
                    offset = input.arrayOffset() + input.readerIndex();
                }
                else
                {
                    array = new byte[length];
                    input.getBytes(input.readerIndex(), array, 0, length);
                    offset = 0;
                } // if
                
                output.add(INSTANCE_GSON
                    .fromJson(new String(array, offset, length, StandardCharsets.UTF_8), JsonObject.class));
            }
        });
        soc.pipeline().addLast("gson_encoder(frame)", new ProtobufVarint32LengthFieldPrepender());
        soc.pipeline().addLast("gson_encoder", new MessageToMessageEncoder<Object>() {
            @Override
            protected void encode(ChannelHandlerContext ctx, Object input, List<Object> output) throws Exception
            {
                output.add(Unpooled.wrappedBuffer(INSTANCE_GSON.toJson(input).getBytes(StandardCharsets.UTF_8)));
            }
        });
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public void active(Connection connection) throws Exception
    {
        for (Processor itor : connectProcessors)
            itor.onEvent(connection, null);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public void inactive(Connection connection) throws Exception
    {
        for (Processor itor : disconnectProcessors)
            itor.onEvent(connection, null);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public final void recv(Connection connection, final Object object) throws Exception
    {
        if (connection == null)
            throw new Exception("connection null");
        
        if (object == null)
            throw new Exception("packet null");
        
        if (object instanceof JsonObject == false)
            throw new Exception("packet not json");
        
        CorePacket corePacket = INSTANCE_GSON.fromJson((JsonObject) object, CorePacket.class);
        
        for (int i = 0, max = corePacket.size(); i < max; ++i)
        {
            String packetName = corePacket.getPacketName(i);
            String packetData = corePacket.getPacketData(i);
            Processor processor = packetProcessors.get(packetName);
            
            if (processor == null)
                throw new Exception("processor not found(" + packetName + ")");
            
            processor.onEvent(connection, INSTANCE_GSON.fromJson(packetData, processor.packetClass()));
        } //for
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public final Object send(final Object... objects) throws Exception
    {
        if (objects == null)
            throw new Exception("packet null");
        
        if (objects.length <= 0)
            throw new Exception("packet empty");
        
        CorePacket corePacket = new CorePacket();
        
        for (Object itor : objects)
            corePacket.add(itor.getClass().getSimpleName(), INSTANCE_GSON.toJson(itor));
        
        return corePacket;
    }
}
