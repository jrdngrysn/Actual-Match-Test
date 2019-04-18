using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int type;
    public Color[] tileColors;

    public Vector2Int coords;
    public TextMesh coordText;

    public bool inSlide = false;
    public Vector3 startPos;
    public Vector3 endPos;

    public void SetSprite(int Rand)
    {
        type = Rand;
        GetComponent<SpriteRenderer>().color = tileColors[Rand];
    }

    private void Start()
    {
     if (gameObject.tag == "Player")
        {
            type = -1;
        }
    }

    private void Update()
    {
        if (type != -1)
        {
            coordText.text = coords.x.ToString() + "," + coords.y.ToString();
        }

         if (inSlide)
        {
            if (GridMaker.slideLerp < 0)
            {
                gameObject.transform.localPosition = endPos;
                inSlide = false;
            }
            else
            {
                gameObject.transform.localPosition = Vector3.Lerp(startPos, endPos, GridMaker.slideLerp);
            }
        }
    }
    public bool isMatch (GameObject gameObject1, GameObject gameObject2)
    {
        TileScript ts1 = gameObject1.GetComponent<TileScript>();
        TileScript ts2 = gameObject2.GetComponent<TileScript>();
        return ts1 != null && ts2 != null && type == ts1.type && type == ts2.type;
    }

    public void SetUpSlide (Vector2 newCoords) 
    {
        inSlide = true;
        startPos = gameObject.transform.localPosition;
        endPos = newCoords;
    }

}
