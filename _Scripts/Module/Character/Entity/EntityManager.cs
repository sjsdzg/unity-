using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// 实体管理器
    /// </summary>
    public class EntityManager : Singleton<EntityManager>
    {
        /// <summary>
        /// EntityNPC List
        /// </summary>
        private List<EntityNPC> m_EntityNPCList = new List<EntityNPC>();

        /// <summary>
        /// EntityMyself
        /// </summary>
        private EntityMyself entityMyself = null;

        private void Clear()
        {
            m_EntityNPCList.Clear();
            entityMyself = null;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Initialize(List<EntityBase> entities)
        {
            Clear();

            foreach (var entity in entities)
            {
                entity.Generate();
                if (entity is EntityNPC)
                {
                    m_EntityNPCList.Add((EntityNPC)entity);
                }
                else if (entity is EntityMyself)
                {
                    entityMyself = (EntityMyself)entity;
                }
            }
        }

        /// <summary>
        /// 根据Id,获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityNPC GetEntityNPCById(string id)
        {
            EntityNPC npc = null;
            EntityBase entity = m_EntityNPCList.Find(x => x.Id == id);
            npc = entity as EntityNPC;
            return npc;
        }

        /// <summary>
        /// 根据名称，获取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public EntityNPC GetEntityNPCByName(string name)
        {
            EntityNPC npc = null;
            EntityBase entity = m_EntityNPCList.Find(x => x.Name == name);
            npc = entity as EntityNPC;
            return npc;
        }

        /// <summary>
        /// 获取自身
        /// </summary>
        /// <returns></returns>
        public EntityMyself GetEntityMyself()
        {
            return entityMyself;
        }
    }
}
