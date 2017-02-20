package next.net.netty.handler;

import io.netty.channel.socket.SocketChannel;
import io.netty.handler.codec.protobuf.ProtobufDecoder;
import io.netty.handler.codec.protobuf.ProtobufEncoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32FrameDecoder;
import io.netty.handler.codec.protobuf.ProtobufVarint32LengthFieldPrepender;

import java.util.Map;
import java.util.Queue;
import java.util.concurrent.ConcurrentHashMap;
import java.util.concurrent.ConcurrentLinkedQueue;

import next.net.netty.connection.Connection;

import com.google.protobuf.AbstractMessage;
import com.google.protobuf.Descriptors.Descriptor;
import com.google.protobuf.Descriptors.FieldDescriptor;
import com.google.protobuf.Message.Builder;
import com.google.protobuf.MessageLite;

/**
 * <pre>
 * 以google protobuf為基礎的封包處理類別
 * 
 * > 封包格式
 * 以一個核心proto, 其他的proto將以子proto方式被包含在此核心proto內, 並且欄位名稱必須與這些proto的名稱一致
 * 另外以一個int32的列表, 名稱為type的成員來指示封包使用的proto的編號, 此type成員的封包編號必須為1
 * 核心proto內各個子proto的封包編號不可重複
 * messageCode(google protobuf 3.0.0):
 * <code>
 * message Proto
 * {
 *     repeated int32 type = 1; // 封包編號, 編號內容為的子項目proto的編號列表
 *     
 *     Data1 Data1 = 101; // proto類別, 欄位名稱與類別名稱必須一致
 *     Data2 Data2 = 102; // proto類別
 *     Data3 Data3 = 103; // proto類別
 *     Data4 Data4 = 104; // proto類別
 * }
 * </code>
 * 
 * > 封包類別
 * 以google protobuf 3.0.0的編譯工具編譯出來即可
 * 
 * > 注意!一個封包編號(以及其封包物件)只能在核心封包中出現一次
 * </pre>
 * 
 * @author yilin_lee
 */
public class HandlerProto extends BaseHandler
{
    /**
     * <pre>
     * 核心proto介面
     * </pre>
     */
    public static interface CoreProto
    {
        /**
         * <pre>
         * 取得核心proto建造物件
         * </pre>
         * 
         * @return 核心proto建造物件
         */
        Builder getBuilder();
        
        /**
         * <pre>
         * 取得核心proto描述物件
         * </pre>
         * 
         * @return 核心proto描述物件
         */
        Descriptor getDescriptor();
        
        /**
         * <pre>
         * 取得核心proto預設實體
         * </pre>
         * 
         * @return 核心proto預設實體
         */
        Object getDefaultInstance();
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
    
    /** 型態欄位編號 */
    private static final int FIELD_TYPE = 1;
    
    /** 核心proto物件 */
    private CoreProto coreProto = null;
    /** 連線處理列表 */
    private Queue<Processor> connectProcessors = new ConcurrentLinkedQueue<>();
    /** 斷線處理列表 */
    private Queue<Processor> disconnectProcessors = new ConcurrentLinkedQueue<>();
    /** 封包處理列表 */
    private Map<Integer, Processor> packetProcessors = new ConcurrentHashMap<>();
    
    /**
     * <pre>
     * 建構子需要核心proto物件, 接收者物件
     * </pre>
     * 
     * @param coreProto 核心proto物件
     * @param receiver 接收者物件
     */
    public HandlerProto(final CoreProto coreProto)
    {
        this.coreProto = coreProto;
    }
    
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
        FieldDescriptor field = coreProto.getDescriptor().findFieldByName(name);
        
        if (field == null)
            throw new Exception("field not found(" + name + ")");
        
        int number = field.getNumber();
        
        if (packetProcessors.containsKey(number))
            throw new Exception("duplicate processor(" + name + ")");
        
        packetProcessors.put(number, processor);
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public final void coder(SocketChannel soc) throws Exception
    {
        soc.pipeline().addLast("proto_decoder(frame)", new ProtobufVarint32FrameDecoder());
        soc.pipeline().addLast("proto_decoder", new ProtobufDecoder((MessageLite) coreProto.getDefaultInstance()));
        soc.pipeline().addLast("proto_encoder(frame)", new ProtobufVarint32LengthFieldPrepender());
        soc.pipeline().addLast("proto_encoder", new ProtobufEncoder());
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
        if (coreProto == null)
            throw new Exception("proto core null");
        
        if (connection == null)
            throw new Exception("connection null");
        
        if (object == null)
            throw new Exception("object null");
        
        if (object instanceof AbstractMessage == false)
            throw new Exception("object not proto");
        
        AbstractMessage corePacket = (AbstractMessage) object;
        FieldDescriptor fieldType = corePacket.getDescriptorForType().findFieldByNumber(FIELD_TYPE);
        
        if (fieldType == null)
            throw new Exception("type not found");
        
        for (int i = 0, max = corePacket.getRepeatedFieldCount(fieldType); i < max; ++i)
        {
            int type = (int) corePacket.getRepeatedField(fieldType, i);
            FieldDescriptor fieldData = corePacket.getDescriptorForType().findFieldByNumber(type);
            
            if (fieldData == null)
                throw new Exception("data not found(" + type + ")");
            
            Processor processor = packetProcessors.get(type);
            
            if (processor == null)
                throw new Exception("processor not found(" + type + ")");
            
            processor.onEvent(connection, corePacket.getField(fieldData));
        }//for
    }
    
    /**
     * {@inheritDoc}
     */
    @Override
    public final Object send(final Object... objects) throws Exception
    {
        if (coreProto == null)
            throw new Exception("proto core null");
        
        if (objects == null)
            throw new Exception("packet null");
        
        if (objects.length <= 0)
            throw new Exception("packet empty");
        
        // 取得核心proto建造物件
        Builder builder = coreProto.getBuilder();
        
        if (builder == null)
            throw new Exception("create builder failed");
        
        // 檢查核心proto是否有型態欄位
        FieldDescriptor fieldType = builder.getDescriptorForType().findFieldByNumber(FIELD_TYPE);
        
        if (fieldType == null)
            throw new Exception("type not found");
        
        for (Object itor : objects)
        {
            if (itor instanceof AbstractMessage == false)
                throw new Exception("packet not proto");
            
            // 取得要傳送的proto名稱
            String name = itor.getClass().getSimpleName();
            
            // 檢查核心proto是否有要傳送的proto欄位
            FieldDescriptor fieldData = builder.getDescriptorForType().findFieldByName(name);
            
            if (fieldData == null)
                throw new Exception("data not found(" + name + ")");
            
            builder.addRepeatedField(fieldType, fieldData.getNumber());
            builder.setField(fieldData, itor);
        }//for
        
        return builder.build();
    }
}
