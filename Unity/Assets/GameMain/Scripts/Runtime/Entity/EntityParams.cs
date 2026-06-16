using GameFramework;
using UnityGameFramework.Runtime;
using UnityEngine;

namespace GameMain
{
    /// <summary>
    /// 实体参数
    /// </summary>
    public class EntityParams : IReference
    {
        private static int sEntityId = 0;
        
        public int Id { get; private set; }
        public Vector3? Position { get; set; } = null;
        public Vector3? LocalPosition { get; set; } = null;
        public Vector3? EulerAngles { get; set; } = null;
        public Vector3? LocalEulerAngles { get; set; } = null;
        public Vector3? LocalScale { get; set; } = null;

        public int GameObjectLayer { get; set; } = -1;

        /// <summary>
        /// 绑定到父实体
        /// </summary>
        public UnityGameFramework.Runtime.Entity AttachToEntity;
        
        /// <summary>
        /// 指定绑定到父实体下的哪个节点
        /// </summary>
        public Transform ParentTransform;

        /// <summary>
        /// 实体显示时回调
        /// </summary>
        public GameFrameworkAction<EntityLogic> OnShowEntityCallback;
        
        /// <summary>
        /// 实体隐藏时回调
        /// </summary>
        public GameFrameworkAction<EntityLogic> OnHideEntityCallback;

        /// <summary>
        /// 创建一个实例(必须使用该接口创建)
        /// </summary>
        /// <param name="position"></param>
        /// <param name="eulerAngles"></param>
        /// <param name="localScale"></param>
        /// <returns></returns>
        public static EntityParams Create(Vector3? position = null, Vector3? eulerAngles = null, Vector3? localScale = null)
        {
            var entityParams = ReferencePool.Acquire<EntityParams>();
            entityParams.Id = ++sEntityId;
            entityParams.Position = position;
            entityParams.EulerAngles = eulerAngles;
            entityParams.LocalScale = localScale;

            return entityParams;
        }
        
        public void Clear()
        {
            Position = null;
            LocalPosition = null;
            EulerAngles = null;
            LocalEulerAngles = null;
            LocalScale = null;
            GameObjectLayer = -1;

            AttachToEntity = null;
            ParentTransform = null;
            OnShowEntityCallback = null;
            OnHideEntityCallback = null;
        }
    }
}