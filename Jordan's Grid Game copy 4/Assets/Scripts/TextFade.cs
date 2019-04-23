using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextFade : MonoBehaviour
{

    bool down = false;

    Color alphaChange;
    // Start is called before the first frame update
    void Start()
    {
        alphaChange = new Color(0, 0, 0, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (down)
        {
            alphaChange.a-= .03f;
        }
        else
        {
            alphaChange.a+= .03f;
        }

        if (alphaChange.a >= 1)
        {
            down = true;
        } else if (alphaChange.a <= 0)
        {
            down = false;
        }
        //Debug.Log(alphaChange.a);
        gameObject.GetComponent<Text>().color = alphaChange;
    }
}
