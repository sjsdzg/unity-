#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Paroxe.PdfRenderer.Internal.Viewer
{
    [ExecuteInEditMode]
    public class PDFViewerBuilder : UIBehaviour
    {
#if UNITY_EDITOR
        public static void BuildPDFViewerWithin(GameObject root, PDFViewer prefabViewer)
        {
            if (root.transform.childCount > 0)
            {
                int c = root.transform.childCount;
                for (int i = 0; i < c; ++i)
                {
                    Destroy(root.transform.GetChild(0).gameObject);
                }
            }

            PDFViewer viewer = root.GetComponent<PDFViewer>();

            if (viewer == null)
            {
                viewer = root.AddComponent<PDFViewer>();
            }

            if (prefabViewer != null)
            {
                CopyViewer(prefabViewer, viewer);
            }

            RectTransform rootTransform = root.GetComponent<RectTransform>();
            if (rootTransform == null)
            {
                rootTransform = root.AddComponent<RectTransform>();
                rootTransform.anchorMin = Vector2.zero;
                rootTransform.anchorMax = Vector2.one;
                rootTransform.offsetMin = Vector2.zero;
                rootTransform.offsetMax = Vector2.zero;
            }

            GameObject internalPrefab = (GameObject) AssetDatabase.LoadAssetAtPath(
                AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("PDFViewer_Internal")[0]), typeof (GameObject));

            GameObject viewerInternal = (GameObject) Instantiate(internalPrefab);
            PrefabUtility.DisconnectPrefabInstance(viewerInternal);

            viewerInternal.GetComponent<PDFViewerInternal>().m_PDFViewer = root.GetComponent<PDFViewer>();
            viewer.m_Internal = viewerInternal.GetComponent<PDFViewerInternal>();
            viewerInternal.name = "_Internal";
            RectTransform internalRectTransform = viewerInternal.transform as RectTransform;
            internalRectTransform.SetParent(viewer.transform, false);
            internalRectTransform.anchorMin = Vector2.zero;
            internalRectTransform.anchorMax = Vector2.one;
            internalRectTransform.offsetMin = Vector2.zero;
            internalRectTransform.offsetMax = Vector2.zero;

            Selection.activeGameObject = root;
        }

        private static void CopyViewer(PDFViewer prefabViewer, PDFViewer viewer)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                 BindingFlags.Default | BindingFlags.DeclaredOnly;
            PropertyInfo[] pinfos = typeof (PDFViewer).GetProperties(flags);
            foreach (var pinfo in pinfos)
            {
                if (pinfo.CanWrite)
                {
                    try
                    {
                        pinfo.SetValue(viewer, pinfo.GetValue(prefabViewer, null), null);
                    }
                    catch
                    {
                    }
                }
            }
            FieldInfo[] finfos = typeof (PDFViewer).GetFields(flags);
            foreach (var finfo in finfos)
            {
                finfo.SetValue(viewer, finfo.GetValue(prefabViewer));
            }
        }

        protected override void Start()
        {
            GameObject pdfViewer = new GameObject("PDFViewer");
            RectTransform rootTransform = pdfViewer.AddComponent<RectTransform>();
            rootTransform.SetParent(transform.parent, false);
            rootTransform.anchorMin = Vector2.zero;
            rootTransform.anchorMax = Vector2.one;
            rootTransform.offsetMin = Vector2.zero;
            rootTransform.offsetMax = Vector2.zero;

            BuildPDFViewerWithin(pdfViewer, GetComponent<PDFViewer>());

            DestroyImmediate(gameObject);
        }

#endif
    }
}