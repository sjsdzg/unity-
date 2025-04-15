using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = "CO\u00B2111110000111110000111110000111110000111110000";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
