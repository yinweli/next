package surfer.server.util;

/**
 * <pre>
 * 相當於C#的Tuple
 * </pre>
 */
public class Multi2<T1, T2>
{
    private T1 t1 = null;
    private T2 t2 = null;
    
    public Multi2(T1 t1, T2 t2)
    {
        this.t1 = t1;
        this.t2 = t2;
    }
    
    public T1 get1()
    {
        return t1;
    }
    
    public T2 get2()
    {
        return t2;
    }
}
