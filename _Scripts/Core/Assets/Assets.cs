using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using XFramework.Core;
using UnityEngine.Events;
using System.Linq;

/*  The AssetBundle Manager provides a High-Level API for working with AssetBundles. 
    The AssetBundle Manager will take care of loading AssetBundles and their associated 
    Asset Dependencies.
        Initialize()
            Initializes the AssetBundle manifest object.
        LoadAssetAsync()
            Loads a given asset from a given AssetBundle and handles all the dependencies.
        LoadLevelAsync()
            Loads a given scene from a given AssetBundle and handles all the dependencies.
        LoadDependencies()
            Loads all the dependent AssetBundles for a given AssetBundle.
        BaseDownloadingURL
            Sets the base downloading url which is used for automatic downloading dependencies.
        SimulateAssetBundleInEditor
            Sets Simulation Mode in the Editor.
        Variants
            Sets the active variant.
        RemapVariantName()
            Resolves the correct AssetBundle according to the active variant.
*/

namespace XFramework.Core
{
    /// <summary>
    /// Loaded assetBundle contains the references count which can be used to
    /// unload dependent assetBundles automatically.
    /// </summary>
    public class LoadedAssetBundle
    {
        public AssetBundle m_AssetBundle;
        public int m_ReferencedCount;
        public LoadOptions LoadedOption;

        internal event Action unload;

        internal void OnUnload()
        {
            m_AssetBundle.Unload(false);
            if (unload != null)
                unload();
        }

        public object[] LoadAllAssets()
        {
            if (m_AssetBundle != null)
                return m_AssetBundle.LoadAllAssets();

            return null;
        }

        public LoadedAssetBundle(AssetBundle assetBundle, LoadOptions LoadedOption)
        {
            m_AssetBundle = assetBundle;
            switch (LoadedOption)
            {
                case LoadOptions.None:
                    m_ReferencedCount = 0;
                    break;
                case LoadOptions.Preload:
                    m_ReferencedCount = 0;
                    break;
                case LoadOptions.Lazyload:
                    m_ReferencedCount = 1;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Class takes care of loading assetBundle and its dependencies
    /// automatically, loading variants automatically.
    /// </summary>
    public class Assets : MonoBehaviour
    {
        public enum LogMode { All, JustErrors };
        public enum LogType { Info, Warning, Error };

        static LogMode m_LogMode = LogMode.All;
        static string m_BaseDownloadingURL = "";
        static string[] m_ActiveVariants =  {};
        static AssetBundleManifest m_AssetBundleManifest = null;
        public static bool initialized = false;//�Ƿ��ʼ��
#if UNITY_EDITOR
        static int m_SimulateAssetBundleInEditor = -1;
        const string kSimulateAssetBundles = "SimulateAssetBundles";
#endif

        static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();
        static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();
        static List<string> m_DownloadingBundles = new List<string>();
        static List<AsyncLoadOperation> m_InProgressOperations = new List<AsyncLoadOperation>();
        static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

        public static LogMode logMode
        {
            get { return m_LogMode; }
            set { m_LogMode = value; }
        }

        /// <summary>
        /// The base downloading url which is used to generate the full
        /// downloading url with the assetBundle names.
        /// </summary>
        public static string BaseDownloadingURL
        {
            get { return m_BaseDownloadingURL; }
            set { m_BaseDownloadingURL = value; }
        }

        public delegate string OverrideBaseDownloadingURLDelegate(string bundleName);

        /// <summary>
        /// Implements per-bundle base downloading URL override.
        /// The subscribers must return null values for unknown bundle names;
        /// </summary>
        public static event OverrideBaseDownloadingURLDelegate overrideBaseDownloadingURL;

        /// <summary>
        /// Variants which is used to define the active variants.
        /// </summary>
        public static string[] ActiveVariants
        {
            get { return m_ActiveVariants; }
            set { m_ActiveVariants = value; }
        }

        /// <summary>
        /// AssetBundleManifest object which can be used to load the dependecies
        /// and check suitable assetBundle variants.
        /// </summary>
        public static AssetBundleManifest AssetBundleManifestObject
        {
            set {m_AssetBundleManifest = value; }
        }

        private static void Log(LogType logType, string text)
        {
            if (logType == LogType.Error)
                Debug.LogError("[Assets] " + text);
            else if (m_LogMode == LogMode.All && logType == LogType.Warning)
                Debug.LogWarning("[Assets] " + text);
            else if (m_LogMode == LogMode.All)
                Debug.Log("[Assets] " + text);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Flag to indicate if we want to simulate assetBundles in Editor without building them actually.
        /// </summary>
        public static bool SimulateAssetBundleInEditor
        {
            get
            {
                if (m_SimulateAssetBundleInEditor == -1)
                    m_SimulateAssetBundleInEditor = EditorPrefs.GetBool(kSimulateAssetBundles, true) ? 1 : 0;

                return m_SimulateAssetBundleInEditor != 0;
            }
            set
            {
                int newValue = value ? 1 : 0;
                if (newValue != m_SimulateAssetBundleInEditor)
                {
                    m_SimulateAssetBundleInEditor = newValue;
                    EditorPrefs.SetBool(kSimulateAssetBundles, value);
                }
            }
        }
#endif

        private static string GetStreamingAssetsPath()
        {
            if (Application.isEditor)
                return "file://" +  System.Environment.CurrentDirectory.Replace("\\", "/"); // Use the build output folder directly.
            else if (Application.isMobilePlatform || Application.isConsolePlatform)
                return Application.streamingAssetsPath;
            else // For standalone player.
                return "file://" +  Application.streamingAssetsPath;
        }

        /// <summary>
        /// Sets base downloading URL to a directory relative to the streaming assets directory.
        /// Asset bundles are loaded from a local directory.
        /// </summary>
        public static void SetSourceAssetBundleDirectory(string relativePath)
        {
            BaseDownloadingURL = GetStreamingAssetsPath() + relativePath;
        }

        /// <summary>
        /// Sets base downloading URL to a web URL. The directory pointed to by this URL
        /// on the web-server should have the same structure as the AssetBundles directory
        /// in the demo project root.
        /// </summary>
        /// <example>For example, AssetBundles/iOS/xyz-scene must map to
        /// absolutePath/iOS/xyz-scene.
        /// <example>
        public static void SetSourceAssetBundleURL(string absolutePath)
        {
            if (!absolutePath.EndsWith("/"))
            {
                absolutePath += "/";
            }

            BaseDownloadingURL = absolutePath + Utility.AssetBundlesOutputPath + "/" + Utility.GetPlatformName() + "/";
        }

        /// <summary>
        /// ������Դ������Url
        /// </summary>
        public static void SetAssetServer()
        {
//#if UNITY_EDITOR
//            // If we're in Editor simulation mode, we don't have to setup a download URL
//            if (SimulateAssetBundleInEditor)
//                return;
//#endif
            //��Դ������Url
            string assetServerUrl = AppSettings.Settings.AssetServerUrl;

            if (assetServerUrl.StartsWith("http://"))
            {
                SetSourceAssetBundleURL(assetServerUrl);
            }
            else if (assetServerUrl.StartsWith("file://"))
            {
                //AppSettings.Settings.AssetServerUrl = "file://" + Application.streamingAssetsPath + "/";
                AppSettings.Settings.AssetServerUrl = Application.streamingAssetsPath + "/";
                string relativePath = "/" + Utility.AssetBundlesOutputPath + "/" + Utility.GetPlatformName() + "/";
                SetSourceAssetBundleDirectory(relativePath);
            }
            else
            {
                Log(LogType.Error, "Development Server URL could not be found.");
            }
        }

        /// <summary>
        /// Retrieves an asset bundle that has previously been requested via LoadAssetBundle.
        /// Returns null if the asset bundle or one of its dependencies have not been downloaded yet.
        /// </summary>
        static public LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
        {
            if (m_DownloadingErrors.TryGetValue(assetBundleName, out error))
                return null;

            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle == null)
                return null;

            // No dependencies are recorded, only the bundle itself is required.
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return bundle;

            // Make sure all dependencies are loaded
            foreach (var dependency in dependencies)
            {
                if (m_DownloadingErrors.TryGetValue(dependency, out error))
                    return null;

                // Wait all the dependent assetBundles being loaded.
                LoadedAssetBundle dependentBundle;
                m_LoadedAssetBundles.TryGetValue(dependency, out dependentBundle);
                if (dependentBundle == null)
                    return null;
            }

            return bundle;
        }

        /// <summary>
        /// Returns true if certain asset bundle has been downloaded without checking
        /// whether the dependencies have been loaded.
        /// </summary>
        static public bool IsAssetBundleDownloaded(string assetBundleName)
        {
            return m_LoadedAssetBundles.ContainsKey(assetBundleName);
        }

        /// <summary>
        /// Initializes asset bundle namager and starts download of manifest asset bundle.
        /// Returns the manifest asset bundle downolad operation object.
        /// </summary>
        static public AssetBundleLoadManifestOperation Initialize()
        {
            if (initialized)
            {
                Log(LogType.Warning, "Assets has already been Initialized");
                return null;
            }

            initialized = true;
            SetAssetServer();
            return Initialize(Utility.GetPlatformName());
        }

        /// <summary>
        /// Initializes asset bundle namager and starts download of manifest asset bundle.
        /// Returns the manifest asset bundle downolad operation object.
        /// </summary>
        static public AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
        {
#if UNITY_EDITOR
            Log(LogType.Info, "Simulation Mode: " + (SimulateAssetBundleInEditor ? "Enabled" : "Disabled"));
#endif

            var go = new GameObject("[Assets]", typeof(Assets));
            DontDestroyOnLoad(go);

#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't need the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return null;
#endif

            LoadAssetBundle(manifestAssetBundleName, true);
            var operation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
            m_InProgressOperations.Add(operation);
            return operation;
        }

        // Temporarily work around a il2cpp bug
        static protected void LoadAssetBundle(string assetBundleName)
        {
            LoadAssetBundle(assetBundleName, false);
        }
            
        // Starts the download of the asset bundle identified by the given name, and asset bundles
        // that this asset bundle depends on.
        static protected void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            Log(LogType.Info, "Loading Asset Bundle " + (isLoadingAssetBundleManifest ? "Manifest: " : ": ") + assetBundleName);

#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
                return;
#endif

            if (!isLoadingAssetBundleManifest)
            {
                if (m_AssetBundleManifest == null)
                {
                    Log(LogType.Error, "Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                    return;
                }
            }

            // Check if the assetBundle has already been processed.
            bool isAlreadyProcessed = LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest);

            // Load dependencies.
            if (!isAlreadyProcessed && !isLoadingAssetBundleManifest)
                LoadDependencies(assetBundleName);
        }

        // Returns base downloading URL for the given asset bundle.
        // This URL may be overridden on per-bundle basis via overrideBaseDownloadingURL event.
        protected static string GetAssetBundleBaseDownloadingURL(string bundleName)
        {
            if (overrideBaseDownloadingURL != null)
            {
                foreach (OverrideBaseDownloadingURLDelegate method in overrideBaseDownloadingURL.GetInvocationList())
                {
                    string res = method(bundleName);
                    if (res != null)
                        return res;
                }
            }
            return m_BaseDownloadingURL;
        }

        // Checks who is responsible for determination of the correct asset bundle variant
        // that should be loaded on this platform. 
        //
        // On most platforms, this is done by the AssetBundleManager itself. However, on
        // certain platforms (iOS at the moment) it's possible that an external asset bundle
        // variant resolution mechanism is used. In these cases, we use base asset bundle 
        // name (without the variant tag) as the bundle identifier. The platform-specific 
        // code is responsible for correctly loading the bundle.
        static protected bool UsesExternalBundleVariantResolutionMechanism(string baseAssetBundleName)
        {
#if ENABLE_IOS_APP_SLICING
            var url = GetAssetBundleBaseDownloadingURL(baseAssetBundleName);
            if (url.ToLower().StartsWith("res://") ||
                url.ToLower().StartsWith("odr://"))
                return true;
#endif
            return false;
        }

        // Remaps the asset bundle name to the best fitting asset bundle variant.
        static protected string RemapVariantName(string assetBundleName)
        {
            string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

            // Get base bundle name
            string baseName = assetBundleName.Split('.')[0];

            if (UsesExternalBundleVariantResolutionMechanism(baseName))
                return baseName;

            int bestFit = int.MaxValue;
            int bestFitIndex = -1;
            // Loop all the assetBundles with variant to find the best fit variant assetBundle.
            for (int i = 0; i < bundlesWithVariant.Length; i++)
            {
                string[] curSplit = bundlesWithVariant[i].Split('.');
                string curBaseName = curSplit[0];
                string curVariant = curSplit[1];

                if (curBaseName != baseName)
                    continue;

                int found = System.Array.IndexOf(m_ActiveVariants, curVariant);

                // If there is no active variant found. We still want to use the first
                if (found == -1)
                    found = int.MaxValue - 1;

                if (found < bestFit)
                {
                    bestFit = found;
                    bestFitIndex = i;
                }
            }

            if (bestFit == int.MaxValue - 1)
            {
                Log(LogType.Warning, "Ambigious asset bundle variant chosen because there was no matching active variant: " + bundlesWithVariant[bestFitIndex]);
            }

            if (bestFitIndex != -1)
            {
                return bundlesWithVariant[bestFitIndex];
            }
            else
            {
                return assetBundleName;
            }
        }

        // Sets up download operation for the given asset bundle if it's not downloaded already.
        static protected bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
        {
            // Already loaded.
            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return true;
            }

            // @TODO: Do we need to consider the referenced count of WWWs?
            // In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
            // But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
            if (m_DownloadingBundles.Contains(assetBundleName))
                return true;

            string bundleBaseDownloadingURL = GetAssetBundleBaseDownloadingURL(assetBundleName);

            if (bundleBaseDownloadingURL.ToLower().StartsWith("odr://"))
            {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
                Log(LogType.Info, "Requesting bundle " + assetBundleName + " through ODR");
                m_InProgressOperations.Add(new AssetBundleDownloadFromODROperation(assetBundleName));
#else
                new ApplicationException("Can't load bundle " + assetBundleName + " through ODR: this Unity version or build target doesn't support it.");
#endif
            }
            else if (bundleBaseDownloadingURL.ToLower().StartsWith("res://"))
            {
#if ENABLE_IOS_APP_SLICING
                Log(LogType.Info, "Requesting bundle " + assetBundleName + " through asset catalog");
                m_InProgressOperations.Add(new AssetBundleOpenFromAssetCatalogOperation(assetBundleName));
#else
                new ApplicationException("Can't load bundle " + assetBundleName + " through asset catalog: this Unity version or build target doesn't support it.");
#endif
            }
            else
            {
                UnityWebRequest request = null;

                if (!bundleBaseDownloadingURL.EndsWith("/"))
                {
                    bundleBaseDownloadingURL += "/";
                }

                string url = bundleBaseDownloadingURL + assetBundleName;

                // For manifest assetbundle, always download it as we don't have hash for it.
                if (isLoadingAssetBundleManifest)
                    request = UnityWebRequestAssetBundle.GetAssetBundle(url);
                else
                    request = UnityWebRequestAssetBundle.GetAssetBundle(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0);

                m_InProgressOperations.Add(new AssetBundleLoadFromWebOperation(assetBundleName, request));
            }
            m_DownloadingBundles.Add(assetBundleName);

            return false;
        }

        // Where we get all the dependencies and load them all.
        static protected void LoadDependencies(string assetBundleName)
        {
            if (m_AssetBundleManifest == null)
            {
                Log(LogType.Error, "Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return;
            }

            // Get dependecies from the AssetBundleManifest object..
            string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
            if (dependencies.Length == 0)
                return;

            for (int i = 0; i < dependencies.Length; i++)
                dependencies[i] = RemapVariantName(dependencies[i]);

            // Record and load all dependencies.
            m_Dependencies.Add(assetBundleName, dependencies);
            for (int i = 0; i < dependencies.Length; i++)
                LoadAssetBundleInternal(dependencies[i], false);
        }

        static public void UnloadAll()
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return;
#endif
            string[] assetBundleNames = new string[m_LoadedAssetBundles.Count];
            m_LoadedAssetBundles.Keys.CopyTo(assetBundleNames, 0);
            foreach (var assetBundleName in assetBundleNames)
            {
                UnloadAssetBundle(assetBundleName);
            }
        }

        /// <summary>
        /// Unloads assetbundle and its dependencies.
        /// </summary>
        static public void UnloadAssetBundle(string assetBundleName)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to load the manifest assetBundle.
            if (SimulateAssetBundleInEditor)
                return;
#endif
            assetBundleName = RemapVariantName(assetBundleName);

            UnloadAssetBundleInternal(assetBundleName);
            UnloadDependencies(assetBundleName);
        }

        static protected void UnloadDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
                return;

            // Loop dependencies.
            foreach (var dependency in dependencies)
            {
                UnloadAssetBundleInternal(dependency);
            }

            m_Dependencies.Remove(assetBundleName);
        }

        static protected void UnloadAssetBundleInternal(string assetBundleName)
        {
            string error;
            LoadedAssetBundle bundle = GetLoadedAssetBundle(assetBundleName, out error);
            if (bundle == null)
                return;

            if (--bundle.m_ReferencedCount <= 0) // 0, -1
            {
                bundle.OnUnload();
                m_LoadedAssetBundles.Remove(assetBundleName);

                Log(LogType.Info, assetBundleName + " has been unloaded successfully");
            }
        }

        void Update()
        {
            // Update all in progress operations
            for (int i = 0; i < m_InProgressOperations.Count;)
            {
                var operation = m_InProgressOperations[i];
                if (operation.Update())
                {
                    i++;
                }
                else
                {
                    m_InProgressOperations.RemoveAt(i);
                    ProcessFinishedOperation(operation);
                }
            }
        }

        void ProcessFinishedOperation(AsyncLoadOperation operation)
        {
            AssetBundleLoadOperation download = operation as AssetBundleLoadOperation;
            if (download == null)
                return;

            if (string.IsNullOrEmpty(download.Error))
            {
                switch (download.LoadOption)
                {
                    case LoadOptions.None:
                        //download.assetBundle.m_AssetBundle.Unload(true);
                        break;
                    case LoadOptions.Preload:
                        m_LoadedAssetBundles.Add(download.assetBundleName, download.assetBundle);
                        break;
                    case LoadOptions.Lazyload:
                        m_LoadedAssetBundles.Add(download.assetBundleName, download.assetBundle);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                string msg = string.Format("Failed downloading bundle {0} from {1}: {2}",
                        download.assetBundleName, download.GetSourceURL(), download.Error);
                m_DownloadingErrors.Add(download.assetBundleName, msg);
            }

            m_DownloadingBundles.Remove(download.assetBundleName);
        }

        /// <summary>
        /// �˷����ʺ�����ǰ����AssetBundle��Ԥ����AssetBundleʹ�ã����ᴦ��AssetBundle������
        /// ����������ܻ᷵��null��
        /// 1.�༭���£������˷���ģʽ
        /// 2.AssetBundleManifest δ����
        /// 3.AssetBundle������
        /// 4.AssetBundle��������
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        static public AssetBundleLoadOperation LoadAssetBundleExternal(string assetBundleName, LoadOptions loadedOption = LoadOptions.Preload)
        {
            assetBundleName = assetBundleName.ToLower();
            Log(LogType.Info, "downloading Asset Bundle :" + assetBundleName);

#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
            {
                //Log(LogType.Error, "we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.");
                return null;
            }   
#endif
            assetBundleName = RemapVariantName(assetBundleName);

            AssetBundleLoadOperation operation = null;

            if (m_AssetBundleManifest == null)
            {
                Log(LogType.Error, "Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
                return null;
            }

            // Already loaded.
            LoadedAssetBundle bundle = null;
            m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle);
            if (bundle != null)
            {
                bundle.m_ReferencedCount++;
                return null;
            }

            // @TODO: Do we need to consider the referenced count of WWWs?
            // In the demo, we never have duplicate WWWs as we wait LoadAssetAsync()/LoadLevelAsync() to be finished before calling another LoadAssetAsync()/LoadLevelAsync().
            // But in the real case, users can call LoadAssetAsync()/LoadLevelAsync() several times then wait them to be finished which might have duplicate WWWs.
            if (m_DownloadingBundles.Contains(assetBundleName))
                return null;

            string bundleBaseDownloadingURL = GetAssetBundleBaseDownloadingURL(assetBundleName);

            UnityWebRequest request = null;

            if (!bundleBaseDownloadingURL.EndsWith("/"))
            {
                bundleBaseDownloadingURL += "/";
            }

            string url = bundleBaseDownloadingURL + assetBundleName;

            request = UnityWebRequestAssetBundle.GetAssetBundle(url, m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0);

            operation = new AssetBundleLoadFromWebOperation(assetBundleName, request, loadedOption);
            m_InProgressOperations.Add(operation);
            m_DownloadingBundles.Add(assetBundleName);

            return operation;
        }

        /// <summary>
        /// Starts a load operation for an asset from the given asset bundle.
        /// </summary>
        static public AsyncLoadAssetOperation LoadAssetAsync<T>(string assetBundleName, string assetName) where T : UnityEngine.Object
        {
            assetBundleName = assetBundleName.ToLower();
            Log(LogType.Info, "Loading " + assetName + " from " + assetBundleName + " bundle");

            AsyncLoadAssetOperation operation = null;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                string[] strs = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
                string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);

                UnityEngine.Object target = null;
                if (assetPaths.Length == 0)
                {
                    Log(LogType.Error, "There is no asset with name \"" + assetName + "\" in " + assetBundleName);
                    return null;
                }
                else if (assetPaths.Length == 1)
                {
                    target = AssetDatabase.LoadAssetAtPath<T>(assetPaths[0]);
                }
                else
                {
                    target = AssetDatabase.LoadMainAssetAtPath(assetPaths[0]);
                }

                // @TODO: Now we only get the main object from the first asset. Should consider type also.
                
                operation = new AssetBundleLoadAssetOperationSimulation(target);
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadAssetOperation(assetBundleName, assetName, typeof(T));
            }

            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// �첽������Դ
        /// 1.path��Asset���ƶ�Ӧ
        /// 2.һ��Asset��Ӧһ��AssetBundle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AsyncLoadAssetOperation LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            string assetBundleName = path;
            string sceneName = path.Substring(path.LastIndexOf('/') + 1);
            return LoadAssetAsync<T>(assetBundleName, sceneName);
        }

        /// <summary>
        /// �첽���س���
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="sceneName"></param>
        /// <param name="isAdditive"></param>
        /// <returns></returns>
        static public AsyncLoadOperation LoadSceneAsync(string assetBundleName, string sceneName, bool isAdditive)
        {
            assetBundleName = assetBundleName.ToLower();
            Log(LogType.Info, "Loading " + sceneName + " from " + assetBundleName + " bundle");

            AsyncLoadOperation operation = null;
#if UNITY_EDITOR
            if (SimulateAssetBundleInEditor)
            {
                operation = new AssetBundleLoadSceneSimulationOperation(assetBundleName, sceneName, isAdditive);
            }
            else
#endif
            {
                assetBundleName = RemapVariantName(assetBundleName);
                LoadAssetBundle(assetBundleName);
                operation = new AssetBundleLoadSceneOperation(assetBundleName, sceneName, isAdditive);
            }
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// �첽���س���
        /// 1.path��Asset���ƶ�Ӧ
        /// 2.һ��Asset��Ӧһ��AssetBundle
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isAdditive"></param>
        /// <returns></returns>
        public static AsyncLoadOperation LoadSceneAsync(string path, bool isAdditive)
        {
            string assetBundleName = path;
            string sceneName = path.Substring(path.LastIndexOf('/') + 1);
            return LoadSceneAsync(assetBundleName, sceneName, isAdditive);
        }

        /// <summary>
        /// �첽����AssetBundles �Զ���������
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static AssetBundlePreloadOperation LoadAssetBundleAsync(string[] paths)
        {
#if UNITY_EDITOR
            // If we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.
            if (SimulateAssetBundleInEditor)
            {
                //Log(LogType.Error, "we're in Editor simulation mode, we don't have to really load the assetBundle and its dependencies.");
                return null;
            }
#endif
            paths = paths.Distinct().ToArray();//ȥ��
            List<string> assetBundleNames = new List<string>();
            foreach (var path in paths)
            {
                string assetBundleName = path.ToLower();
                //��������
                string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
                for (int i = 0; i < dependencies.Length; i++)
                    dependencies[i] = RemapVariantName(dependencies[i]);

                // Record and load all dependencies.
                if (dependencies.Length > 0 && !m_Dependencies.ContainsKey(assetBundleName))
                {
                    //if (m_Dependencies.ContainsKey(assetBundleName))
                    //    Debug.LogWarning("[Assets] m_Dependencies already exists the key : " + assetBundleName);
                    //else
                    //    m_Dependencies.Add(assetBundleName, dependencies);
                    m_Dependencies.Add(assetBundleName, dependencies);
                }

                //�������·��
                assetBundleNames.Add(assetBundleName);
                assetBundleNames.AddRange(dependencies);
            }

            AssetBundlePreloadOperation operation = new AssetBundlePreloadOperation(assetBundleNames);
            m_InProgressOperations.Add(operation);
            return operation;
        }

        /// <summary>
        /// �첽����AssetBundle �Զ���������
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static AssetBundlePreloadOperation LoadAssetBundleAsync(string path)
        {
            return LoadAssetBundleAsync(new string[] { path });
        }

    } 
}
