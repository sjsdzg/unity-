using UnityEngine;
using System.Collections;
using XFramework.Diagram;
using XFramework.Core;

public class ProcessDesignScene : MonoBehaviour
{
    private bool m_IsControlDown = false;

    // Use this for initialization
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            m_IsControlDown = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            m_IsControlDown = false;
        }

        if (m_IsControlDown && Input.GetKeyDown(KeyCode.Z))
        {
            UndoManager.Instance.Undo();
        }

        if (m_IsControlDown && Input.GetKeyDown(KeyCode.Y))
        {
            UndoManager.Instance.Redo();
        }
    }
}
