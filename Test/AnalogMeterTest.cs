using UnityEngine;
using System.Collections;
using XFramework.Component;

public class AnalogMeterTest : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    private AnalogMeterComponent component;
    // Use this for initialization
    void Start()
    {
        component = transform.GetComponent<AnalogMeterComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            component.Value = -0.1f;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            component.Value = 0f;
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            component.Value = 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            component.Value = 0.5f;
        }

    }
}
