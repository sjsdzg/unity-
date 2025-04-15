using System;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Core;

namespace XFramework.Core
{
	public class Messager : Singleton<Messager>
	{
		private Dictionary<string, List<MessageEventHandler>> DictMessageEvents = null;

		protected override void Init()
		{
			DictMessageEvents = new Dictionary<string, List<MessageEventHandler>>();
		}

		#region Add & Remove Listener
		public void AddListener(string messageName, MessageEventHandler messageEvent)
		{
			//Debug.Log("AddListener Name : " + messageName);
			List<MessageEventHandler> list = null;
			if (DictMessageEvents.ContainsKey(messageName))
			{
				list = DictMessageEvents[messageName];
			}
			else
			{
				list = new List<MessageEventHandler>();
				DictMessageEvents.Add(messageName, list);
			}
			// no same messageEvent then add
			if (!list.Contains(messageEvent))
			{
				list.Add(messageEvent);
			}
		}

		public void RemoveListener(string messageName, MessageEventHandler messageEvent)
		{
			Debug.Log("RemoveListener Name : " + messageName);
			if (DictMessageEvents.ContainsKey(messageName))
			{
				List<MessageEventHandler> list = DictMessageEvents[messageName];
				if (list.Contains(messageEvent))
				{
					list.Remove(messageEvent);
				}
				if (list.Count <= 0)
				{
					DictMessageEvents.Remove(messageName);
				}
			}
		}

		public void RemoveAllListener()
		{
			DictMessageEvents.Clear();
		}
		#endregion

		#region Send Message

		public void SendMessage(Message message)
		{
			DoMessageDispatcher(message);
		}

		public void SendMessage(string name, object sender)
		{
			SendMessage(new Message(name, sender));
		}

		public void SendMessage(string name, object sender, object content)
		{
			SendMessage(new Message(name, sender, content));
		}

		public void SendMessage(string name, object sender, object content, params object[] dicParams)
		{
			SendMessage(new Message(name, sender, content, dicParams));
		}

		private void DoMessageDispatcher(Message message)
		{
			//Debug.Log("DoMessageDispatcher Name : " + message.Name);
			if (DictMessageEvents == null || !DictMessageEvents.ContainsKey(message.Name))
				return;
			List<MessageEventHandler> list = DictMessageEvents[message.Name];
			for (int i=0; i<list.Count; i++)
			{
                MessageEventHandler messageEvent = list[i];
				if (null != messageEvent)
				{
					messageEvent(message);
				}
			}
		}

		#endregion

	}
}

