package next.net.netty.test.json;

public class Packet1
{
    private int playerId = 0;
    private int money = 0;
    private long time = 0;
    private boolean vip = false;
    
    public int getPlayerId()
    {
        return playerId;
    }
    
    public void setPlayerId(int playerId)
    {
        this.playerId = playerId;
    }
    
    public int getMoney()
    {
        return money;
    }
    
    public void setMoney(int money)
    {
        this.money = money;
    }
    
    public long getTime()
    {
        return time;
    }
    
    public void setTime(long time)
    {
        this.time = time;
    }
    
    public boolean getVip()
    {
        return vip;
    }
    
    public void setVip(boolean vip)
    {
        this.vip = vip;
    }
}
