using GameFramework;
using UnityGameFramework.Runtime;

namespace GameMain.Scripts.Runtime.Entity
{
    public abstract class BEntity : EntityLogic
    {
        /// <summary>
        /// 实体编号
        /// </summary>
        public int Id => Entity.Id;
        
        public EntityParams EntityParams { get; private set; }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            
            if (userData == null)
            {
                Log.Error("创建Entity失败! 你必须为Entity传入EntityParams数据");
            }
            
            EntityParams = userData as EntityParams;
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            
            if (userData == null)
            {
                Log.Error("创建Entity失败! 你必须为Entity传入EntityParams数据");
            }
            
            OnRegisterEvent();
            
            EntityParams = userData as EntityParams;
            
            if (MainEntry.Entity.IsValidEntity(EntityParams.AttachToEntity))
            {
                MainEntry.Entity.AttachEntity(this.Entity, EntityParams.AttachToEntity, EntityParams.ParentTransform);
            }
            if (EntityParams.Position != null)
            {
                this.CachedTransform.position = EntityParams.Position.Value;
            }
            if (EntityParams.EulerAngles != null)
            {
                this.CachedTransform.eulerAngles = EntityParams.EulerAngles.Value;
            }
            if (EntityParams.LocalScale != null)
            {
                this.CachedTransform.localScale = EntityParams.LocalScale.Value;
            }
            if (EntityParams.GameObjectLayer >= 0)
            {
                gameObject.layer = EntityParams.GameObjectLayer;
            }

            EntityParams.OnShowEntityCallback?.Invoke(this);
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        protected override void OnHide(bool isShutdown, object userData)
        {
            EntityParams.OnHideEntityCallback?.Invoke(this);
            base.OnHide(isShutdown, userData);
            
            OnUnRegisterEvent();

            if (!isShutdown && EntityParams != null)
            {
                ReferencePool.Release(EntityParams);
            }
        }
        
        protected virtual void OnRegisterEvent() { }

        protected virtual void OnUnRegisterEvent() { }
        
        public void Broadcast(uint msgId, params object[] args)
        {
            MainEntry.Messenger.Broadcast(msgId, args);
        }

        public void AddListener(uint msgId, MessageEvent messageEvent)
        {
            MainEntry.Messenger.AddListener(msgId, messageEvent);
        }

        public void RemoveListener(uint msgId, MessageEvent messageEvent)
        {
            MainEntry.Messenger.RemoveListener(msgId, messageEvent);
        }

        public void RemoveAll(uint msgId)
        {
            MainEntry.Messenger.RemoveAll(msgId);
        }
    }
}