using UnityEngine;
using System.Collections;
using XFramework.Common;

public class ProgressPanelTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ProgressPanel.Instance.Show("正在持续过程...", 5f);
        }
    }
}
