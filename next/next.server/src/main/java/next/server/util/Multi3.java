package next.server.util;

/**
 * <pre>
 * 相當於C#的Tuple
 * </pre>
 */
public class Multi3<T1, T2, T3>
{
    private T1 t1 = null;
    private T2 t2 = null;
    private T3 t3 = null;
    
    public Multi3(T1 t1, T2 t2, T3 t3)
    {
        this.t1 = t1;
        this.t2 = t2;
        this.t3 = t3;
    }
    
    public T1 get1()
    {
        return t1;
    }
    
    public T2 get2()
    {
        return t2;
    }
    
    public T3 get3()
    {
        return t3;
    }
}
