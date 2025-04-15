using UnityEngine;
using UnityEditor;

namespace XFramework.Editor
{
    public enum FontStyle
    {
        None,
        //
        // 摘要:
        //     No special style is applied.
        Normal,
        //
        // 摘要:
        //     Bold style applied to your texts.
        Bold,
        //
        // 摘要:
        //     Italic style applied to your texts.
        Italic,
        //
        // 摘要:
        //     Bold and Italic styles applied to your texts.
        BoldAndItalic
    }


    public enum ApplyMode
    {
        Selection,
        All,
    }
}
