using System.Collections;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 协程组件
    /// </summary>
    public class CoroutineComponent : GameFrameworkComponent
    {
        private ICoroutineManager mCoroutineManager;
        private CoroutineBehaviour mBehaviour;

        protected override void Awake()
        {
            base.Awake();

            mCoroutineManager = new CoroutineManager();

            var obj = new GameObject("CoroutineBehaviour");
            mBehaviour = obj.GetOrAddComponent<CoroutineBehaviour>();
            
            mCoroutineManager.SetBehaviour(mBehaviour);
        }

        /// <summary>
        /// 执行协程
        /// </summary>
        public void DoCoroutine(string name, IEnumerator enumerator)
        {
            mCoroutineManager.DoCoroutine(name, enumerator);
        }

        /// <summary>
        /// 执行协程
        /// </summary>
        public void DoCoroutine(IEnumerator enumerator)
        {
            mCoroutineManager.DoCoroutine(enumerator);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public new void StopCoroutine(string name)
        {
            mCoroutineManager.StopCoroutine(name);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public new void StopCoroutine(Coroutine coroutine)
        {
            mCoroutineManager.StopCoroutine(coroutine);
        }

        /// <summary>
        /// 清楚所有缓存协程
        /// </summary>
        public void ClearAll()
        {
            mCoroutineManager.ClearAll();
        }
    }
}