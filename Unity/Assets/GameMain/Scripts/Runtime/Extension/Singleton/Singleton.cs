using System;

namespace GameMain
{
    public abstract class Singleton<T> where T : class
    {
        private class Nested
        {
            // 创建模板类实例，参数2设为true表示支持私有构造函数
            internal static readonly T Instance = Activator.CreateInstance(typeof(T), true) as T;
        }

        public static T Instance => Nested.Instance;
    }
}