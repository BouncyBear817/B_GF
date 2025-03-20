using System.Collections;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 协程管理器接口
    /// </summary>
    public interface ICoroutineManager
    {
        /// <summary>
        /// 设置协程行为
        /// </summary>
        void SetBehaviour(CoroutineBehaviour behaviour);
        
        /// <summary>
        /// 执行协程
        /// </summary>
        void DoCoroutine(string name, IEnumerator enumerator);

        /// <summary>
        /// 执行协程
        /// </summary>
        void DoCoroutine(IEnumerator enumerator);

        /// <summary>
        /// 停止协程
        /// </summary>
        void StopCoroutine(string name);

        /// <summary>
        /// 停止协程
        /// </summary>
        void StopCoroutine(Coroutine coroutine);

        /// <summary>
        /// 清楚所有缓存协程
        /// </summary>
        void ClearAll();
    }
}