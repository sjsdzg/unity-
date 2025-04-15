using System;
using System.Collections;
using System.Collections.Generic;


namespace XFramework.Core
{
	public class Message : IEnumerable<KeyValuePair<string, object>>
	{
		private Dictionary<string, object> DictDatas = null;
		public string Name { get; private set; }
		public object Sender { get; private set; }
		public object Content { get; set; }

		#region message[key] = value or data = message[key]

		/// <summary>
		/// Gets or sets the <see cref="XFramework.Message"/> with the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public object this[string key]
		{
			get
			{
				if (null == DictDatas || !DictDatas.ContainsKey(key))
					return null;
				return DictDatas[key];
			}
			set
			{
				if (null == DictDatas)
					DictDatas = new Dictionary<string, object>();
				if (DictDatas.ContainsKey(key))
					DictDatas[key] = value;
				else
					DictDatas.Add(key, value);
			}
		}

		#endregion

		#region IEnumerable implementation

		public IEnumerator<KeyValuePair<string, object>> GetEnumerator ()
		{
			if (null == DictDatas)
				yield break;
			foreach (KeyValuePair<string, object> kvp in DictDatas)
			{
				yield return kvp;
			}
		}

		#endregion

		#region IEnumerable implementation

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return DictDatas.GetEnumerator();
		}

		#endregion

        /// <summary>
        /// 获取可用参数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Object> GetParams()
        {
            return DictDatas;
        }

        #region Message Construction Function

        /// <summary>
        /// Initializes a new instance of the <see cref="XFramework.Message"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="sender">Sender.</param>
        public Message (string name, object sender)
		{
			Name = name;
			Sender = sender;
			Content = null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XFramework.Message"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="sender">Sender.</param>
		/// <param name="content">Content.</param>
		public Message (string name, object sender, object content)
		{
			Name = name;
			Sender = sender;
			Content = content;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XFramework.Message"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="sender">Sender.</param>
		/// <param name="content">Content.</param>
		/// <param name="_dicParams">_dic parameters.</param>
		public Message (string name, object sender, object content, params object[] _dicParams)
		{
			Name = name;
			Sender = sender;
			Content = content;
			if (_dicParams.GetType() == typeof(Dictionary<string, object>))
			{
				foreach (object _dicParam in _dicParams)
				{
					foreach (KeyValuePair<string, object> kvp in _dicParam as Dictionary<string, object>)
					{
						//dicDatas[kvp.Key] = kvp.Value;  //error
						this[kvp.Key] = kvp.Value;
					}
				}
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XFramework.Message"/> class.
		/// </summary>
		/// <param name="message">Message.</param>
		public Message (Message message)
		{
			Name = message.Name;
			Sender = message.Sender;
			Content = message.Content;
			foreach (KeyValuePair<string, object> kvp in message.DictDatas)
			{
				this[kvp.Key] = kvp.Value;
			}
		}

		#endregion

		#region Add & Remove
		/// <summary>
		/// Add the specified key and value.
		/// </summary>
		/// <param name="key">Key.</param>
		/// <param name="value">Value.</param>
		public void Add(string key, object value)
		{
			this[key] = value;
		}

		/// <summary>
		/// Remove the specified key.
		/// </summary>
		/// <param name="key">Key.</param>
		public void Remove(string key)
		{
			if (null != DictDatas && DictDatas.ContainsKey(key))
			{
				DictDatas.Remove(key);
			}
		}
		#endregion

		#region Send()
		/// <summary>
		/// Send this instance.
		/// </summary>
		public void Send()
		{
			//MessageCenter Send Message
			Messager.Instance.SendMessage(this);
		}
		#endregion

	}
}

