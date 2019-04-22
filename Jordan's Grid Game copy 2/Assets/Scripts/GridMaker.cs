using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GridMaker : MonoBehaviour
{
    //variables
    public const int WIDTH = 5;
    public const int HEIGHT = 7;

    float xOffset = WIDTH / 2f + 3.5f;
    float yOffset = HEIGHT / 2f - .5f;

    public GameObject[,] tiles;

    public GameObject tilePrefab;

    public GameObject player;

    public GameObject particleBurst;

    GameObject gridHolder;

    public Vector2Int currentPlayerCoords;
    public Vector2Int newPlayerCoords;

    public static GridMaker Instance;

    public string gameState;

    public Text scoreText;

    public int score = 0;

    public GameObject monster;
    MonsterScript monsterScript;

    //public Dictionary<Vector2Int, Transform> transformDict = new Dictionary<Vector2Int, Transform>();


    public static float slideLerp = -1;
    public float lerpSpeed = .25f;


    // Start is called before the first frame update
    void Start()
    {
        //set up instance and create the gridHolder
        Instance = this;

        gameState = "start";

        monsterScript = monster.GetComponent<MonsterScript>();

        tiles = new GameObject[WIDTH, HEIGHT];

        gridHolder = new GameObject();
        gridHolder.transform.position = new Vector3(-1f, -0.5f, 0);

        CreateGrid();

    }

    // Update is called once per frame
    void Update()
    {
        //HasMatch();
        if (slideLerp < 0 && !Repopulate() && HasMatch())
        {
            RemoveMatches();
        }
        else if (slideLerp >= 0)
        {
            slideLerp += Time.deltaTime / lerpSpeed;
            if (slideLerp >= 1)
            {
                slideLerp = -1;
            }
        }
        //constantly update score nad check if the game has finished
        scoreText.text = "Score: " + score.ToString();

        if (player.GetComponent<playerScript>().turnsLeft < 1)
        {
            SceneManager.LoadScene("EndScene");
        }


    }

    void CreateGrid()
    {
        //create the grid using a double for loop
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

        //spawn in the player and delete the tile where it spawns
        currentPlayerCoords = new Vector2Int(WIDTH / 2, HEIGHT / 2);

        GameObject newPlayer = Instantiate(player);

        GameObject removedTiled = tiles[currentPlayerCoords.x, currentPlayerCoords.y];

        newPlayer.transform.parent = gridHolder.transform;
        newPlayer.transform.localPosition = tiles[currentPlayerCoords.x, currentPlayerCoords.y].transform.localPosition;

        player = newPlayer;
        tiles[currentPlayerCoords.x, currentPlayerCoords.y] = newPlayer;

        //while (HasMatch())
        //{
        //    ResetGrid();
        //}
        Destroy(removedTiled);
        gameState = "play";
    }

    void MovePlayer()
    {
        //move the player, called from the playerScript
        GameObject targetTile = tiles[newPlayerCoords.x, newPlayerCoords.y];

        Vector2 oldPlayerPos = new Vector2(player.transform.localPosition.x, player.transform.localPosition.y);

        player.transform.localPosition = targetTile.transform.localPosition;
        targetTile.transform.localPosition = oldPlayerPos;
        tiles[currentPlayerCoords.x, currentPlayerCoords.y] = targetTile;
        tiles[newPlayerCoords.x, newPlayerCoords.y] = player;
        TileScript tileScript = targetTile.GetComponent<TileScript>();
        tileScript.coords = currentPlayerCoords;
        currentPlayerCoords = newPlayerCoords;

    }


    bool HasMatch()
    {

        //iterate through the grid and turn matching tiles gray
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                if (x < WIDTH - 2 && tileScript.isMatch(tiles[x + 1, y], tiles[x + 2, y]))
                {
                    return true;
                }
                if (y < HEIGHT - 2 && tileScript.isMatch(tiles[x, y + 1], tiles[x, y + 2]))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void RemoveMatches()
    {

        //iterate through the grid and turn matching tiles gray
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
                if (x < WIDTH - 2 && tileScript.isMatch(tiles[x + 1, y], tiles[x + 2, y]))
                {
                    GameObject newBurst = Instantiate(particleBurst);
                    newBurst.transform.parent = gridHolder.transform;
                    newBurst.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);
                    GameObject newBurst2 = Instantiate(particleBurst);
                    newBurst2.transform.parent = gridHolder.transform;
                    newBurst2.transform.localPosition = new Vector2(WIDTH - x + 1 - xOffset, HEIGHT - y - yOffset);
                    GameObject newBurst3 = Instantiate(particleBurst);
                    newBurst3.transform.parent = gridHolder.transform;
                    newBurst3.transform.localPosition = new Vector2(WIDTH - x + 2 - xOffset, HEIGHT - y - yOffset);

                    score+= 3;

                    player.GetComponent<playerScript>().turnsLeft = 6;

                    Destroy(tiles[x, y]);
                    Destroy(tiles[x + 1, y]);
                    Destroy(tiles[x + 2, y]);
                }
                if (y < HEIGHT - 2 && tileScript.isMatch(tiles[x, y + 1], tiles[x, y + 2]))
                {
                    GameObject newBurst = Instantiate(particleBurst);
                    newBurst.transform.parent = gridHolder.transform;
                    newBurst.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);
                    GameObject newBurst2 = Instantiate(particleBurst);
                    newBurst2.transform.parent = gridHolder.transform;
                    newBurst2.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y + 1 - yOffset);
                    GameObject newBurst3 = Instantiate(particleBurst);
                    newBurst3.transform.parent = gridHolder.transform;
                    newBurst3.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y + 2- yOffset);

                    score+= 3;
                    monsterScript.takeHit = true;
                    player.GetComponent<playerScript>().turnsLeft = 6;

                    Destroy(tiles[x, y]);
                    Destroy(tiles[x, y + 1]);
                    Destroy(tiles[x, y + 2]);

                }
            }
        }
       
    }
    //if (tiles[x, y].tag != "Player")
    //{
    //int currentColor = tiles[x, y].GetComponent<TileScript>().type;

    //GameObject middleTile = tiles[x, y];

    //if (x - 1 <= WIDTH - 1
    //    && x - 1 >= 0
    //    && x + 1 <= WIDTH - 1
    //    && x + 1 >= 0)
    //{

    //if (tiles[x - 1, y].tag != "Player"
    //&& tiles[x + 1, y].tag != "Player")
    //{
    //GameObject leftTile = tiles[x - 1, y];
    //GameObject rightTile = tiles[x + 1, y];

    //if (leftTile.GetComponent<TileScript>().type == currentColor && rightTile.GetComponent<TileScript>().type == currentColor)
    //{
    //middleTile.GetComponent<SpriteRenderer>().color = Color.gray;
    //leftTile.GetComponent<SpriteRenderer>().color = Color.gray;
    //rightTile.GetComponent<SpriteRenderer>().color = Color.gray;
    //return true;
    //        }
    //                    }
    //                }

    //                if (y - 1 <= HEIGHT - 1
    //                    && y - 1 >= 0
    //                    && y + 1 <= HEIGHT - 1
    //                    && y + 1 >= 0)
    //                {
    //                    if (tiles[x, y - 1].tag != "Player"
    //                       && tiles[x, y + 1].tag != "Player")
    //                    {

    //                        GameObject belowTile = tiles[x, y - 1];
    //                        GameObject aboveTile = tiles[x, y + 1];

    //                        if (aboveTile.GetComponent<TileScript>().type == currentColor && belowTile.GetComponent<TileScript>().type == currentColor)
    //                        {
    //                            middleTile.GetComponent<SpriteRenderer>().color = Color.gray;
    //                            belowTile.GetComponent<SpriteRenderer>().color = Color.gray;
    //                            aboveTile.GetComponent<SpriteRenderer>().color = Color.gray;
    //                            return true;
    //                        }
    //                    }
    //                }


    //            }
    //        }
    //    }
    //    return false;
    //}

    void ResetGrid()
    {

    }


    bool Repopulate()
    {
        bool repopulate = false;
        for (int x = 0; x < WIDTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                if (tiles[x, y] == null)
                {
                    repopulate = true;
                    if (y == 0)
                    {
                        GameObject newTile = Instantiate(tilePrefab);

                        newTile.transform.parent = gridHolder.transform;
                        newTile.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

                        tiles[x, y] = newTile;

                        TileScript tileScript = newTile.GetComponent<TileScript>();
                        tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
                        tileScript.coords = new Vector2Int(x, y);
                    }
                    else
                    {
                        slideLerp = 0;
                        tiles[x, y] = tiles[x, y - 1];
                        TileScript tileScript = tiles[x, y].GetComponent<TileScript>();

                        if (tileScript)
                        {
                            tileScript.SetUpSlide(new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset));
                            tileScript.coords = new Vector2Int(x, y);
                        }
                        if (tiles[x, y].tag == "Player")
                        {
                            player.GetComponent<playerScript>().playerCoords = new Vector2Int(x, y);
                        }
                        tiles[x, y - 1] = null;
                    }
                }
            }
        }
        return repopulate;
    }

    //void RemoveMatchedTiles()
    //{
    //    //    //after all tiles are turned gray, iterate through and delete them if in play mode, or if in start mode, change their color
    //    for (int x = 0; x < WIDTH; x++)
    //    {
    //        for (int y = 0; y < HEIGHT; y++)
    //        {
    //            if (tiles[x, y] != null)
    //            {
    //                if (tiles[x, y].GetComponent<SpriteRenderer>().color == Color.gray)
    //                {
    //                    GameObject middleTile = tiles[x, y];

    //                    if (x - 1 <= WIDTH - 1
    //                   && x - 1 >= 0
    //                   && x + 1 <= WIDTH - 1
    //                   && x + 1 >= 0)
    //                    {

    //                        if (tiles[x - 1, y].tag != "Player"
    //                    && tiles[x + 1, y].tag != "Player")
    //                        {
    //                            GameObject leftTile = tiles[x - 1, y];
    //                            GameObject rightTile = tiles[x + 1, y];
    //                            if (middleTile.GetComponent<SpriteRenderer>().color == leftTile.GetComponent<SpriteRenderer>().color && middleTile.GetComponent<SpriteRenderer>().color == rightTile.GetComponent<SpriteRenderer>().color)
    //                            {
    //                                Vector2Int coords = new Vector2Int(x, y);
    //                                //transformDict.Add(coords, tiles[x, y].transform);

    //                                GameObject newBurst = Instantiate(particleBurst);
    //                                newBurst.transform.parent = gridHolder.transform;
    //                                newBurst.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

    //                                score++;
    //                                player.GetComponent<playerScript>().turnsLeft = 6;


    //                                //tiles[x, y].GetComponent<SpriteRenderer>().enabled = false;
    //                                //tiles[x, y].GetComponent<TileScript>().type = -1;
    //                                Destroy(tiles[x, y]);
    //                                Destroy(tiles[x - 1, y]);
    //                                Destroy(tiles[x + 1, y]);
    //                            }
    //                        }
    //                        if (y - 1 <= HEIGHT - 1
    //                    && y - 1 >= 0
    //                    && y + 1 <= HEIGHT - 1
    //                    && y + 1 >= 0)
    //                        {
    //                            if (tiles[x, y - 1].tag != "Player"
    //                    && tiles[x, y + 1].tag != "Player")
    //                            {
    //                                GameObject aboveTile = tiles[x, y - 1];
    //                                GameObject belowTile = tiles[x, y + 1];
    //                                if (middleTile.GetComponent<SpriteRenderer>().color == aboveTile.GetComponent<SpriteRenderer>().color && middleTile.GetComponent<SpriteRenderer>().color == belowTile.GetComponent<SpriteRenderer>().color)
    //                                {
    //                                    Vector2Int coords = new Vector2Int(x, y);
    //                                    //transformDict.Add(coords, tiles[x, y].transform);

    //                                    GameObject newBurst = Instantiate(particleBurst);
    //                                    newBurst.transform.parent = gridHolder.transform;
    //                                    newBurst.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

    //                                    score++;
    //                                    player.GetComponent<playerScript>().turnsLeft = 6;


    //                                    //tiles[x, y].GetComponent<SpriteRenderer>().enabled = false;
    //                                    //tiles[x, y].GetComponent<TileScript>().type = -1;
    //                                    Destroy(tiles[x, y]);
    //                                    Destroy(tiles[x, y - 1]);
    //                                    Destroy(tiles[x, y + 1]);
    //                                }
    //                            }
    //                        }


    //                        //if (gameState == "start")
    //                        //{
    //                        //    //Debug.Log("Change " + x + "," + y);
    //                        //    TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
    //                        //    tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
    //                        //    CheckGridForMatches();
    //                        //}

    //                        //Debug.Log(x + ", " + y);

    //                        //tiles[x,y] = null;

    //                    }

    //                }

    //            }
    //        }
    //    }
    //}

        //    CheckGridForNulls();
        //}

        //void CheckGridForNulls()
        //{
        //    //iterate through the grid again and find null spaces and then call do a lerp
        //    for (int x = 0; x < WIDTH; x++)
        //    {
        //        for (int y = 0; y < HEIGHT; y++)
        //        { 

        //        //TileScript tileScript = tiles[x, y].GetComponent<TileScript>();
        //            if (tiles[x,y].GetComponent<TileScript>().type == -1)
        //            {

        //                Vector2Int coordsToFill = new Vector2Int(x, y);
        //               //Debug.Log(coordsToFill);
        //                DoALerp(coordsToFill);
        //            }
        //        }
        //    }

        //}

        //void DoALerp(Vector2Int coordsToFill)
        //{
        //    //iterate up the grid above the missing tile and adjust their position in the grid, and call the coroutine to lerp them down
        //    for (int i = 1; i < 7; i++)
        //    {
        //        if (coordsToFill.y - i >= 0)
        //        {
        //            if (tiles[coordsToFill.x, coordsToFill.y - i].GetComponent<TileScript>().type != -1)
        //            {
        //                // Debug.Log(coordsToFill.y - i);
        //                float lerpStart;
        //                float lerpEnd;
        //                lerpStart = coordCorrection(coordsToFill.y - i);
        //                lerpEnd = coordCorrection(coordsToFill.y - i + 1);
        //                Transform tileToMove = tiles[coordsToFill.x, coordsToFill.y - i].transform;
        //                StartCoroutine(OnlyLerp(lerpStart, lerpEnd, tileToMove));
        //                transformDict.Remove(coordsToFill);
        //                tiles[coordsToFill.x, coordsToFill.y - i + 1] = tiles[coordsToFill.x, coordsToFill.y - i];

        //            }
        //            //GameObject movedTile;
        //            //movedTile = tiles[coordsToFill.x, coordsToFill.y - i + 1];
        //            //TileScript tileScript = movedTile.GetComponent<TileScript>();
        //            //tileScript.coords = new Vector2Int(coordsToFill.x, coordsToFill.y - i + 1);
        //        }
        //    }
        //}

        //public IEnumerator OnlyLerp(float lerpStart, float lerpEnd, Transform tileToMove)
        //{
        //    //calculate the proper coordinates and then lerp down a tile one at a time
        //    float endTime = 1f;
        //    float elapsedTime = 0;
        //    //lerpStart = tiles[coordsToFill.x, coordsToFill.y - i].transform.localPosition.y;
        //    //lerpEnd = tiles[coordsToFill.x, coordsToFill.y - i + 1].transform.localPosition.y;
        //    //Debug.Log(lerpEnd + " " + i);
        //    while (elapsedTime < endTime)
        //    {
        //        elapsedTime += Time.deltaTime;
        //        //Debug.Log(elapsedTime);
        //       //Debug.Log("The Missing Tile: " + coordsToFill);
        //       //Debug.Log("The tile " + spot + " spaces above: " + tiles[coordsToFill.x, coordsToFill.y - spot].GetComponent<TileScript>().coords);
        //       //Debug.Log("x coord is " + coordsToFill.x + "y coord is " + (coordsToFill.y - spot));
        //            tileToMove.localPosition = new Vector3(
        //            tileToMove.localPosition.x,
        //            Mathf.Lerp(lerpStart, lerpEnd, elapsedTime), tileToMove.localPosition.z);


        //        yield return null;
        //    }
        //    Repopulate();
        //    //CheckGridForNulls();
        //    yield break;
        //}

        //int coordCorrection (int gridCoordsToChange)
        //{
        //    //calculate the difference between the grid position and the transform position
        //    int positionIntY = gridCoordsToChange;

        //    if (gridCoordsToChange == 0)
        //    {
        //        positionIntY = 4;
        //    }
        //    else if (gridCoordsToChange == 1)
        //    {
        //        positionIntY = 3;
        //    }
        //    else if (gridCoordsToChange == 2)
        //    {
        //        positionIntY = 2;
        //    }
        //    else if (gridCoordsToChange == 3)
        //    {
        //        positionIntY = 1;
        //    }
        //    else if (gridCoordsToChange == 4)
        //    {
        //        positionIntY = 0;
        //    }
        //    else if (gridCoordsToChange == 5)
        //    {
        //        positionIntY = -1;
        //    }
        //    else if (gridCoordsToChange == 6)
        //    {
        //        positionIntY = -2;
        //    }

        //    return positionIntY;
        //}

        //void Repopulate()
        //{

        //    for (int x = 0; x < WIDTH; x++)
        //    {
        //        for (int y = 0; y < HEIGHT; y++)
        //        {
        //            //Debug.Log(x + ", " + y);
        //            if (tiles[x, y].GetComponent<TileScript>().type == -1 && tiles[x, y].tag != "Player")
        //            {
        //                Debug.Log("Repopulate");
        //                Debug.Log(x + ", " + y);
        //                    GameObject newTile = Instantiate(tilePrefab);

        //                    newTile.transform.parent = gridHolder.transform;
        //                    newTile.transform.localPosition = new Vector2(WIDTH - x - xOffset, HEIGHT - y - yOffset);

        //                    tiles[x, y] = newTile;

        //                    TileScript tileScript = newTile.GetComponent<TileScript>();
        //                    tileScript.SetSprite(Random.Range(0, tileScript.tileColors.Length));
        //                    tileScript.coords = new Vector2Int(x, y);

        //                //else
        //                //{
        //                //    Vector2Int coordsToFill = new Vector2Int(x, y);
        //                //    StartCoroutine(LerpDown(coordsToFill));
        //                //}
        //            }
        //        }
        //    }
        //    CheckGridForNulls();
        //}


    }
