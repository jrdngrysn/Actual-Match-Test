using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFloat : MonoBehaviour
{
    public Vector3 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1f) * .2f;

        transform.position = tempPos;
    }
}
