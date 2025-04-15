using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace XFramework.Core
{
    public interface IPref
    {
        void Load();

        void ResetToDefault();
    }

    public class PrefSettings
    {
        private static List<IPref> m_Prefs = new List<IPref>();

        public static void Add(IPref pref)
        {
            m_Prefs.Add(pref);
        }

        public static void ResetAllToDefault()
        {
            foreach (var pref in m_Prefs)
            {
                pref.ResetToDefault();
            }
        }

        /// <summary>
        /// 加载所需对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadRequired<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
    }

    public class PrefBool : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private bool m_DefaultValue;

        private bool m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public bool Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Value = value;
                PlayerPrefs.SetString(m_Name, m_Value.ToString());
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefBool(string name, bool defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_Value = defaultValue;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            string str = PlayerPrefs.GetString(m_Name);
            m_Value = bool.Parse(str);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator bool(PrefBool pref)
        {
            return pref.Value;
        }
    }

    public class PrefInt : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private int m_DefaultValue;

        private int m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public int Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Value = value;
                PlayerPrefs.SetString(m_Name, m_Value.ToString());
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefInt(string name, int defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_Value = defaultValue;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            string str = PlayerPrefs.GetString(m_Name);
            m_Value = int.Parse(str);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator int(PrefInt pref)
        {
            return pref.Value;
        }
    }

    public class PrefFloat : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private float m_DefaultValue;

        private float m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public float Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Value = value;
                PlayerPrefs.SetString(m_Name, m_Value.ToString());
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefFloat(string name, float defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_Value = defaultValue;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            string str = PlayerPrefs.GetString(m_Name);
            m_Value = float.Parse(str);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator float(PrefFloat pref)
        {
            return pref.Value;
        }
    }

    public class PrefString : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private string m_DefaultValue;

        private string m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Value = value;
                PlayerPrefs.SetString(m_Name, m_Value.ToString());
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefString(string name, string defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_Value = defaultValue;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            m_Value = PlayerPrefs.GetString(m_Name);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator string(PrefString pref)
        {
            return pref.Value;
        }
    }

    public class PrefVector3 : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private Vector3 m_DefaultValue;

        private Vector3 m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public Vector3 Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Value = value;
                PlayerPrefs.SetString(m_Name, ToString(m_Value));
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefVector3(string name, Vector3 defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_Value = defaultValue;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            string s = PlayerPrefs.GetString(m_Name);
            m_Value = FromString(s);
        }

        private string ToString(Vector3 vector)
        {
            return string.Format("{0},{1},{2}", vector.x, vector.y, vector.z);
        }

        private Vector3 FromString(string s)
        {
            string[] split = s.Split(',');

            if (split.Length != 3)
            {
                Debug.LogError("Parsing PrefVector3 failed");
                return new Vector3();
            }

            float x = float.Parse(split[0]);
            float y = float.Parse(split[1]);
            float z = float.Parse(split[2]);

            return new Vector3(x, y, z);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator Vector3(PrefVector3 pref)
        {
            return pref.Value;
        }
    }

    public class PrefColor : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private Color m_DefaultValue;

        private Color m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public Color Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Value = value;
                PlayerPrefs.SetString(m_Name,ToString(m_Value));
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefColor(string name, Color defaultValue)
        {
            m_Name = name;
            m_DefaultValue = defaultValue;
            m_Value = defaultValue;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            string s = PlayerPrefs.GetString(m_Name);
            m_Value = FromString(s);
        }

        private string ToString(Color color)
        {
            return string.Format("{0};{1};{2};{3}", color.r, color.g, color.b, color.a);
        }

        private Color FromString(string s)
        {
            string[] split = s.Split(';');

            if (split.Length != 4)
            {
                Debug.LogError("Parsing PrefColor failed");
                return new Color();
            }

            float r = float.Parse(split[0]);
            float g = float.Parse(split[1]);
            float b = float.Parse(split[2]);
            float a = float.Parse(split[3]);

            return new Color(r, g, b, a);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator Color(PrefColor pref)
        {
            return pref.Value;
        }

        public static implicit operator Color32(PrefColor pref)
        {
            return pref.Value;
        }
    }

    public class PrefMaterial : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private string m_DefaultValue;

        private string m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Material = null;
                m_Value = value;
                PlayerPrefs.SetString(m_Name, m_Value.ToString());
            }
        }

        private Material m_Material;
        /// <summary>
        /// 材质
        /// </summary>
        public Material Material
        {
            get
            {
                if (m_Material == null)
                {
                    m_Material = PrefSettings.LoadRequired<Material>(Value);
                }
                return m_Material;
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefMaterial(string name, string defaultMaterialPath)
        {
            m_Name = name;
            m_DefaultValue = defaultMaterialPath;
            m_Value = defaultMaterialPath;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            m_Value = PlayerPrefs.GetString(m_Name);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator Material(PrefMaterial pref)
        {
            return pref.Material;
        }
    }

    public class PrefTexture2D : IPref
    {
        /// <summary>
        /// 默认值
        /// </summary>
        private string m_DefaultValue;

        private string m_Value;
        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get { Load(); return m_Value; }
            set
            {
                Load();
                if (m_Value == value)
                    return;

                m_Texture2D = null;
                m_Value = value;
                PlayerPrefs.SetString(m_Name, m_Value.ToString());
            }
        }

        private Texture2D m_Texture2D;
        /// <summary>
        /// Texture2D
        /// </summary>
        public Texture2D Texture2D
        {
            get
            {
                if (m_Texture2D == null)
                {
                    m_Texture2D = PrefSettings.LoadRequired<Texture2D>(Value);
                }
                return m_Texture2D;
            }
        }

        /// <summary>
        /// 是否加载过
        /// </summary>
        private bool m_Loaded;

        private string m_Name;
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return m_Name; }
        }

        public PrefTexture2D(string name, string defaultMaterialPath)
        {
            m_Name = name;
            m_DefaultValue = defaultMaterialPath;
            m_Value = defaultMaterialPath;
            PrefSettings.Add(this);
            m_Loaded = false;
        }

        public void Load()
        {
            if (m_Loaded)
                return;

            m_Loaded = true;

            if (!PlayerPrefs.HasKey(m_Name))
            {
                m_Value = m_DefaultValue;
                return;
            }

            m_Value = PlayerPrefs.GetString(m_Name);
        }

        public void ResetToDefault()
        {
            Value = m_DefaultValue;
        }

        public static implicit operator Texture2D(PrefTexture2D pref)
        {
            return pref.Texture2D;
        }
    }
}
