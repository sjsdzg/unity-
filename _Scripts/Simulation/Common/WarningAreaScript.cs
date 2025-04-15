using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XFramework.Common;
using XFramework.Core;
using XFramework.Simulation;

public class WarningAreaScript : MonoBehaviour {
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            EventDispatcher.ExecuteEvent(Events.HUDText.Show, Utils.NewGameObject().transform, "危险！请站在警示线外！", Color.red);

        }
    }
}
