using GameFramework;
using UnityGameFramework.Runtime;

namespace GameMain
{
    public static class EntityExtension
    {
         /// <summary>
        /// 创建Entity
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="prefabName">预制体资源名(相对于Assets/AAAGame/Prefabs/Entity目录)</param>
        /// <param name="logicName">Entity逻辑脚本名</param>
        /// <param name="eGroup">Entity所属的组(Const.EntityGroup枚举)</param>
        /// <param name="priority">异步加载优先级</param>
        /// <param name="entityParams">Entity参数(必须)</param>
        /// <returns>Entity Id</returns>
        public static int ShowEntity(this EntityComponent entityComponent, string prefabName, string logicName, Constant.EEntityGroup eGroup, int priority, EntityParams entityParams)
        {
            var entityId = entityParams.Id;
            var assetFullName = AssetUtil.GetEntityAsset(prefabName);
            entityComponent.ShowEntity(entityId, Utility.Assembly.GetType(logicName), assetFullName, eGroup.ToString(), priority, entityParams);
            return entityId;
        }

        /// <summary>
        /// 创建Entity
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="prefabName">预制体资源名(相对于Assets/AAAGame/Prefabs/Entity目录)</param>
        /// <param name="logicName">Entity逻辑脚本名</param>
        /// <param name="eGroup">Entity所属的组(Const.EntityGroup枚举)</param>
        /// <param name="entityParams">Entity参数(必须)</param>
        /// <returns>Entity Id</returns>
        public static int ShowEntity(this EntityComponent entityComponent, string prefabName, string logicName, Constant.EEntityGroup eGroup, EntityParams entityParams)
        {
            return entityComponent.ShowEntity(prefabName, logicName, eGroup, 0, entityParams);
        }

        /// <summary>
        /// 创建Entity
        /// </summary>
        /// <typeparam name="T">Entity逻辑脚本类型</typeparam>
        /// <param name="entityComponent"></param>
        /// <param name="prefabName">预制体资源名(相对于Assets/AAAGame/Prefabs/Entity目录)</param>
        /// <param name="eGroup">Entity所属的组(Const.EntityGroup枚举)</param>
        /// <param name="priority">异步加载优先级</param>
        /// <param name="entityParams">Entity参数(必须)</param>
        /// <returns>Entity Id</returns>
        public static int ShowEntity<T>(this EntityComponent entityComponent, string prefabName, Constant.EEntityGroup eGroup, int priority, EntityParams entityParams) where T : EntityLogic
        {
            var entityId = entityParams.Id;
            var assetFullName = AssetUtil.GetEntityAsset(prefabName);
            entityComponent.ShowEntity<T>(entityId, assetFullName, eGroup.ToString(), priority, entityParams);
            return entityId;
        }

        /// <summary>
        /// 创建Entity
        /// </summary>
        /// <typeparam name="T">Entity逻辑脚本类型</typeparam>
        /// <param name="entityComponent"></param>
        /// <param name="prefabName">预制体资源名(相对于Assets/AAAGame/Prefabs/Entity目录)</param>
        /// <param name="eGroup">Entity所属的组(Const.EntityGroup枚举)</param>
        /// <param name="entityParams">Entity参数(必须)</param>
        /// <returns>Entity Id</returns>
        public static int ShowEntity<T>(this EntityComponent entityComponent, string prefabName, Constant.EEntityGroup eGroup, EntityParams entityParams) where T : EntityLogic
        {
            return entityComponent.ShowEntity<T>(prefabName, eGroup, 0, entityParams);
        }

        /// <summary>
        /// 隐藏一个Entity组下所有Entities
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="groupName"></param>
        public static void HideGroup(this EntityComponent entityComponent, string groupName)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                Log.Warning("Entity Group Is Null Or WhiteSpace");
                return;
            }
            var eGroup = entityComponent.GetEntityGroup(groupName);
            var all = eGroup.GetAllEntities();

            foreach (var entity in all)
            {
                var e = (Entity)entity;
                entityComponent.HideEntity(e);
            }
        }
        
        /// <summary>
        /// 隐藏Entity(带有安全检测, 无需判空)
        /// </summary>
        /// <param name="entityComponent"></param>
        /// <param name="logic"></param>
        public static void HideEntitySafe(this EntityComponent entityComponent, EntityLogic logic)
        {
            if (logic != null && logic.Available)
            {
                entityComponent.HideEntity(logic.Entity);
            }
        }
        
        /// <summary>
        /// 获取Entity的逻辑脚本
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityComponent"></param>
        /// <param name="eId"></param>
        /// <returns></returns>
        public static T GetEntity<T>(this EntityComponent entityComponent, int eId) where T : EntityLogic
        {
            if (!entityComponent.HasEntity(eId)) return null;

            var eLogic = entityComponent.GetEntity(eId).Logic as T;
            return eLogic;
        }
    }
}