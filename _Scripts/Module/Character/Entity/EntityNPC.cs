using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using XFramework.Common;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    /// <summary>
    /// NPC实体
    /// </summary>
    [XmlType("EntityNPC")]
    public class EntityNPC : EntityBase
    {
        public override void Generate()
        {
            base.Generate();
            //加载NPC角色
            string path = CharacterDefine.GetCharacterPrefabPath(Cleanliness, Gender);
            //ResourceManager.Instance.LoadCoroutineInstance(path, x =>
            //{
            //    GameObject npc = x as GameObject;
            //    npc.transform.localPosition = Converter.ToVector3(Position);
            //    npc.transform.localEulerAngles = Converter.ToVector3(Rotation);
            //    npc.transform.localScale = Converter.ToVector3(Scale);

            //    npc.name = Name;
            //    npc.AddComponent<NPCControl>();
            //    NPCControl control = npc.GetComponent<NPCControl>();
            //    control.Entity = this;
            //    control.GetComponent<NavMeshAgent>().speed = 1.5f;

            //    CacheTransform = npc.transform;
            //    CacheTransform.tag = "NPC";
            //});

            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                GameObject prefab = loader.GetAsset<GameObject>();
                GameObject npc = MonoBehaviour.Instantiate(prefab);
                npc.transform.localPosition = Converter.ToVector3(Position);
                npc.transform.localEulerAngles = Converter.ToVector3(Rotation);
                npc.transform.localScale = Converter.ToVector3(Scale);

                npc.name = Name;
                npc.AddComponent<NPCControl>();
                NPCControl control = npc.GetComponent<NPCControl>();
                control.Entity = this;
                control.GetComponent<NavMeshAgent>().speed = 1.5f;

                CacheTransform = npc.transform;
                CacheTransform.tag = "NPC";
            });
        }
    }
}
