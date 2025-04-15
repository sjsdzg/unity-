using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace XFramework.UIWidgets
{
    public class UIText : Text
    {
        public static readonly string no_breaking_space = "\u00A0";

        private string m_CacheText;

        public override void SetVerticesDirty()
        {
            base.SetVerticesDirty();
            UpdateCacheText();
        }

        public override void SetLayoutDirty()
        {
            base.SetLayoutDirty();
            UpdateCacheText();
        }

        public void UpdateCacheText()
        {
            m_CacheText = text;
            m_CacheText = m_CacheText.Replace("\\n", "\n");
            m_CacheText = m_CacheText.Replace(" ", no_breaking_space);

            var generator = new TextGenerator();
            var settings = GetGenerationSettings(rectTransform.rect.size);
            generator.Populate(m_CacheText, settings);

            var characterCountVisible = generator.characterCountVisible;
            if (m_CacheText.Length > characterCountVisible && characterCountVisible > 0)
            {
                m_CacheText = m_CacheText.Substring(0, characterCountVisible - 1);
                m_CacheText += "...";
            }
        }

        readonly UIVertex[] m_CacheVerts = new UIVertex[4];

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            if (font == null)
                return;

            // We don't care if we the font Texture changes while we are doing our Update.
            // The end result of cachedTextGenerator will be valid for this instance.
            // Otherwise we can get issues like Case 619238.
            m_DisableFontTextureRebuiltCallback = true;

            Vector2 extents = rectTransform.rect.size;

            var settings = GetGenerationSettings(extents);
            cachedTextGenerator.PopulateWithErrors(m_CacheText, settings, gameObject);

            // Apply the offset to the vertices
            IList<UIVertex> verts = cachedTextGenerator.verts;
            float unitsPerPixel = 1 / pixelsPerUnit;
            //Last 4 verts are always a new line... (\n)
            int vertCount = verts.Count - 4;

            Vector2 roundingOffset = new Vector2(verts[0].position.x, verts[0].position.y) * unitsPerPixel;
            roundingOffset = PixelAdjustPoint(roundingOffset) - roundingOffset;
            toFill.Clear();
            if (roundingOffset != Vector2.zero)
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    m_CacheVerts[tempVertsIndex] = verts[i];
                    m_CacheVerts[tempVertsIndex].position *= unitsPerPixel;
                    m_CacheVerts[tempVertsIndex].position.x += roundingOffset.x;
                    m_CacheVerts[tempVertsIndex].position.y += roundingOffset.y;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_CacheVerts);
                }
            }
            else
            {
                for (int i = 0; i < vertCount; ++i)
                {
                    int tempVertsIndex = i & 3;
                    m_CacheVerts[tempVertsIndex] = verts[i];
                    m_CacheVerts[tempVertsIndex].position *= unitsPerPixel;
                    if (tempVertsIndex == 3)
                        toFill.AddUIVertexQuad(m_CacheVerts);
                }
            }

            m_DisableFontTextureRebuiltCallback = false;
        }


#if UNITY_EDITOR
        public override void OnRebuildRequested()
        {
            UpdateCacheText();
            base.OnRebuildRequested();
        }

        protected override void OnValidate()
        {
            UpdateCacheText();
            base.OnValidate();
        }
#endif
    }
}

