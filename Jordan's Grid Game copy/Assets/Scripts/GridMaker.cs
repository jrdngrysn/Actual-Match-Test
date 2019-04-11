using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    public const int WIDTH = 5;
    public const int HEIGHT = 7;

    float xOffset = WIDTH / 2f - .5f;
    float yOffset = HEIGHT / 2f - .5f;

    public GameObject[,] tiles;

    public GameObject tilePrefab;

    public GameObject player;

    GameObject gridHolder;

    public Vector2Int currentPlayerCoords;
    public Vector2Int newPlayerCoords;

    public static GridMaker Instance;

    public string gameState;

    //public static float slideLErp = -1;
    //public float lerpSpeed = .25f;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        gameState = "start";

        tiles = new GameObject[WIDTH, HEIGHT];

        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(-1f, -0.5f, 0);

        CreateGrid();

    }

    // Update is called once per frame
    void Update()
    {

    
       
    }

    void CreateGrid ()
    {
       
        gameState = "start";

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                GameObject newTile = Instantiate(tilePrefab);

                newTile.transform.parent = gridHolder.transform;
                newTile.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                tiles[x, y] = newTile;

                TileScript tileScript = newTile.GetComponent<TileScript>();
                tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                tileScript.coords = new Vector2Int(x, y);

            }
        }


        currentPlayerCoords = new Vector2Int(WIDTH / 2, HEIGHT / 2);

        GameObject newPlayer = Instantiate(player);

        newPlayer.transform.parent = gridHolder.transform;
        newPlayer.transform.localPosition = tiles[currentPlayerCoords.x, currentPlayerCoords.y].transform.localPosition;
        Destroy(tiles[currentPlayerCoords.x, currentPlayerCoords.y]);

        player = newPlayer;
        tiles[currentPlayerCoords.x, currentPlayerCoords.y] = newPlayer;

        CheckGridForMatches();
        gameState = "play";
    }

    void MovePlayer()
    {
        GameObject targetTile = tiles[newPlayerCoords.x, newPlayerCoords.y];

        Vector2 oldPlayerPos = new Vector2(player.transform.localPosition.x, player.transform.localPosition.y);

        player.transform.localPosition = targetTile.transform.localPosition;
        targetTile.transform.localPosition = oldPlayerPos;
        tiles[currentPlayerCoords.x, currentPlayerCoords.y] = targetTile;
        tiles[newPlayerCoords.x, newPlayerCoords.y] = player;
        currentPlayerCoords = newPlayerCoords;
        CheckGridForMatches();

    }


    void CheckGridForMatches()
    {

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (tiles[x, y].tag != "Player")
                {
                    int currentColor = tiles[x, y].GetComponent<TileScript>().type;

                    GameObject middleTile = tiles[x, y];

                    if (x - 1 <= WIDTH - 1
                        && x - 1 >= 0
                        && x + 1 <= WIDTH - 1
                        && x + 1 >= 0)
                    {

                        if (tiles[x - 1, y].tag != "Player"
                        && tiles[x + 1, y].tag != "Player")
                        {
                            GameObject leftTile = tiles[x - 1, y];
                            GameObject rightTile = tiles[x + 1, y];

                            if (leftTile.GetComponent<TileScript>().type == currentColor && rightTile.GetComponent<TileScript>().type == currentColor)
                            {
                                middleTile.GetComponent<SpriteRenderer>().color = Color.gray;
                                leftTile.GetComponent<SpriteRenderer>().color = Color.gray;
                                rightTile.GetComponent<SpriteRenderer>().color = Color.gray;

                            }
                        }
                    }

                    if (y - 1 <= HEIGHT - 1
                        && y - 1 >= 0
                        && y + 1 <= HEIGHT - 1
                        && y + 1 >= 0)
                    {
                        if (tiles[x, y - 1].tag != "Player"
                           && tiles[x, y + 1].tag != "Player")
                        {

                            GameObject belowTile = tiles[x, y - 1];
                            GameObject aboveTile = tiles[x, y + 1];

                            if (aboveTile.GetComponent<TileScript>().type == currentColor && belowTile.GetComponent<TileScript>().type == currentColor)
                            {
                                middleTile.GetComponent<SpriteRenderer>().color = Color.gray;
                                belowTile.GetComponent<SpriteRenderer>().color = Color.gray;
                                aboveTile.GetComponent<SpriteRenderer>().color = Color.gray;
                            }
                        }
                    }


                }
            }
        }

        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (tiles[x, y] != null)
                {
                    if (tiles[x, y].GetComponent<SpriteRenderer>().color == Color.gray)
                    {

                        if (gameState == "start")
                        {
                            //Debug.Log("Change " + x + "," + y);
                            TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                            tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                            CheckGridForMatches();
                        }
                        else if (gameState == "play")
                        {
                            Debug.Log(x + ", " + y);
                            Destroy(tiles[x, y]);

                        }

                    }

                }

            }
        }
        CheckGridForNulls();
    }

    void CheckGridForNulls()
    {
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (!tiles[x, y])
                {
                    Debug.Log("null");
                    Vector2Int coordsToFill = new Vector2Int(x, y);
                    DoALerp(coordsToFill);
                }
            }
        }

    }

    void DoALerp(Vector2Int coordsToFill)
    {
        for (int i = 0; i < 7; i++)
        {
            if (coordsToFill.y - i >= 0 && tiles[coordsToFill.x, coordsToFill.y] != null)
            {
                StartCoroutine(OnlyLerp(coordsToFill, i));
                tiles[coordsToFill.x, coordsToFill.y - i + 1] = tiles[coordsToFill.x, coordsToFill.y - i];
                GameObject movedTile;
                movedTile = tiles[coordsToFill.x, coordsToFill.y - i + 1];
                TileScript tileScript = movedTile.GetComponent<TileScript>();
                tileScript.coords = new Vector2Int(coordsToFill.x, coordsToFill.y - i + 1);
            }
            else
            {
                continue;
            }
        }
    }

    public IEnumerator OnlyLerp(Vector2Int coordsToFill, int spot)
    {
        float lerpStart;
        float lerpEnd;
        float endTime = 5f;
        float elapsedTime = 0;
        lerpStart = coordCorrection(coordsToFill.y - spot);
        lerpEnd = coordCorrection(coordsToFill.y - spot + 1);
        //lerpStart = tiles[coordsToFill.x, coordsToFill.y - i].transform.localPosition.y;
        //lerpEnd = tiles[coordsToFill.x, coordsToFill.y - i + 1].transform.localPosition.y;
        //Debug.Log(lerpEnd + " " + i);
        while (elapsedTime < endTime)
        {
            elapsedTime += Time.deltaTime;
            //Debug.Log(elapsedTime);
            tiles[coordsToFill.x, coordsToFill.y - spot].transform.localPosition = new Vector3(
            tiles[coordsToFill.x, coordsToFill.y].transform.localPosition.x,
            Mathf.Lerp(lerpStart, lerpEnd, elapsedTime), tiles[coordsToFill.x, coordsToFill.y].transform.localPosition.z);
            Debug.Log(tiles[coordsToFill.x, coordsToFill.y - spot].transform.localPosition);
            yield return null;
        }
        yield return null;
    }

    int coordCorrection (int gridCoordsToChange)
    {
        int positionIntY = gridCoordsToChange;

        if (gridCoordsToChange == 0)
        {
            positionIntY = 4;
        }
        else if (gridCoordsToChange == 1)
        {
            positionIntY = 3;
        }
        else if (gridCoordsToChange == 2)
        {
            positionIntY = 2;
        }
        else if (gridCoordsToChange == 3)
        {
            positionIntY = 1;
        }
        else if (gridCoordsToChange == 4)
        {
            positionIntY = 0;
        }
        else if (gridCoordsToChange == 5)
        {
            positionIntY = -1;
        }
        else if (gridCoordsToChange == 6)
        {
            positionIntY = -2;
        }

        return positionIntY;
    }

    void Repopulate()
    {
        Debug.Log("Repopulate");
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                Debug.Log(x + ", " + y);
                if (tiles[x, y] == null)
                {
                    Debug.Log("null");
                   
                        GameObject newTile = Instantiate(tilePrefab);

                        newTile.transform.parent = gridHolder.transform;
                        newTile.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                        tiles[x, y] = newTile;

                        TileScript tileScript = newTile.GetComponent<TileScript>();
                        tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                        tileScript.coords = new Vector2Int(x, y);

                    //else
                    //{
                    //    Vector2Int coordsToFill = new Vector2Int(x, y);
                    //    StartCoroutine(LerpDown(coordsToFill));
                    //}
                }
            }
        }
    }


}
