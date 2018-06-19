using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EcoManager : MonoBehaviour {
    public static EcoManager instance;
    public int pollution;
    public int basePollution;
    public int endGamePollution;
    public Image pollutionImage;
    public float polutionCheck;

    public int burnChance;

    [System.Serializable]
    public enum GroundState {
        fertile = 1,
        Grass = 2,
        Sand = 3,
        Water = 4,
        burned = 5,
        driedGround = 6
    }

    [Header("Genorator")]
    [HideInInspector] public ArraySlot[] Grid;
    GameObject[] treesInScene;
    public Transform parent;

    public static Material[] groundTextures;
    public static Sprite[] groundSprites;
    public static string[] groundDescription;
    public static string[] groundName;
    public List<Ground> groundTexturesInput = new List<Ground>();

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(gameObject);
        }

        groundTextures = new Material[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundTextures[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tex;
        }

        groundSprites = new Sprite[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundSprites[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].groundSprite;
        }

        groundDescription = new string[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundDescription[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].description;
        }

        groundName = new string[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundName[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tileName;
        }
        StartCoroutine(PollutionCheck());

        islandSize = GameManager.worldWidth;
        GenorateIsland();

        basePollution = pollution;
        AddPollution(0);
    }

    public void AddPollution(int PollutionToAdd) {
        pollution += PollutionToAdd;

        if (basePollution != 0) {
            pollutionImage.fillAmount = pollution / basePollution;
        }

        if (pollution >= endGamePollution) {
            //          ***EndGame
        }
    }

    public Tile GetTile(Vector3 pos) {
        return (Grid[Mathf.RoundToInt(pos.x / xStepSize)].myArray[Mathf.RoundToInt(pos.z / yStepSize)/ 2]);
    }

    #region Tile Genorator
    public int islandSize;
    public int xSize, ySize;
    public float xStepSize, yStepSize;
    public GameObject tile;
    public List<ToSpawn> toSpawnList = new List<ToSpawn>();
    public int islandLakesChance;
    public int percentOfFurtileGround;
    public int percentOfBurnedGround;
    public int percentOfDriedGround;
    List<Tile> grassTiles = new List<Tile>();
    List<Tile> sandTiles = new List<Tile>();
    List<Tile> driedTiles = new List<Tile>();
    List<Tile> burnedTiles = new List<Tile>();
    List<Tile> furtileTiles = new List<Tile>();
    List<Tile> tiles = new List<Tile>();
    public List<Tile> lowDryChance = new List<Tile>();
    public List<Tile> midDryChance = new List<Tile>();
    public List<Tile> highDryChance = new List<Tile>();

    [System.Serializable]
    public class ToSpawn {
        public List<GameObject> spawnedObjects = new List<GameObject>();
        public int amountToSpawn;
        public GameObject objectToSpawn;
        public bool x, y, z;
    }

    public void GenerateMap() {

        groundTextures = new Material[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundTextures[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tex;
        }

        groundSprites = new Sprite[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundSprites[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].groundSprite;
        }

        groundDescription = new string[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundDescription[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].description;
        }

        groundName = new string[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundName[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tileName;
        }

        Grid = new ArraySlot[xSize];
        bool pos = false;

        for (int x = 0; x < xSize; x++) {
            Grid[x] = new ArraySlot();
            Grid[x].myArray = new Tile[ySize];
            for (int y = 0; y < ySize; y++) {
                Vector3 positionInWorld = Vector3.zero;

                if (pos == true) {
                    positionInWorld = new Vector3(x * xStepSize, 0, y * yStepSize * 2);
                } else {
                    positionInWorld = new Vector3(x * xStepSize, 0, y * yStepSize * 2 + yStepSize);
                }

                GameObject newTile = Instantiate(tile, positionInWorld, tile.transform.rotation, parent);
                ThisTile t = newTile.GetComponent<ThisTile>();
                t.x = x;
                t.y = y;
                Grid[x].myArray[y] = new Tile(newTile, GroundState.Water, x, y);
            }
            pos = !pos;
        }
    }

    public void Serialize() {
        EditorUtility.SetDirty(this);
    }

    public void DestroyMap() {
        for (int i = 0; i < toSpawnList.Count; i++) {
            for (int ii = 0; ii < toSpawnList[i].spawnedObjects.Count; ii++) {
                if (toSpawnList[i].spawnedObjects[ii] != null) {
                    DestroyImmediate(toSpawnList[i].spawnedObjects[ii]);
                }
            }
            toSpawnList[i].spawnedObjects.Clear();
        }

        if (Grid != null) {
            for (int i = 0; i < Grid.Length; i++) {
                foreach (Tile tile in Grid[i].myArray) {
                    if (tile.myTile != null) {
                        DestroyImmediate(tile.myTile);
                    }
                }

            }

        }
    }

    public void GenorateIsland() {
        groundTextures = new Material[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundTextures[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tex;
        }

        groundSprites = new Sprite[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundSprites[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].groundSprite;
        }

        groundDescription = new string[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundDescription[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].description;
        }

        groundName = new string[groundTexturesInput.Count];
        for (int i = 0; i < groundTexturesInput.Count; i++) {
            groundName[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tileName;
        }

        grassTiles.Clear();
        driedTiles.Clear();
        furtileTiles.Clear();
        sandTiles.Clear();
        burnedTiles.Clear();
        tiles.Clear();

        if (islandSize > xSize * ySize) {

            Debug.Log("The Size Of The Island Is To Big For The Map Size");
            return;
        }

        Queue<Tile> toCheck = new Queue<Tile>();

        Tile startTile = Grid[xSize / 2].myArray[ySize / 2];
        toCheck.Enqueue(startTile);

        int landAmounts = 0;

        while (islandSize > landAmounts) {
            Tile dequeueTile = null;
            if (toCheck.Count > 0) {
                dequeueTile = toCheck.Dequeue();
            } else {
                return;
            }

            if (dequeueTile != null) {
                if (dequeueTile.currentState == GroundState.Water && Random.Range(0, 100)<= 10) {
                    dequeueTile.myTimeLine.Clear();
                    dequeueTile.ChangeMaterial(GroundState.Grass, "Grass Is A Fast Growing Plant Witch Can Grow If There Is Enough Water Around.");
                    dequeueTile.myTile.layer = LayerMask.NameToLayer("ground");
                    tiles.Add(dequeueTile);

                    grassTiles.Add(dequeueTile);
                    landAmounts++;

                    for (int i = -1; i < 2; i++) {
                        for (int ii = -1; ii < 2; ii++) {
                            if (Grid[dequeueTile.gridPosX + i].myArray[dequeueTile.gridPosY + ii].gridPosX > 0 &&
                                Grid[dequeueTile.gridPosX + i].myArray[dequeueTile.gridPosY + ii].gridPosX < xSize &&
                                Grid[dequeueTile.gridPosX + i].myArray[dequeueTile.gridPosY + ii].gridPosY > 0 &&
                                Grid[dequeueTile.gridPosX + i].myArray[dequeueTile.gridPosY + ii].gridPosY < ySize) {
                                toCheck.Enqueue(Grid[dequeueTile.gridPosX + i].myArray[dequeueTile.gridPosY + ii]);
                            }
                        }
                    }
                } else if (Random.Range(0, 100)<= islandLakesChance) {
                    toCheck.Enqueue(dequeueTile);
                }
            }
        }

        List<Tile> phase2GrassTiles = new List<Tile>();

        foreach (Tile t in grassTiles) {
            for (int i = -1; i < 2; i++) {
                for (int ii = -1; ii < 2; ii++) {
                    if (i == 0 || ii == 0) {
                        if (Grid[t.gridPosX + i].myArray[t.gridPosY + ii].currentState == GroundState.Water) {
                            if (Random.Range(0, 100)<= 70) {
                                t.myTimeLine.Clear();
                                t.ChangeMaterial(GroundState.Sand, "Water Pushes The Small Rocks To Land Witch Formes Beaches");
                                t.myTile.layer = LayerMask.NameToLayer("ground");
                                sandTiles.Add(t);
                            }
                            break;
                        }
                    }
                }
            }
            if (Grid[t.gridPosX].myArray[t.gridPosY].currentState == GroundState.Grass) {
                phase2GrassTiles.Add(t);
            }
        }

        foreach (Tile t in tiles) {
            Check(t);
        }

        for (int i = 0; i < toSpawnList.Count; i++) {
            int localToSpawn = toSpawnList[i].amountToSpawn;

            while (localToSpawn > 0) {
                if (phase2GrassTiles.Count == 0) {
                    Debug.Log("Not enough space for trees");
                    return;
                }

                Tile toPlant = phase2GrassTiles[Random.Range(0, phase2GrassTiles.Count)];

                toSpawnList[i].spawnedObjects.Add(Instantiate(toSpawnList[i].objectToSpawn, toPlant.myTile.transform.position, Quaternion.Euler(new Vector3(toSpawnList[i].x == true ? Random.Range(0, 360): toSpawnList[i].objectToSpawn.transform.localEulerAngles.x, toSpawnList[i].y == true ? Random.Range(0, 360): toSpawnList[i].objectToSpawn.transform.localEulerAngles.y, toSpawnList[i].z == true ? Random.Range(0, 360): toSpawnList[i].objectToSpawn.transform.localEulerAngles.z))));
                localToSpawn--;

                phase2GrassTiles.Remove(toPlant);
            }
        }
    }

    void Check(Tile t) {
        bool midRange = false;

        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Grid[t.gridPosX + i].myArray[t.gridPosY + ii].currentState == GroundState.Water) {
                    lowDryChance.Add(t);
                    return;
                }
                for (int r = -1; r < 2; r++) {
                    for (int rr = -1; rr < 2; rr++) {
                        if (Grid[t.gridPosX + r].myArray[t.gridPosY + rr].currentState == GroundState.Water) {
                            midRange = true;
                        }
                    }
                }
            }
        }
        if (midRange == true) {
            midDryChance.Add(t);

        } else {
            highDryChance.Add(t);
        }

    }

    #endregion

    [System.Serializable]
    public class Tile {
        public List<State> myTimeLine = new List<State>();
        public GroundState currentState;
        public int gridPosX, gridPosY;
        public GameObject myTile;
        Renderer myRenderer;
        public AddPolution myOccupant;

        public Tile(GameObject newTile, GroundState newState, int gridX, int gridY) {
            myTile = newTile;
            myRenderer = myTile.GetComponent<Renderer>();
            ChangeMaterial(newState, "How Did This Get Here.");
            gridPosX = gridX;
            gridPosY = gridY;
        }

        public void ChangeMaterial(GroundState newState, string newReason) {
            State s = new State();
            s.state = newState;
            s.reason = newReason;

            myTimeLine.Add(s);
            currentState = newState;
            if (myRenderer == null) {
                myRenderer = myTile.GetComponent<Renderer>();
            }
            myRenderer.material = groundTextures[(int)newState - 1];
        }
    }

    [System.Serializable]
    public struct State {
        public GroundState state;
        public string reason;
    }

    [System.Serializable]
    public struct Ground {
        public GroundState state;
        public Material tex;
        public Sprite groundSprite;
        public string description;
        public string tileName;
    }

    public void DryGrounds(Vector3 dryPosition, int radius, string reason, string downgradeReason, string downgradeReason2) {
        Tile tileToEdit = GetTile(dryPosition);

        Grid[tileToEdit.gridPosX].myArray[tileToEdit.gridPosY].ChangeMaterial(GroundState.driedGround, reason);

        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Random.Range(0, 100)<= 70 && (int)Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].currentState <= (int)GroundState.Grass) {
                    Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.driedGround, reason);

                    for (int z = -1; z < 2; z++) {
                        for (int zz = -1; zz < 2; zz++) {
                            if (Random.Range(0, 100)< 20 && (int)Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].currentState <= (int)GroundState.Grass) {
                                Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.driedGround, reason);
                            } else if (Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].currentState == GroundState.fertile) {
                                Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].ChangeMaterial(GroundState.Grass, downgradeReason2);
                            }
                        }
                    }
                } else if (Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].currentState == GroundState.fertile) {
                    Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.Grass, downgradeReason2);
                }
            }
        }
    }

    public void BurnGrounds(Vector3 burnPosition, int radius, string reason, string downgradeReason, string downgradeReason2) {
        Tile tileToEdit = GetTile(burnPosition);

        Grid[tileToEdit.gridPosX].myArray[tileToEdit.gridPosY].ChangeMaterial(GroundState.burned, reason);
        StartFire(Grid[tileToEdit.gridPosX].myArray[tileToEdit.gridPosY].myTile.transform.position);

        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Random.Range(0, 100)<= 70 && (int)Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].currentState <= (int)GroundState.Grass) {
                    Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.burned, reason);
                    StartFire(Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].myTile.transform.position);

                    for (int z = -1; z < 2; z++) {
                        for (int zz = -1; zz < 2; zz++) {
                            if (Random.Range(0, 100)<= 20 && (int)Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].currentState <= (int)GroundState.Grass) {
                                Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.driedGround, reason);
                            } else if (Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].currentState == GroundState.fertile) {
                                Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].ChangeMaterial(GroundState.Grass, downgradeReason2);
                            }
                        }
                    }
                } else if ((int)Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].currentState <= (int)GroundState.Grass) {
                    Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.driedGround, downgradeReason2);
                    for (int z = -1; z < 2; z++) {
                        for (int zz = -1; zz < 2; zz++) {
                            if (Random.Range(0, 100)<= 20 && (int)Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].currentState <= (int)GroundState.Grass) {
                                Grid[tileToEdit.gridPosX + i].myArray[tileToEdit.gridPosY + ii].ChangeMaterial(GroundState.driedGround, reason);
                            } else if (Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].currentState == GroundState.fertile) {
                                Grid[tileToEdit.gridPosX + i + z].myArray[tileToEdit.gridPosY + ii + zz].ChangeMaterial(GroundState.Grass, downgradeReason2);
                            }
                        }
                    }
                }
            }
        }
    }

    public void StartFire(Vector3 pos) {
        if (Random.Range(0, 100)<= burnChance) {
            //burn plz
        }
    }

    [System.Serializable]
    public struct ArraySlot {
        public Tile[] myArray;
    }

    public IEnumerator PollutionCheck() {

        yield return new WaitForSeconds(polutionCheck);


                        //***Fixxxxx
        for (int i = 0; i < basePollution / pollution * 2; i++){

        }

        StartCoroutine(PollutionCheck());
    }
}