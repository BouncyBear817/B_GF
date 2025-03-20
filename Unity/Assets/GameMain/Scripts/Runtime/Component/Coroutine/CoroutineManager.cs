using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace GameMain
{
    /// <summary>
    /// 协程管理器
    /// </summary>
    public class CoroutineManager : ICoroutineManager
    {
        private readonly Dictionary<string, Coroutine> mCoroutines = new Dictionary<string, Coroutine>();

        private CoroutineBehaviour mBehaviour;

        /// <summary>
        /// 设置协程行为
        /// </summary>
        public void SetBehaviour(CoroutineBehaviour behaviour)
        {
            mBehaviour = behaviour;
        }

        /// <summary>
        /// 执行协程
        /// </summary>
        public void DoCoroutine(string name, IEnumerator enumerator)
        {
            if (mBehaviour == null)
            {
                Log.Error("Behaviour is invalid. Please Set Behaviour first.");
                return;
            }

            var key = name;
            if (string.IsNullOrEmpty(key))
            {
                key = nameof(enumerator);
            }

            if (!mCoroutines.ContainsKey(key))
            {
                mCoroutines.Add(key, mBehaviour.StartCoroutine(enumerator));
            }
        }

        /// <summary>
        /// 执行协程
        /// </summary>
        public void DoCoroutine(IEnumerator enumerator)
        {
            if (mBehaviour == null)
            {
                Log.Error("Behaviour is invalid. Please Set Behaviour first.");
                return;
            }

            mBehaviour.StartCoroutine(enumerator);
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public void StopCoroutine(string name)
        {
            if (mBehaviour == null)
            {
                Log.Error("Behaviour is invalid. Please Set Behaviour first.");
                return;
            }

            if (mCoroutines.TryGetValue(name, out var coroutine))
            {
                mBehaviour.StopCoroutine(coroutine);
                mCoroutines.Remove(name);
            }
        }

        /// <summary>
        /// 停止协程
        /// </summary>
        public void StopCoroutine(Coroutine coroutine)
        {
            if (mBehaviour == null)
            {
                Log.Error("Behaviour is invalid. Please Set Behaviour first.");
                return;
            }
            
            mBehaviour.StopCoroutine(coroutine);
        }

        /// <summary>
        /// 清楚所有缓存协程
        /// </summary>
        public void ClearAll()
        {
            if (mBehaviour == null)
            {
                Log.Error("Behaviour is invalid. Please Set Behaviour first.");
                return;
            }
            
            foreach (var value in mCoroutines.Values)
            {
                mBehaviour.StopCoroutine(value);
            }
            
            mCoroutines.Clear();
        }
    }
}