package client.unit;

import java.util.ArrayList;
import java.util.List;

import org.apache.log4j.Logger;

public class ClientUnits
{
    private static final Logger log = Logger.getLogger(ClientUnits.class);
    
    /** 客戶端數量 */
    public int count = 0;
    /** 伺服器位址 */
    public String ip = "";
    /** 伺服器埠號 */
    public int port = 0;
    /** 間隔時間 */
    public long interval = 0;
    
    /** 客戶端列表 */
    private List<ClientUnit> clientUnits = new ArrayList<>();
    /** 上次更新時間 */
    private long lastTime = 0;
    
    /**
     * <pre>
     * 啟動客戶端列表
     * </pre>
     */
    public void start()
    {
        for (int i = 0; i < count; ++i)
        {
            try
            {
                clientUnits.add(new ClientUnit().start(ip, port));
            }
            catch (Exception e)
            {
                e.printStackTrace();
            }
        } //for
    }
    
    public void update()
    {
        long currentTime = System.currentTimeMillis();
        
        if (currentTime - lastTime < 10000)
            return;
        
        lastTime = currentTime;
        
        log.info(String.format("connected=%d", clientUnits.stream().filter(itor -> itor.getConnected()).count()));
    }
}
