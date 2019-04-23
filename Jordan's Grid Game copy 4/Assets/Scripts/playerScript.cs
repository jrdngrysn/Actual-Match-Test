using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    public Vector2Int playerCoords;
    public int turnsLeft = 6;

    public bool canMove = false;

    public Sprite inactiveSprite;
    public Sprite activeSprite;



    public float healthScale;
    float barSubtract;
    public float healthPos;
    float barSubtractPos;
    float healthBarStartPos;
    public GameObject playerHealthBar;
    float healthBarStart;

    int hitTimer = 0;

    public bool takeHit = false;

    // Start is called before the first frame update
    void Start()
    {

        playerHealthBar = GameObject.Find("PlayerHealthColor");
        playerCoords = GridMaker.Instance.currentPlayerCoords;
        healthBarStart = playerHealthBar.transform.localScale.x;
        healthScale = healthBarStart;

        healthBarStartPos = playerHealthBar.transform.localPosition.x;
        healthPos = healthBarStartPos;
    }

    // Update is called once per frame
    void Update()
    {
//        gameObject.GetComponentInChildren<TextMesh>().text = playerCoords.ToString();

        if (canMove)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = activeSprite;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
            {
                if (playerCoords.x - 1 >= 0)
                {
                    turnsLeft--;
                    playerCoords.x--;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                    canMove = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
            {
                if (playerCoords.x + 1 <= GridMaker.WIDTH - 1)
                {
                    turnsLeft--;
                    playerCoords.x++;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                    canMove = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown("w"))
            {
                if (playerCoords.y - 1 >= 0)
                {
                    turnsLeft--;
                    playerCoords.y--;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                    canMove = false;
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown("s"))
            {
                if (playerCoords.y + 1 <= GridMaker.HEIGHT - 1)
                {
                    turnsLeft--;
                    playerCoords.y++;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                    canMove = false;
                }
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = inactiveSprite;
        }

        if (takeHit)
        {
            if (hitTimer < 100)
            {
                healthScale -= .01f;
                healthPos -= .01f;
                if (hitTimer < 30)
                {
                    if (hitTimer % 10 == 0)
                    {
                        playerHealthBar.GetComponent<SpriteRenderer>().color = Color.black;
                    }
                    else
                    {
                        playerHealthBar.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                }

                hitTimer++;
            }
            else
            {
                hitTimer = 0;
                takeHit = false;
                playerHealthBar.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }

        barSubtract = healthBarStart - healthScale;
        barSubtractPos = healthBarStartPos - healthPos;
        playerHealthBar.transform.localScale = new Vector3(healthBarStart - barSubtract, playerHealthBar.transform.localScale.y, playerHealthBar.transform.localScale.z);
        playerHealthBar.transform.localPosition = new Vector3(healthBarStartPos - barSubtractPos / 1.33f, playerHealthBar.transform.localPosition.y, playerHealthBar.transform.localPosition.z);
    }
}
