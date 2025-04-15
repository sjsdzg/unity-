using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace XFramework.Editor
{
    public class WebGLSettingScript : ScriptableObject
    {
        private static bool isCleanupMissingScripts = false;
        private static bool isReplaceTextFont = false;
        private static bool isReplaceTMPFont = false;
        private static bool isAddWebGLInput = false;

        private static Font toFont;
        private static TMP_FontAsset toFontAsset;

        public static void Draw()
        {
            isCleanupMissingScripts = EditorGUILayout.Toggle("清理 Missing Script", isCleanupMissingScripts);

            isReplaceTextFont = EditorGUILayout.Toggle("是否替换Text字体", isReplaceTextFont);
            if (isReplaceTextFont)
                toFont = (Font)EditorGUILayout.ObjectField("替换字体：", toFont, typeof(Font), true);

            isReplaceTMPFont = EditorGUILayout.Toggle("是否替换TMP字体", isReplaceTMPFont);
            if (isReplaceTMPFont)
                toFontAsset = (TMP_FontAsset)EditorGUILayout.ObjectField("替换TMP字体：", toFontAsset, typeof(TMP_FontAsset), true);

            isAddWebGLInput = EditorGUILayout.Toggle("是否添加WebGLInput", isAddWebGLInput);

            EditorGUILayout.Space();
            if (GUILayout.Button("一键设置"))
                QuickSetting();
        }

        /// <summary>
        /// WebGL打包设置
        /// </summary>
        private static void QuickSetting()
        {
            //toFontStyle = FontStyle.None;
            //string symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL);
            //symbols += ";"
            if (isCleanupMissingScripts)
            {
                CleanupMissingScript.CleanupMissingScriptsForAll();
            }

            if (isReplaceTextFont)
            {
                TextProperties properties = new TextProperties();
                properties.Font = toFont;
                properties.FontStyle = FontStyle.None;
                ModifyTextScript.ModifyTextForAll(properties);
            }

            if (isReplaceTMPFont)
            {
                TMPProperties properties = new TMPProperties();
                properties.FontAsset = toFontAsset;
                ModifyTMPScript.ModifyTMPAssetForAll(properties);
            }

            if (isAddWebGLInput)
            {
                AddWebGLInputScript.AddWebGLInputForAll();
            }
        }
    }
}
