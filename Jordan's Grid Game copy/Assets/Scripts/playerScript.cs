using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    public Vector2Int playerCoords;

    public int turnsLeft = 6;


    // Start is called before the first frame update
    void Start()
    {
        playerCoords = GridMaker.Instance.currentPlayerCoords;
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponentInChildren<TextMesh>().text = turnsLeft.ToString();

        if (turnsLeft > 0)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (playerCoords.x - 1 >= 0)
                {
                    turnsLeft--;
                    playerCoords.x--;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (playerCoords.x + 1 <= GridMaker.WIDTH - 1)
                {
                    turnsLeft--;
                    playerCoords.x++;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (playerCoords.y - 1 >= 0)
                {
                    turnsLeft--;
                    playerCoords.y--;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (playerCoords.y + 1 <= GridMaker.HEIGHT - 1)
                {
                    turnsLeft--;
                    playerCoords.y++;
                    GridMaker.Instance.newPlayerCoords = playerCoords;
                    GridMaker.Instance.SendMessage("MovePlayer");
                }
            }
        }
    }
}
