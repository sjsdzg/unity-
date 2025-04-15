using UnityEngine;
using System.Collections;
using PathologicalGames;

public class SpawnPoolTest : MonoBehaviour
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
            SpawnPool pool = PoolManager.Pools["HUDText"];
            GameObject obj = pool.prefabs["BubbleText"].gameObject;
            Debug.Log(obj.name);
        }
    }
}
