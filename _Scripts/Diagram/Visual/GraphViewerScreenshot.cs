using UnityEngine;
using System.Collections;
using XFramework.UIWidgets;
using UnityEngine.UI;
using System.Collections.Generic;

namespace XFramework.Diagram
{
    public class GraphViewerScreenshot : MonoBehaviour
    {
        private GraphViewer m_Viewer;
        /// <summary>
        /// Viewer
        /// </summary>
        public GraphViewer Viewer
        {
            get
            {
                if (m_Viewer == null)
                {
                    m_Viewer = GameObject.FindObjectOfType<GraphViewer>();
                }

                return m_Viewer;
            }
        }

        private Texture2D texture;
        /// <summary>
        /// 当前截图
        /// </summary>
        public Texture2D Texture
        {
            get { return texture; }
        }

        /// <summary>
        /// 捕获截图
        /// </summary>
        /// <returns></returns>
        public IEnumerator Capture()
        {
            // 记录当前选中 unit
            var currentUnit = GraphMaster.Instance.currentSelectedUnit;
            // 记录 viewer 缩放值
            float viewerScale = Viewer.Scale;
            // 记录 content 位置
            Vector3 contentLocalPosition = Viewer.content.localPosition;

            // 将当前选中 unit 设置为空
            GraphMaster.Instance.SetSelectedUnit(null);
            // 将 Content 放置前面进行截图
            RectTransformUtils.SetParentAndAlign(Viewer.content.gameObject, Viewer.transform.parent.gameObject);
            Viewer.content.transform.SetAsLastSibling();
            // 调整缩放
            Viewer.Scale = 1f;
            // 调整位置
            Viewer.AdjustPagePosition();

            //RectTransformUtils.AnchorToWorldPoint(Viewer.Page, new Vector2(0, 1), out Vector3 pageUpperLeftWorldPoint);
            //RectTransformUtils.AnchorToWorldPoint(Viewer.Page, new Vector2(1, 1), out Vector3 pageUpperRightWorldPoint);
            //Vector2 pageUpperLeftScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pageUpperLeftWorldPoint);
            //Vector2 pageUpperRightScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, pageUpperRightWorldPoint);

            //RectTransformUtils.AnchorToWorldPoint(Viewer.viewport, new Vector2(0, 0), out Vector3 viewportUpperLeftWorldPoint);
            //RectTransformUtils.AnchorToWorldPoint(Viewer.viewport, new Vector2(0, 1), out Vector3 viewportUpperRightWorldPoint);
            //Vector2 viewportLowerLeftScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, viewportUpperLeftWorldPoint);
            //Vector2 viewportUpperLeftScreenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, viewportUpperRightWorldPoint);

            Vector2 pageUpperLeftScreenPoint = RectTransformUtils.AnchorToScreenPoint(Camera.main, Viewer.Page, new Vector2(0, 1));
            Vector2 pageUpperRightScreenPoint = RectTransformUtils.AnchorToScreenPoint(Camera.main, Viewer.Page, new Vector2(1, 1));
            Vector2 viewportLowerLeftScreenPoint = RectTransformUtils.AnchorToScreenPoint(Camera.main, Viewer.viewport, new Vector2(0, 0));
            Vector2 viewportUpperLeftScreenPoint = RectTransformUtils.AnchorToScreenPoint(Camera.main, Viewer.viewport, new Vector2(0, 1));

            // 截图 Rect
            Rect shotRect = new Rect(pageUpperLeftScreenPoint.x, viewportLowerLeftScreenPoint.y,
                pageUpperRightScreenPoint.x - pageUpperLeftScreenPoint.x, 
                viewportUpperLeftScreenPoint.y - viewportLowerLeftScreenPoint.y);

            // 页面的高度 / 视口的高度
            float multiple = Viewer.Page.rect.height / Viewer.viewport.rect.height;
            // 需要几步截图
            int floorToInt = Mathf.FloorToInt(multiple);
            float remainder = (multiple - floorToInt) * Viewer.viewport.rect.height;
            int stepCount = floorToInt + 1;

            // 截图
            List<Texture2D> textures = new List<Texture2D>();
            for (int i = 0; i < stepCount; i++)
            {
                yield return new WaitForEndOfFrame();
                var tex = Screenshot(shotRect);
                textures.Add(tex);

                if (i < stepCount - 2)
                {
                    Viewer.content.localPosition += new Vector3(0, Viewer.viewport.rect.height, 0);
                }
                else if (i == stepCount - 2)
                {
                    Viewer.content.localPosition += new Vector3(0, remainder, 0);
                    shotRect.height = remainder;
                }
            }

            // 拼接图片
            texture = MergeTextures(textures);

            // 重置
            GraphMaster.Instance.SetSelectedUnit(currentUnit);
            RectTransformUtils.SetParentAndAlign(Viewer.content.gameObject, Viewer.viewport.gameObject);
            Viewer.content.localPosition = contentLocalPosition;
            Viewer.Scale = viewerScale;
        }

        //int number = 1;

        /// <summary>
        /// 截图
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        private Texture2D Screenshot(Rect rect)
        {
            // 读取像素
            Texture2D texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
            texture.ReadPixels(rect, 0, 0);
            texture.Apply();

            // 测试
            //byte[] bytes = texture.EncodeToPNG();
            //System.IO.File.WriteAllBytes(number + ".png", bytes);
            //number++;

            return texture;
        }

        /// <summary>
        /// 拼接图片
        /// </summary>
        /// <param name="textures"></param>
        public Texture2D MergeTextures(List<Texture2D> textures)
        {
            int width = 0, height = 0;
            for (int i = 0; i < textures.Count; i++)
            {
                height += textures[i].height;
                if (width < textures[i].width)
                {
                    width = textures[i].width;
                }
            }

            // 初始化 Texture2D
            Texture2D texture = new Texture2D(width, height);
            int x = 0, y = 0;
            for (int i = textures.Count - 1; i >= 0; i--)
            {
                // 取图
                Color32[] colors = textures[i].GetPixels32(0);
                texture.SetPixels32(x, y, textures[i].width, textures[i].height, colors);
                y += textures[i].height;
            }

            texture.Apply();

            return texture;

            // 测试
            //byte[] bytes = texture.EncodeToPNG();
            //System.IO.File.WriteAllBytes(number + ".png", bytes);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartCoroutine(Capture());
            }
        }
    }
}

