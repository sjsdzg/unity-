using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XFramework.Core
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// MonoSingleton根节点名称
        /// </summary>
        private const string root_name = "[MonoSingleton]";

        /// <summary>
        /// MonoSingleton的根节点下的Canvas节点名称
        /// </summary>
        private const string root_canvas_name = "[MonoSingleton.Canvas]";

        protected static T instance;
        /// <summary>
        /// 单例
        /// </summary>
        public static T Instance
        {
            get
            {
                //如果为null，从Resources中加载
                if (instance == null)
                {
                    //自动加载及实例化通用资源
                    string assetName = typeof(T).ToString();
                    assetName = assetName.Substring(assetName.LastIndexOf('.') + 1);
                    GameObject prafab = Resources.Load<GameObject>("Common/" + assetName);
                    if (prafab != null)
                    {
                        Instantiate(prafab);
                    }
                }
                //没获取到单例
                if (instance == null)
                {
                    Debug.LogWarningFormat("[MonoSingleton]:{0} is null.", typeof(T).ToString());
                }
                return instance;
            }
        }

        private void Awake()
        {
            //设置单例
            instance = this as T;
            //构建结构
            Build();
            //进行初始化
            Init();
            Debug.LogFormat("[MonoSingleton]:{0} is Inited.", typeof(T).ToString());
        }


        /// <summary>
        /// 构建结构
        /// </summary>
        protected virtual void Build()
        {
            GameObject rootObj = GameObject.Find(root_name);
            if (rootObj == null)
                rootObj = new GameObject(root_name);
            //如果是RectTransform并且不附加Canvas，需要放到[MonoSingleton.Canvas]节点下面
            if (transform is RectTransform && transform.GetComponent<Canvas>() == null)
            {
                GameObject canvasObj = GameObject.Find(root_name + "/" + root_canvas_name);
                if (canvasObj == null)
                    canvasObj = new GameObject(root_canvas_name);

                canvasObj.transform.SetParent(rootObj.transform);
                //设置Canvas
                Canvas canvas = canvasObj.GetOrAddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 8;
                canvasObj.GetOrAddComponent<CanvasScaler>();
                canvasObj.GetOrAddComponent<GraphicRaycaster>();
                //设置层级
                transform.SetParent(canvasObj.transform);
                transform.gameObject.layer = canvasObj.layer;
            }
            else
            {
                //设置层级
                transform.SetParent(rootObj.transform);
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {

        }

        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void Release()
        {

        }
    }
}

