using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public int type;
    public Color[] tileColors;

    public Vector2Int coords;

    public void SetSprite(int Rand)
    {
        type = Rand;
        GetComponent<SpriteRenderer>().color = tileColors[Rand];
    }

    private void Start()
    {
     
    }
}
