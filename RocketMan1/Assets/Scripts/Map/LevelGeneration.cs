using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class LevelGenerator : MonoBehaviour
{
    [Header("---Pathfinding")]
    [SerializeField] private AstarPath ap;
    [SerializeField] private float mapScanDelayTime;
    private GridGraph gg;

    [Header("---Player")]
    [SerializeField] private GameObject player;

    [Header("---Map")]
    [SerializeField] private GameObject startPoint;
    [SerializeField] private GameObject endPoint;
    [SerializeField] private GameObject background;

    GameObject b;

    [SerializeField] private List<GameObject> map;
    [HideInInspector] public List<GameObject> tempMap;

    [SerializeField] private GameObject tileSpawner;
    
    [SerializeField] private Vector2 mapSize;
    [SerializeField] private float targetCoverage;

    private GameObject spawnerPosition;

    private GameObject tilesInMap;
    private GameObject tileSpawnersInMap;
    private GameObject fillerBlocks;
    private GameObject bulletsInMap;

    [Header("---Tiles")]
    [SerializeField] private GameObject closer;
    [SerializeField] private GameObject[] L_tiles;
    [SerializeField] private GameObject[] R_tiles;
    [SerializeField] private GameObject[] T_tiles;
    [SerializeField] private GameObject[] B_tiles;

    private List<GameObject[]> tiles;

    //[Header("---Misc-Debugger")]
    [HideInInspector] public List<GameObject> spawners;
    [HideInInspector] public List<GameObject> openings;
    [HideInInspector] public List<GameObject> newOpenings;

    Dictionary<string, int> openingType;

    private bool tileSpawnerGenerated;

    Dictionary<Vector2, GameObject> spawnerPositions;


    private int randPos;
    private int randTileList;
    private int randTile;

    private GameObject startingSpawner;
    private GameObject[] randList;
    private GameObject startTile;

    public List<GameObject> Map { get => map; set => map = value; }
    public GameObject Player { get => player; set => player = value; }

    private void Start()
    {
        player.SetActive(false);

        gg = ap.data.gridGraph;
        
        openingType = new Dictionary<string, int>()
        {
            {"Opening", 0},
            {"Opening (1)", 1},
            {"Opening (2)", 2},
            {"Opening (3)", 3},
        };

        tiles = new List<GameObject[]>();
        tiles.Add(L_tiles);
        tiles.Add(R_tiles);
        tiles.Add(T_tiles);
        tiles.Add(B_tiles);

        tilesInMap = new GameObject("TilesInMap");
        tileSpawnersInMap = new GameObject("SpawnersInMap");
        fillerBlocks = new GameObject("FillerBlock");
        bulletsInMap = new GameObject("BulletsInMap");

        tileSpawnersInMap.transform.parent = gameObject.transform;
        tilesInMap.transform.parent = gameObject.transform;
        fillerBlocks.transform.parent = gameObject.transform;
        bulletsInMap.transform.parent = gameObject.transform;
    }


    private void ResetScene()
    {
        foreach (GameObject tile in map)
        {
            Destroy(tile);
        }
        Destroy(b);

        foreach (GameObject spawner in spawners)
        {
            if (spawner == null)
            {
                continue;
            }
            spawner.GetComponent<TileSpawner>().HasSpawned = false;
            Destroy(spawner);
        }

        player.SetActive(false);

        tiles = new List<GameObject[]>();
        tiles.Add(L_tiles);
        tiles.Add(R_tiles);
        tiles.Add(T_tiles);
        tiles.Add(B_tiles);

        tileSpawnerGenerated = false;
        spawners.Clear();
        spawners = new List<GameObject>();
        openings.Clear();
        openings = new List<GameObject>();
        map.Clear();
        map = new List<GameObject>();
        tempMap.Clear();
        tempMap = new List<GameObject>();
    }

    IEnumerator ScanMap() 
    {
        yield return new WaitForSeconds(mapScanDelayTime); // Wait for 0.2 seconds

        ap.Scan(); // Scan the graph
    }

    private void FinishMap()
    {
        foreach (GameObject t in map)
        {
            if (t.GetComponent<Tile>() == null)
            {
                continue;
            }

            foreach (GameObject enemySpawner in t.GetComponent<Tile>().EnemySpanwers)
            {
                if (enemySpawner != null)
                {
                    enemySpawner.GetComponent<EnemySpawner>().CanSpawn = true;
                }
            }
        }

        gg.center = new Vector3(Mathf.FloorToInt((mapSize.x * 20) / 2), Mathf.FloorToInt((mapSize.y * 20) / 2), 10f);

        gg.SetDimensions(200,200,1f);

        map.Add(b);
        StartCoroutine(ScanMap());
    }

    private void SetStartAndFinish()
    {
        // get start
        GameObject startingTile = map[0];

        // get finish
        GameObject finishingTile = null;

        for (int i = map.Count - 1; i >= 0; i--)
        {
            if (map[i].GetComponent<Tile>() != null)
            {
                finishingTile = map[i];
                break;
            }
        }

        startingTile.GetComponent<Tile>().StartTile = true;
        finishingTile.GetComponent<Tile>().FinishTile = true;

        GameObject sp = Instantiate(startPoint, startingTile.transform.position, Quaternion.identity);
        GameObject ep = Instantiate(endPoint, finishingTile.transform.position, Quaternion.identity);

        map.Add(sp);
        map.Add(ep);

        sp.transform.parent = gameObject.transform;
        ep.transform.parent = gameObject.transform;


        player.transform.position = startingTile.transform.position;

        player.SetActive(true);

        Camera.main.GetComponent<CamFollowPlayer>().Target = player.transform;
    }

    private void FillEmptySpaces()
    {
        foreach (GameObject spawner in spawners)
        {
            if (!spawner.GetComponent<TileSpawner>().HasSpawned)
            {
                GameObject b = Instantiate(closer, spawner.transform.position, Quaternion.identity);
                map.Add(b);

                b.transform.parent = fillerBlocks.gameObject.transform;

                continue;
            }
        }
    }

    private void ValidateMap()
    {
        // calulate amount of spawners that has spawned a tile
        int count = 0;
        foreach (GameObject spawner in spawners)
        {
            if (spawner.GetComponent<TileSpawner>().HasSpawned)
            {
                count++;
            }
        }

        if (spawners.Count <= 0)
        {
            Debug.Log("spawner count = 0");
        }
        else
        {
            float percentage = (float)count / (float)spawners.Count * 100f;
            if ((percentage >= targetCoverage) || (percentage >= targetCoverage - 5))
            {
                FillEmptySpaces();
                SetStartAndFinish();
                return;
            }
            else
            {
                Button_Start();
            }
        }

    }

    /// <summary>
    /// spawn correct tiles in correct position
    /// </summary>

    private void SpawnTiles(GameObject spawner, GameObject tileOpening)
    {
        GameObject newTile;
        GameObject tile;

        // if opening type == left
        if (openingType[tileOpening.name] == 0)
        {
            // spawn tile with right opening
            int rand = Random.Range(0, R_tiles.Length);

            tile = R_tiles[rand];

            if (tile.GetComponent<Tile>() != null)
            {
                // get tile spawn chance
                float spawnTileChance = tile.GetComponent<Tile>().spawnChance;
                // get random spawn threshold
                float randSpawnThreshold = Random.Range(0.0f, 1.0f);
                // if spawn chance > spawn threshold
                if (spawnTileChance > randSpawnThreshold)
                {
                    // spawn tile
                    newTile = Instantiate(tile, spawner.transform.position, Quaternion.identity);
                    tempMap.Add(newTile);

                    newTile.transform.parent = tilesInMap.gameObject.transform;
                }
                else
                {
                    SpawnTiles(spawner, tileOpening);
                }

            }
            else
            {
                newTile = Instantiate(closer, spawner.transform.position, Quaternion.identity);
                tempMap.Add(newTile);

                newTile.transform.parent = tilesInMap.gameObject.transform;
            }
        }

        // if opening type == right
        if (openingType[tileOpening.name] == 1)
        {
            // spawn tile with left opening
            int rand = Random.Range(0, L_tiles.Length);

            tile = L_tiles[rand];

            if (tile.GetComponent<Tile>() != null)
            {
                // get tile spawn chance
                float spawnTileChance = tile.GetComponent<Tile>().spawnChance;
                // get random spawn threshold
                float randSpawnThreshold = Random.Range(0.0f, 1.0f);
                // if spawn chance > spawn threshold
                if (spawnTileChance > randSpawnThreshold)
                {
                    // spawn tile
                    newTile = Instantiate(tile, spawner.transform.position, Quaternion.identity);
                    tempMap.Add(newTile);

                    newTile.transform.parent = tilesInMap.gameObject.transform;

                }
                else
                {
                    SpawnTiles(spawner, tileOpening);
                }

            }
            else
            {
                newTile = Instantiate(closer, spawner.transform.position, Quaternion.identity);
                tempMap.Add(newTile);

                newTile.transform.parent = tilesInMap.gameObject.transform;
            }
        }

        // if opening type == top
        if (openingType[tileOpening.name] == 2)
        {
            // spawn tile with bottom opening
            int rand = Random.Range(0, B_tiles.Length);

            tile = B_tiles[rand];

            if (tile.GetComponent<Tile>() != null)
            {
                // get tile spawn chance
                float spawnTileChance = tile.GetComponent<Tile>().spawnChance;
                // get random spawn threshold
                float randSpawnThreshold = Random.Range(0.0f, 1.0f);
                // if spawn chance > spawn threshold
                if (spawnTileChance > randSpawnThreshold)
                {
                    // spawn tile
                    newTile = Instantiate(tile, spawner.transform.position, Quaternion.identity);
                    tempMap.Add(newTile);

                    newTile.transform.parent = tilesInMap.gameObject.transform;
                }
                else
                {
                    SpawnTiles(spawner, tileOpening);
                }

            }
            else
            {
                newTile = Instantiate(closer, spawner.transform.position, Quaternion.identity);
                tempMap.Add(newTile);

                newTile.transform.parent = tilesInMap.gameObject.transform;
            }
        }

        // if opening type == bottom
        if (openingType[tileOpening.name] == 3)
        {
            // spawn tile with top opening
            int rand = Random.Range(0, T_tiles.Length);

            tile = T_tiles[rand];

            if (tile.GetComponent<Tile>() != null)
            {
                // get tile spawn chance
                float spawnTileChance = tile.GetComponent<Tile>().spawnChance;
                // get random spawn threshold
                float randSpawnThreshold = Random.Range(0.0f, 1.0f);
                // if spawn chance > spawn threshold
                if (spawnTileChance > randSpawnThreshold)
                {
                    // spawn tile
                    newTile = Instantiate(tile, spawner.transform.position, Quaternion.identity);
                    tempMap.Add(newTile);

                    newTile.transform.parent = tilesInMap.gameObject.transform;

                }
                else
                {
                    SpawnTiles(spawner, tileOpening);
                }
            }
            else
            {
                newTile = Instantiate(closer, spawner.transform.position, Quaternion.identity);
                tempMap.Add(newTile);

                newTile.transform.parent = tilesInMap.gameObject.transform;
            }
        }

        spawner.GetComponent<TileSpawner>().HasSpawned = true;
    }

    /// <summary>
    /// place enemy spawner
    /// </summary>
    private void PlaceEnemySpawners() 
    {
        // loop through tiles in map
        foreach (GameObject t in map)
        {
            if (t.GetComponent<Tile>() == null)
            {
                continue;
            }

            if (t.GetComponent<Tile>().EnemySpanwers.Length == 0)
            {
                continue;
            }

            if (t.GetComponent<Tile>().StartTile || t.GetComponent<Tile>().FinishTile) 
            {
                continue;
            }


            int randEnemySpawner = Random.Range(0, t.GetComponent<Tile>().EnemySpanwers.Length);

            int spawnEnemySpawner = Random.Range(0,10);

            if (spawnEnemySpawner < 1)
            {
                // spawn all of the above
                foreach (GameObject spawner in t.GetComponent<Tile>().EnemySpanwers)
                {
                    if (spawner != null)
                    {
                        spawner.SetActive(true);
                    }
                }
                continue;
            }
            else if (spawnEnemySpawner >= 1 && spawnEnemySpawner < 4)
            {
                // spawn random one enemy spanwer
                foreach (GameObject spawner in t.GetComponent<Tile>().EnemySpanwers)
                {
                    // deactivate all spawners
                    if (spawner != null)
                    {
                        spawner.SetActive(false);
                    }
                }

                // activate chosen spawner
                t.GetComponent<Tile>().EnemySpanwers[randEnemySpawner].SetActive(true);
            }
            else
            {
                // spawn nothing
                continue;
            }

        }
        return;
        
    }

    /// <summary>
    /// instantiate tiles
    /// </summary>

    private void PlaceNewTiles(int r)
    {
        // look at each tile in map
        foreach (GameObject tile in map)
        {
            if (tile.GetComponent<Tile>() != null)
            {
                // get openings
                GameObject[] tileOpenings = tile.GetComponent<Tile>().Openings;

                // loop through openings
                foreach (GameObject tileOpening in tileOpenings)
                {
                    // get spawner game objects via opening position
                    if (!spawnerPositions.ContainsKey(tileOpening.GetComponent<Opening>().Position))
                    {
                        if (tileOpening.GetComponent<Opening>().BlockerAdded == false)
                        {
                            GameObject newTile = Instantiate(closer, tileOpening.transform.position, Quaternion.identity);
                            tempMap.Add(newTile);
                            tileOpening.GetComponent<Opening>().BlockerAdded = true;


                            newTile.transform.parent = fillerBlocks.gameObject.transform;
                        }
                        continue;
                    }

                    // look up spawner game object in dictionary
                    GameObject spawner = spawnerPositions[tileOpening.transform.position];


                    // if spawner not spawned tile
                    if (!spawner.GetComponent<TileSpawner>().HasSpawned)
                    {
                        SpawnTiles(spawner, tileOpening);
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }

        // add temp map to map
        foreach (GameObject tm in tempMap)
        {
            map.Add(tm);
        }

        tempMap.Clear();

        if (r <= 0)
        {
            // validate map
            ValidateMap();
            PlaceEnemySpawners();
            FinishMap();
            return;
        }
        else
        {
            PlaceNewTiles(r -= 1);
        }
    }

    /// <summary>
    /// generate map
    /// </summary>

    private void Generate()
    {
        // select random starting pos
        randPos = Random.Range(0, spawners.Count);
        startingSpawner = spawners[randPos];


        // spawn random starting tile
        randTileList = Random.Range(0, tiles.Count);

        randList = null;
        randList = (GameObject[])tiles[randTileList].Clone();


        randTile = Random.Range(0, randList.Length);


        startTile = Instantiate(randList[randTile], startingSpawner.transform.position, Quaternion.identity);

        map.Add(startTile);

        startTile.transform.parent = tilesInMap.gameObject.transform;

        startingSpawner.GetComponent<TileSpawner>().HasSpawned = true;

        int f = Mathf.RoundToInt(mapSize.x * mapSize.y);

        PlaceNewTiles(Mathf.RoundToInt(f * 0.25f));

    }

    /// <summary>
    /// generate grid
    /// </summary>

    public void Button_Start()
    {
        ResetScene();

        background.GetComponent<Transform>().localScale = new Vector2((mapSize.x*20),(mapSize.y*20));
        Vector3 backgroundPosition = new Vector3(Mathf.FloorToInt((mapSize.x*20)/2), Mathf.FloorToInt((mapSize.y*20)/2), 10f);
        b = Instantiate(background, backgroundPosition, Quaternion.identity);

        spawnerPositions = new Dictionary<Vector2, GameObject>();

        if (!tileSpawnerGenerated)
        {
            for (int x = 0; x < mapSize.x; x++)
            {
                for (int y = 0; y < mapSize.y; y++)
                {
                    spawnerPosition = Instantiate(tileSpawner,
                        new Vector3(x * 20 + transform.position.x, y * 20 + transform.position.y, 0.8f),
                        Quaternion.identity);

                    spawnerPosition.name = $"{x}  {y}";

                    spawners.Add(spawnerPosition);

                    spawnerPositions.Add(spawnerPosition.transform.position, spawnerPosition);

                    spawnerPosition.transform.parent = tileSpawnersInMap.gameObject.transform;
                }
            }
            tileSpawnerGenerated = true;
        }

        Generate();
    }



}
