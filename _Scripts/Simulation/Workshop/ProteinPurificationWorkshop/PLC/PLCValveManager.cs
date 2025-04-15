using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XFramework.PLC;
using XFramework.Core;
using XFramework.Module;
using System;

public class PLCValveManager : MonoBehaviour
{
    private UniEvent<string, bool> onClick = new UniEvent<string, bool>();
    private string valveName;
    public UniEvent<string, bool> OnClick
    {
        get { return onClick; }
        set { onClick = value; }
    }
    private Dictionary<string, PLCValveClickComponent> m_PLCValveComponents = new Dictionary<string, PLCValveClickComponent>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform item in transform)
        {
            PLCValveClickComponent component = item.GetComponent<PLCValveClickComponent>();
            if (component != null)
            {
                m_PLCValveComponents.Add(item.name, component);
            }
        }
    }

    public void OpenOrClosePLCValve(string name, ValveState state)
    {
        PLCValveClickComponent component = null;
        if (m_PLCValveComponents.TryGetValue(name, out component))
        {
            valveName = name;
            component.SetReadyState();
            component.State = state;
            component.OnClick.RemoveAllListeners();
            component.OnClick.AddListener(Valve_onClick);
        }
    }
    public void AutoOpenOrClosePLCValve(string name, ValveState state)
    {
        PLCValveClickComponent component = null;
        if (m_PLCValveComponents.TryGetValue(name, out component))
        {
            valveName = name;
            component.OpenOrClose(state);
        }
    }

    private void Valve_onClick(bool state)
    {
        OnClick.Invoke(valveName, state);
    }


}
