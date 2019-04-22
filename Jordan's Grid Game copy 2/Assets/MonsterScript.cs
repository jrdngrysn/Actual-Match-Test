using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour
{
    public float healthScale;
    float barSubtract;


    public float healthPos;
    float barSubtractPos;
    float healthBarStartPos;

    public GameObject healthBar;
    float healthBarStart;

    int hitTimer = 0;

    public bool takeHit = false;

    // Start is called before the first frame update
    void Start()
    {
        healthBarStart = healthBar.transform.localScale.x;
        healthScale = healthBarStart;

        healthBarStartPos = healthBar.transform.localPosition.x;
        healthPos = healthBarStartPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (takeHit)
        {
            if (hitTimer < 50)
            {
                healthScale -= .005f;
                healthPos -= .005f;
                hitTimer++;
            }
            else
            {
                hitTimer = 0;
                takeHit = false;
            }

        }
       
        barSubtract = healthBarStart - healthScale;
        barSubtractPos = healthBarStartPos - healthPos;
        healthBar.transform.localScale = new Vector3 (healthBarStart -  barSubtract, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.localPosition = new Vector3(healthBarStartPos - barSubtractPos, healthBar.transform.localPosition.y, healthBar.transform.localPosition.z);
    }



}
