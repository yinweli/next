using System;

namespace FouridStudio
{
    /// <summary>
    /// 物件池類別
    /// 注意!不支援多執行緒
    /// </summary>
    /// <typeparam name="T">物件型別</typeparam>
    public class ObjectPool<T> where T : new()
    {
        /// <summary>
        /// 委派型態:建立物件
        /// </summary>
        /// <returns>建立物件</returns>
        public delegate T AllocObject();

        /// <summary>
        /// 委派型態:釋放物件, 必須傳回instance
        /// </summary>
        /// <param name="instance">釋放物件</param>
        /// <returns>釋放物件</returns>
        public delegate T FreeObject(T instance);

        /// <summary>
        /// 當可用物件不足時, 分派的新物件數量
        /// </summary>
        public int growSize = 100;

        /// <summary>
        /// 建立物件委派
        /// </summary>
        public AllocObject allocObject = null;

        /// <summary>
        /// 釋放物件委派
        /// </summary>
        public FreeObject freeObject = null;

        /// <summary>
        /// 物件列表
        /// </summary>
        private T[] objects = null;

        /// <summary>
        /// 使用索引
        /// </summary>
        private int nextIndex = 0;

        /// <summary>
        /// 建立物件
        /// </summary>
        /// <returns>建立物件</returns>
        public T alloc()
        {
            if (objects == null || nextIndex >= objects.Length)
                resize();

            return objects[nextIndex++];
        }

        /// <summary>
        /// 釋放物件
        /// </summary>
        /// <param name="instance">釋放物件</param>
        public void free(T instance)
        {
            if (nextIndex > 0)
                objects[--nextIndex] = freeObject != null ? freeObject(instance) : instance;
        }

        /// <summary>
        /// 調整物件池大小
        /// </summary>
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
}