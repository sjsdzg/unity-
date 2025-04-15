using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using XFramework.Common;
using System.Xml.Serialization;
using XFramework.Core;

namespace XFramework.Module
{
    [XmlType("EntityMyself")]
    public class EntityMyself : EntityBase
    {
        public override void Generate()
        {
            base.Generate();
            //加载自身角色
            string path = CharacterDefine.GetCharacterPrefabPath(Cleanliness, Gender);
            //ResourceManager.Instance.LoadCoroutineInstance(path, x =>
            //{
            //    GameObject myself = x as GameObject;
            //    myself.transform.localPosition = Converter.ToVector3(Position);
            //    myself.transform.localEulerAngles = Converter.ToVector3(Rotation);
            //    myself.transform.localScale = Converter.ToVector3(Scale);

            //    myself.name = Name;
            //    myself.AddComponent<MyselfControl>();
            //    MyselfControl control = myself.GetComponent<MyselfControl>();
            //    control.Entity = this;

            //    CacheTransform = myself.transform;
            //    CacheTransform.tag = "Player";
            //    Camera.main.GetComponent<CameraController>().Target = CacheTransform;
            //    Camera.main.transform.eulerAngles = CacheTransform.transform.eulerAngles;
            //    //Selector
            //    Selector m_Selector = Camera.main.GetComponent<Selector>();
            //    if (m_Selector != null)
            //    {
            //        m_Selector.Player = CacheTransform;
            //    }
            //});

            AsyncLoadAssetOperation async = Assets.LoadAssetAsync<GameObject>(path);
            if (async == null)
                return;

            async.OnCompleted(x =>
            {
                AsyncLoadAssetOperation loader = x as AsyncLoadAssetOperation;
                GameObject prefab = loader.GetAsset<GameObject>();
                GameObject myself = MonoBehaviour.Instantiate(prefab);
                myself.transform.localPosition = Converter.ToVector3(Position);
                myself.transform.localEulerAngles = Converter.ToVector3(Rotation);
                myself.transform.localScale = Converter.ToVector3(Scale);

                myself.name = Name;
                myself.AddComponent<MyselfControl>();
                MyselfControl control = myself.GetComponent<MyselfControl>();
                control.Entity = this;

                CacheTransform = myself.transform;
                CacheTransform.tag = "Player";
                Camera.main.GetComponent<CameraController>().Target = CacheTransform;
                Camera.main.transform.eulerAngles = CacheTransform.transform.eulerAngles;
                //Selector
                Selector m_Selector = Camera.main.GetComponent<Selector>();
                if (m_Selector != null)
                {
                    m_Selector.Player = CacheTransform;
                }
            });
        }
    }
}
