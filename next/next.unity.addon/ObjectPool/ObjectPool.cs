using System;

// 物件池類別
// 不支援多執行緒
public class ObjectPool<T> where T : new()
{
    // 委派型態:建立物件
    public delegate T AllocObject();

    // 委派型態:釋放物件, 必須傳回instance
    public delegate T FreeObject(T instance);

    // 成長數量
    public int growSize = 100;

    // 建立物件委派
    public AllocObject allocObject = null;

    // 釋放物件委派
    public FreeObject freeObject = null;

    // 物件列表
    private T[] objects = null;

    // 使用索引
    private int nextIndex = 0;

    // 建立物件
    public T alloc()
    {
        if (objects == null || nextIndex >= objects.Length)
            resize();

        return objects[nextIndex++];
    }

    // 釋放物件
    public void free(T instance)
    {
        if (nextIndex > 0)
            objects[--nextIndex] = freeObject != null ? freeObject(instance) : instance;
    }

    // 調整物件池大小
    private void resize()
    {
        int oldSize = 0;
        T[] newObjects = null;

        if (objects != null)
        {
            oldSize = objects.Length;
            newObjects = new T[oldSize + growSize];

            Array.Copy(objects, newObjects, oldSize);
        }
        else
            newObjects = new T[growSize];

        for (int i = oldSize; i < newObjects.Length; ++i)
            newObjects[i] = allocObject != null ? allocObject() : new T();

        objects = newObjects;
    }
}