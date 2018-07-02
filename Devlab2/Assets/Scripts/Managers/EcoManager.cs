using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public int daysSurvived;
    public int secondsPerDay;

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
    public TextMeshProUGUI myText;
    public GameObject interactorImage;

    IEnumerator myDays() {
        yield return new WaitForSeconds(secondsPerDay);
        daysSurvived++;
        myText.text = "Day: " + daysSurvived.ToString();
        StartCoroutine(myDays());
    }

    void Awake() {
        interactorImage.SetActive(false);
        StartCoroutine(myDays());
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

        islandSize = GameManager.worldWidth;

        basePollution = pollution;
        AddPollution(0);
    }

    void Start() {
        GenerateMap();
        GenorateIsland();
        basePollution = pollution;
        StartCoroutine(PollutionCheck());
    }

    private void Update() {
        if (Input.GetKeyDown("c")) {
            pollution = -1;
            AddPollution(0);
            polutionCheck = 0.5f;
        }
    }

    public void AddPollution(int PollutionToAdd) {
        pollution += PollutionToAdd;

        if (basePollution != 0) {
            pollutionImage.fillAmount = basePollution - (pollution / basePollution);
        }

        if (pollution >= endGamePollution) {
            UIManager.instance.EndGame();
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
    List<Tile> lowDryChance = new List<Tile>();
    List<Tile> highDryChance = new List<Tile>();

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
                t.SetThis(GroundState.Water, x, y);
                Grid[x].myArray[y] = new Tile();
                Grid[x].myArray[y].myTile = t;
            }
            pos = !pos;
        }
    }

    public void Serialize() {
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
#endif
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
                        DestroyImmediate(tile.myTile.myTile);
                    }
                }
            }
        }
    }

    public void GenorateIsland() {
        islandSize = GameManager.worldWidth;

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
        startTile.myTile.myTimeLine.Clear();
        startTile.myTile.ChangeMaterial(GroundState.Grass, "Grass Is A Fast Growing Plant Witch Can Grow If There Is Enough Water Around.");
        startTile.myTile.myTile.layer = LayerMask.NameToLayer("ground");
        tiles.Add(startTile);

        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.gridPosX > 0 &&
                    Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.gridPosX < xSize &&
                    Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.gridPosY > 0 &&
                    Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.gridPosY < ySize) {
                    Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.myTimeLine.Clear();
                    Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.ChangeMaterial(GroundState.Grass, "Grass Is A Fast Growing Plant Witch Can Grow If There Is Enough Water Around.");
                    Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii].myTile.myTile.layer = LayerMask.NameToLayer("ground");
                    tiles.Add(Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii]);

                    for (int l = -1; l < 2; l++) {
                        for (int ll = -1; ll < 2; ll++) {
                            if (Grid[startTile.myTile.gridPosX + i + l].myArray[startTile.myTile.gridPosY + ii + ll].myTile.gridPosX > 0 &&
                                Grid[startTile.myTile.gridPosX + i + l].myArray[startTile.myTile.gridPosY + ii + ll].myTile.gridPosX < xSize &&
                                Grid[startTile.myTile.gridPosX + i + l].myArray[startTile.myTile.gridPosY + ii + ll].myTile.gridPosY > 0 &&
                                Grid[startTile.myTile.gridPosX + i + l].myArray[startTile.myTile.gridPosY + ii + ll].myTile.gridPosY < ySize) {
                                toCheck.Enqueue(Grid[startTile.myTile.gridPosX + i + l].myArray[startTile.myTile.gridPosY + ii + ll]);
                            }
                        }
                    }
                    toCheck.Enqueue(Grid[startTile.myTile.gridPosX + i].myArray[startTile.myTile.gridPosY + ii]);
                }
            }
        }

        int landAmounts = 0;

        while (islandSize > landAmounts) {
            Tile dequeueTile = null;
            if (toCheck.Count > 0) {
                dequeueTile = toCheck.Dequeue();
            } else {
                return;
            }

            if (dequeueTile != null) {
                if (dequeueTile.myTile.currentState == GroundState.Water && Random.Range(0, 100)<= 10) {
                    dequeueTile.myTile.myTimeLine.Clear();
                    dequeueTile.myTile.ChangeMaterial(GroundState.Grass, "Grass Is A Fast Growing Plant Witch Can Grow If There Is Enough Water Around.");
                    dequeueTile.myTile.myTile.layer = LayerMask.NameToLayer("ground");
                    tiles.Add(dequeueTile);

                    grassTiles.Add(dequeueTile);
                    landAmounts++;

                    for (int i = -1; i < 2; i++) {
                        for (int ii = -1; ii < 2; ii++) {
                            if (Grid[dequeueTile.myTile.gridPosX + i].myArray[dequeueTile.myTile.gridPosY + ii].myTile.gridPosX > 0 &&
                                Grid[dequeueTile.myTile.gridPosX + i].myArray[dequeueTile.myTile.gridPosY + ii].myTile.gridPosX < xSize &&
                                Grid[dequeueTile.myTile.gridPosX + i].myArray[dequeueTile.myTile.gridPosY + ii].myTile.gridPosY > 0 &&
                                Grid[dequeueTile.myTile.gridPosX + i].myArray[dequeueTile.myTile.gridPosY + ii].myTile.gridPosY < ySize) {
                                toCheck.Enqueue(Grid[dequeueTile.myTile.gridPosX + i].myArray[dequeueTile.myTile.gridPosY + ii]);
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
                        if (Grid[t.myTile.gridPosX + i].myArray[t.myTile.gridPosY + ii].myTile.currentState == GroundState.Water) {
                            if (Random.Range(0, 100)<= 70) {
                                t.myTile.myTimeLine.Clear();
                                t.myTile.ChangeMaterial(GroundState.Sand, "Water Pushes The Small Rocks To Land Witch Formes Beaches");
                                t.myTile.myTile.layer = LayerMask.NameToLayer("ground");
                                sandTiles.Add(t);
                            }
                            break;
                        }
                    }
                }
            }
            if (Grid[t.myTile.gridPosX].myArray[t.myTile.gridPosY].myTile.currentState == GroundState.Grass) {
                phase2GrassTiles.Add(t);
            }
        }

        for (int i = 0; i < toSpawnList.Count; i++) {
            int localToSpawn = toSpawnList[i].amountToSpawn * (islandSize / 1000);

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
        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Grid[t.myTile.gridPosX + i].myArray[t.myTile.gridPosY + ii].myTile.currentState == GroundState.Water) {
                    lowDryChance.Add(t);
                    return;
                }
                for (int r = -1; r < 2; r++) {
                    for (int rr = -1; rr < 2; rr++) {
                        if (Grid[t.myTile.gridPosX + r].myArray[t.myTile.gridPosY + rr].myTile.currentState == GroundState.Water) {}
                    }
                }
            }
        }
        highDryChance.Add(t);
    }

    #endregion

    [System.Serializable]
    public class Tile {
        public ThisTile myTile;
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

        Grid[tileToEdit.myTile.gridPosX].myArray[tileToEdit.myTile.gridPosY].myTile.ChangeMaterial(GroundState.driedGround, reason);

        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Random.Range(0, 100)<= 3 && (int)Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.currentState <= (int)GroundState.Grass) {
                    Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.ChangeMaterial(GroundState.driedGround, reason);

                    for (int z = -1; z < 2; z++) {
                        for (int zz = -1; zz < 2; zz++) {
                            if (Random.Range(0, 100)< 20 && (int)Grid[tileToEdit.myTile.gridPosX + i + z].myArray[tileToEdit.myTile.gridPosY + ii + zz].myTile.currentState <= (int)GroundState.Grass) {
                                Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.ChangeMaterial(GroundState.driedGround, reason);
                            } else if (Grid[tileToEdit.myTile.gridPosX + i + z].myArray[tileToEdit.myTile.gridPosY + ii + zz].myTile.currentState == GroundState.fertile) {
                                Grid[tileToEdit.myTile.gridPosX + i + z].myArray[tileToEdit.myTile.gridPosY + ii + zz].myTile.ChangeMaterial(GroundState.Grass, downgradeReason2);
                            }
                        }
                    }
                } else if (Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.currentState == GroundState.fertile) {
                    Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.ChangeMaterial(GroundState.Grass, downgradeReason2);
                }
            }
        }
    }

    public void BurnGrounds(Vector3 burnPosition, int radius, string reason, string downgradeReason, string downgradeReason2) {
        Tile tileToEdit = GetTile(burnPosition);

        Grid[tileToEdit.myTile.gridPosX].myArray[tileToEdit.myTile.gridPosY].myTile.ChangeMaterial(GroundState.burned, reason);
        Grid[tileToEdit.myTile.gridPosX].myArray[tileToEdit.myTile.gridPosY].myTile.burn();

        for (int i = -1; i < 2; i++) {
            for (int ii = -1; ii < 2; ii++) {
                if (Random.Range(0, 100)<= 3) {
                    if ((int)Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.currentState == (int)GroundState.Grass || (int)Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.currentState == (int)GroundState.fertile) {
                        Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.ChangeMaterial(GroundState.burned, reason);
                        Grid[tileToEdit.myTile.gridPosX + i].myArray[tileToEdit.myTile.gridPosY + ii].myTile.burn();
                    }
                }
            }
        }
    }

    [System.Serializable]
    public struct ArraySlot {
        public Tile[] myArray;
    }

    public IEnumerator PollutionCheck() {

        yield return new WaitForSeconds(polutionCheck);

        //Grass Ground
        for (int i = 0; i < -pollution / -basePollution * 10; i++) {
            if (driedTiles.Count - 1 > 0) {
                int selected = Random.Range(0, driedTiles.Count - 1);
                driedTiles[selected].myTile.ChangeMaterial(GroundState.Grass, "No Water Nearby To Stop The Fire");
                grassTiles.Add(driedTiles[selected]);
                driedTiles.RemoveAt(selected);
            }
        }

        //Furtile Ground
        for (int i = 0; i < -pollution / -basePollution * 5; i++) {
            if (burnedTiles.Count - 1 > 0) {
                int selected = Random.Range(0, burnedTiles.Count - 1);
                burnedTiles[selected].myTile.ChangeMaterial(GroundState.fertile, "This Ground Is Furtile Because It Has Been Burned Lately");
                furtileTiles.Add(burnedTiles[selected]);
                burnedTiles.RemoveAt(selected);
            } else if (grassTiles.Count - 1 > 0) {
                int selected = Random.Range(0, grassTiles.Count - 1);
                grassTiles[selected].myTile.ChangeMaterial(GroundState.fertile, "This Ground Is Furtile Because It Is Close Enough To A Water Source Or The Ground Is Moist Enough.");
                furtileTiles.Add(grassTiles[selected]);
                grassTiles.RemoveAt(selected);
            }
        }

        //Dried Ground
        for (int i = 0; i < -basePollution / -pollution * 0.5; i++) {
            if (grassTiles.Count - 1 > 0) {
                int selected = Random.Range(0, grassTiles.Count - 1);
                grassTiles[selected].myTile.myTile.layer = LayerMask.NameToLayer("ground");
                grassTiles[selected].myTile.ChangeMaterial(GroundState.driedGround, "Air Is To Dry.");
                driedTiles.Add(grassTiles[selected]);
                grassTiles.RemoveAt(selected);
            }
        }

        //Burned Ground
        for (int i = 0; i < -basePollution / -pollution * 0.25; i++) {
            if (grassTiles.Count - 1 > 0) {
                int selected = Random.Range(0, grassTiles.Count - 1);
                grassTiles[selected].myTile.myTile.layer = LayerMask.NameToLayer("ground");
                grassTiles[selected].myTile.ChangeMaterial(GroundState.burned, "No Water Nearby To Stop The Fire");
                burnedTiles.Add(grassTiles[selected]);
                grassTiles.RemoveAt(selected);
            }
        }

        StartCoroutine(PollutionCheck());
    }
}