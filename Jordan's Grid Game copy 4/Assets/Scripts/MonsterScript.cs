using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MonsterScript : MonoBehaviour
{
    public float healthScale;
    float barSubtract;
    public float healthPos;
    float barSubtractPos;
    float healthBarStartPos;
    public GameObject healthBar;
    float healthBarStart;

    public float nextHitScale;
    float nextHitAdd;
    public float nextHitPos;
    float nextHitAddPos;
    float nextHitStartPos;
    public GameObject nextHitBar;
    float nextHitStart;
    float nextHitIncrement;

    public int nextHitApproach = 0;

    int hitTimer = 0;

    public bool takeHit = false;
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        healthBarStart = healthBar.transform.localScale.x;
        healthScale = healthBarStart;

        healthBarStartPos = healthBar.transform.localPosition.x;
        healthPos = healthBarStartPos;


        nextHitStart = 0;
        nextHitScale = nextHitStart;

        nextHitStartPos = nextHitBar.transform.localPosition.x;
        nextHitPos = nextHitStartPos;

        nextHitIncrement = 3.97f / 3;

        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (healthScale < 0)
        {
            SceneManager.LoadScene("WinScene");
        }

        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * 1f) * .2f;

        transform.position = tempPos;


        if (takeHit)
        {
            if (hitTimer < 80)
            {
                healthScale -= .01f;
                healthPos -= .01f;
                hitTimer++;
                if (hitTimer < 40)
                {
                    if (hitTimer % 10 == 0)
                    {
                        healthBar.GetComponent<SpriteRenderer>().color = Color.black;
                        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                    else
                    {
                        healthBar.GetComponent<SpriteRenderer>().color = Color.white;
                        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }
            }
            else
            {
                hitTimer = 0;
                healthBar.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                takeHit = false;
            }

        }

        if (nextHitApproach > 0)
        {
            if (nextHitScale < nextHitIncrement * nextHitApproach) {
                //Debug.Log(nextHitApproach);
                    nextHitScale += .06f;
                nextHitPos += .06f;
            }

            if (nextHitScale > 3.97f)
            {
                nextHitStart = 0;
                nextHitScale = nextHitStart;
                GridMaker.Instance.HitPlayer();
                nextHitApproach = 0;
            }

        }


        barSubtract = healthBarStart - healthScale;
        barSubtractPos = healthBarStartPos - healthPos;
        healthBar.transform.localScale = new Vector3 (healthBarStart -  barSubtract, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.localPosition = new Vector3(healthBarStartPos - barSubtractPos / 1.33f, healthBar.transform.localPosition.y, healthBar.transform.localPosition.z);


        //nextHitAdd = nextHitStart + nextHitScale;
        nextHitAddPos = nextHitStartPos + nextHitPos;
        nextHitBar.transform.localScale = new Vector3(nextHitStart + nextHitScale, nextHitBar.transform.localScale.y, nextHitBar.transform.localScale.z);
        nextHitBar.transform.localPosition = new Vector3(nextHitStartPos + nextHitScale / 1.33f, nextHitBar.transform.localPosition.y, nextHitBar.transform.localPosition.z);
    }



}
