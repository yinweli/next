namespace FouridStudio
{
    /// <summary>
    /// 繼承此類別後, 便擁有Singleton設計模式的能力
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : class, new()
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = new T();

                return instance;
            }
        }
    }
}