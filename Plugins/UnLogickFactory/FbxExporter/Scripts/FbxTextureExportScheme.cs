using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnLogickFactory
{
	/// <summary>
	/// This class allows you to set up an Fbx Exporter Texture Export scheme
	/// 
	/// It is kept as a separate class to allow you to derive from it and replace it with your own logic.
	/// </summary>
	[Serializable]
	public class FbxTextureExportScheme
	{
		public ColorSpace textureColorSpace = ColorSpace.Gamma;
		public TextureElements textureElements = TextureElements.None;
		public TextureSizes textureSize = TextureSizes.ScaleFull;

		public bool ExportMeshTextures { get { return (textureElements & TextureElements.Meshes) == TextureElements.Meshes; } }
		public bool ExportSkinnedMeshTextures { get { return (textureElements & TextureElements.SkinnedMeshes) == TextureElements.SkinnedMeshes; } }
		public bool ExportTerrainTextures { get { return (textureElements & TextureElements.Terrains) == TextureElements.Terrains; } }


		public virtual void AllocateTextures(Material mat, Renderer renderer, ref Texture2D _diffuseTexture, ref Texture2D _specularMapTexture, ref Texture2D _normalMapTexture, out int resolutionX, out int resolutionY)
		{
			_AllocateTextures(mat, renderer, ref _diffuseTexture, ref _normalMapTexture, ref _specularMapTexture, out resolutionX, out resolutionY);
		}

		public virtual float CalcColorSpace()
		{
#if UNITY_EDITOR
			return (UnityEditor.PlayerSettings.colorSpace == ColorSpace.Gamma) ? (textureColorSpace == ColorSpace.Gamma ? 1 : 2.2f) : (textureColorSpace == ColorSpace.Gamma ? 0.454545f : 1);
#else
			return 1f;
#endif
		}

		public enum TextureElements
		{
			None = 0,
			Meshes = 1,
			SkinnedMeshes = 2,
			MeshesAndSkinnedMeshes = 3,
			Terrains = 4,
			MeshesAndTerrains = 5,
			SkinnedMeshesAndTerrains = 6,
			MeshesAndSkinnedMeshesAndTerrains = 7,
			Everything = 7
		}

		public enum TextureElementsMask
		{
			Meshes = 1,
			SkinnedMeshes = 2,
			Terrains = 4,
		}

		public enum TextureSizes
		{
			ScaleFull,
			ScaleHalf,
			ScaleQuarter,
			ScaleEighth,
			ScaleSixteenth,
			Scale32nd
		}

#if UNITY_EDITOR

		public static FbxTextureExportScheme GetDefaultScheme()
		{
			var result = new FbxTextureExportScheme();
			result.textureColorSpace = (ColorSpace)EditorPrefs.GetInt("UnLogickFactory_FbxExporter_TextureScheme_ColorSpace", (int)result.textureColorSpace);
			result.textureElements = (TextureElements)EditorPrefs.GetInt("UnLogickFactory_FbxExporter_TextureScheme_MeshTypes", (int)result.textureElements);
			result.textureSize = (TextureSizes)EditorPrefs.GetInt("UnLogickFactory_FbxExporter_TextureScheme_TextureSize", (int)result.textureSize);
			return result;
		}

		static GUIStyle guiStyleSmall;

		public bool OnInspectorGUI(bool saveDefault)
		{
			if (guiStyleSmall == null)
			{
				GUISkin skin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);

				guiStyleSmall = new GUIStyle(skin.label);
				guiStyleSmall.normal.textColor = skin.label.normal.textColor;
				guiStyleSmall.fontSize = 11;
				guiStyleSmall.padding = new RectOffset(6, 0, 6, 6);
				guiStyleSmall.richText = true;
				guiStyleSmall.alignment = TextAnchor.MiddleLeft;
			}

			var changed = false;
			GUILayout.Label("<b>Texture Export Options</b>", guiStyleSmall);
			EditorGUI.indentLevel++;
			try
			{
				var elementMask = (TextureElementsMask)textureElements;
				var newTextureElements = (TextureElements)EditorGUILayout.EnumMaskField("Export Textures", elementMask);
				changed = newTextureElements != textureElements;
				textureElements = (TextureElements)newTextureElements;
				if (saveDefault && changed)
				{
					EditorPrefs.SetInt("UnLogickFactory_FbxExporter_TextureScheme_MeshTypes", (int)textureElements);
				}

				if (textureElements == TextureElements.None)
					return changed;

				var newTextureSize = (TextureSizes)EditorGUILayout.EnumPopup("Texture Sizes", textureSize);
				changed |= newTextureSize != textureSize;
				textureSize = newTextureSize;
				if (saveDefault && changed)
				{
					EditorPrefs.SetInt("UnLogickFactory_FbxExporter_TextureScheme_TextureSize", (int)textureSize);
				}

				var newTextureColorSpace = (ColorSpace)EditorGUILayout.EnumPopup("Texture Color Space", textureColorSpace);
				changed |= newTextureColorSpace != textureColorSpace;
				textureColorSpace = newTextureColorSpace;
				if (saveDefault && changed)
				{
					EditorPrefs.SetInt("UnLogickFactory_FbxExporter_TextureScheme_ColorSpace", (int)textureColorSpace);
				}
			}
			finally
			{
				EditorGUI.indentLevel--;
			}
			return changed;
		}
#endif

		protected void _AllocateTextures(Material mat, Renderer renderer, ref Texture2D _diffuseTexture, ref Texture2D _normalMapTexture, ref Texture2D _specularMapTexture, out int maxResolutionX, out int maxResolutionY)
		{
			maxResolutionX = 2;
			maxResolutionY = 2;
			bool updatedDiffuse = false;
			bool updatedSpecular = false;
			bool updatedNormal = false;

			if (!updatedDiffuse && AllocateTextureBasedOnMaterial(mat, "_MainTex", ref _diffuseTexture, ref maxResolutionX, ref maxResolutionY))
				updatedDiffuse = true;

			if (!updatedSpecular && AllocateTextureBasedOnMaterial(mat, "_SpecularMap", ref _specularMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedSpecular = true;
			if (!updatedSpecular && AllocateTextureBasedOnMaterial(mat, "_Specular", ref _specularMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedSpecular = true;
			if (!updatedSpecular && AllocateTextureBasedOnMaterial(mat, "_SpecularTex", ref _specularMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedSpecular = true;
			if (!updatedSpecular && AllocateTextureBasedOnMaterial(mat, "_SpecGlossMap", ref _specularMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedSpecular = true;
			if (!updatedSpecular && AllocateTextureBasedOnMaterial(mat, "_Spc", ref _specularMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedSpecular = true;

			if (!updatedNormal && AllocateTextureBasedOnMaterial(mat, "_NormalMap", ref _normalMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedNormal = true;
			if (!updatedNormal && AllocateTextureBasedOnMaterial(mat, "_NormalTex", ref _normalMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedNormal = true;
			if (!updatedNormal && AllocateTextureBasedOnMaterial(mat, "_BumpMap", ref _normalMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedNormal = true;
			if (!updatedNormal && AllocateTextureBasedOnMaterial(mat, "_BumpTex", ref _normalMapTexture, ref maxResolutionX, ref maxResolutionY))
				updatedNormal = true;

			if (!updatedDiffuse && !updatedSpecular && !updatedNormal)
			{
				// no texture resolution detected from normal texture names
#if UNITY_EDITOR
				var shader = mat.shader;
				var propertyCount = UnityEditor.ShaderUtil.GetPropertyCount(shader);
				bool foundTexture = false;
				for(int i = 0; i < propertyCount; i++)
				{
					if (UnityEditor.ShaderUtil.GetPropertyType(shader, i) == ShaderUtil.ShaderPropertyType.TexEnv)
					{
						var tex = mat.GetTexture(ShaderUtil.GetPropertyName(shader, i));
						if (tex != null) 
						{
							foundTexture = true;
							if (tex.width > maxResolutionX) 
								maxResolutionX = tex.width;
							if (tex.height > maxResolutionY) 
								maxResolutionY = tex.height;
						}
					}
				}
				if (!foundTexture)
					FbxExporter.LogErrorFormat(true, "FbxExporter - Failed to get texture size for {0}, defaulting to 2x2", mat);
#else
				FbxExporter.LogErrorFormat(true, "FbxExporter - Failed to get texture size for {0}, defaulting to 2x2", mat);
#endif
			}

			if (!updatedDiffuse)
				AllocateTexture(ref _diffuseTexture, maxResolutionX, maxResolutionY);
			if (!updatedSpecular)
				AllocateTexture(ref _specularMapTexture, maxResolutionX, maxResolutionY);
			if (!updatedNormal)
				AllocateTexture(ref _normalMapTexture, maxResolutionX, maxResolutionY);
		}

		protected bool AllocateTextureBasedOnMaterial(Material mat, string shaderVariableName, ref Texture2D _diffuseTexture, ref int maxResolutionX, ref int maxResolutionY)
		{
			if (mat.HasProperty(shaderVariableName))
			{
				var texture = mat.GetTexture(shaderVariableName);
				if (texture != null)
				{
					var resolutionX = texture.width;
					var resolutionY = texture.height;

					AdjustResolution(ref resolutionX, ref resolutionY);

					if (resolutionX > maxResolutionX)
						maxResolutionX = resolutionX;
					if (resolutionY > maxResolutionY)
						maxResolutionY = resolutionY;

					AllocateTexture(ref _diffuseTexture, resolutionX, resolutionY);
					return true;
				}
			}
			return false;
		}

		public void AllocateTexture(ref Texture2D texture, int resolutionX, int resolutionY)
		{
			if (texture != null && (texture.width != resolutionX || texture.height != resolutionY))
			{
				UnityEngine.Object.DestroyImmediate(texture, false);
				texture = null;
			}

			if (texture == null)
			{
				texture = new Texture2D(resolutionX, resolutionY, TextureFormat.ARGB32, false, true);
			}
		}

		protected void AdjustResolution(ref int resolutionX, ref int resolutionY)
		{
			switch (textureSize)
			{
				case TextureSizes.ScaleFull:
					break;
				case TextureSizes.ScaleHalf:
					resolutionX /= 2;
					resolutionY /= 2;
					break;
				case TextureSizes.ScaleQuarter:
					resolutionX /= 4;
					resolutionY /= 4;
					break;
				case TextureSizes.ScaleEighth:
					resolutionX /= 8;
					resolutionY /= 8;
					break;
				case TextureSizes.ScaleSixteenth:
					resolutionX /= 16;
					resolutionY /= 16;
					break;
				case TextureSizes.Scale32nd:
					resolutionX /= 32;
					resolutionY /= 32;
					break;
				default:
					resolutionX /= 32;
					resolutionY /= 32;
					break;
			}
		}
	}
}