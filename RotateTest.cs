using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(90, 20, 20, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
