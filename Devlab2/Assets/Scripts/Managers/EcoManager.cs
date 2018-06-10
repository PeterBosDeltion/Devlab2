using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcoManager : MonoBehaviour {
    public static EcoManager instance;
    public int pollution;
    public int endGamePollution;

    [System.Serializable]
    public enum GroundState {
        Grass = 1,
        Sand = 2,
        Water = 3,
        burned = 4,
        fertile = 5,
        driedGround = 6
    }

    public Tile[,] Grid;
    GameObject[] treesInScene;

    public static Material[] groundTextures;
    public static Sprite[] groundSprites;
    public static string[] groundDescription;
    public static string[] groundName;
    public List<Ground> groundTexturesInput = new List<Ground>();

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(gameObject);
        }

        GenerateMap();
    }

    public void AddPollution(int PollutionToAdd) {
        pollution += PollutionToAdd;

        if(pollution >= endGamePollution){
            //          ***EndGame
        }
    }

    public Tile GetTile(Vector3 pos) {
        return (Grid[Mathf.RoundToInt(pos.x / xStepSize) / 2, Mathf.RoundToInt(pos.z / yStepSize) / 2]);
    }

    #region Tile Genorator
    public int edgeOffset;
    public int islandSize;
    public int xSize, ySize;
    public float xStepSize, yStepSize;
    public GameObject tile;
    public List<ToSpawn> toSpawnList = new List<ToSpawn>();

    [System.Serializable]
    public class ToSpawn {
        public List<GameObject> spawnedObjects = new List<GameObject>();
        public int amountToSpawn;
        public GameObject objectToSpawn;
        public bool x, y, z;
    }

    public void GenerateMap() {

        groundTextures = new Material[groundTexturesInput.Count];
        for(int i = 0; i < groundTexturesInput.Count; i++) {
            groundTextures[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tex;
        }

        groundSprites = new Sprite[groundTexturesInput.Count];
        for(int i = 0; i < groundTexturesInput.Count; i++) {
            groundSprites[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].groundSprite;
        }

        groundDescription = new string[groundTexturesInput.Count];
        for(int i = 0; i < groundTexturesInput.Count; i++) {
            groundDescription[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].description;
        }

        groundName = new string[groundTexturesInput.Count];
        for(int i = 0; i < groundTexturesInput.Count; i++) {
            groundName[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tileName;
        }

        DestroyMap();

        Grid = new Tile[xSize, ySize];
        bool pos = false;

        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {
                Vector3 positionInWorld = Vector3.zero;

                if(pos == true) {
                    positionInWorld = new Vector3(x * xStepSize, 0, y * yStepSize * 2);
                }
                else {
                    positionInWorld = new Vector3(x * xStepSize, 0, y * yStepSize * 2 + yStepSize);
                }

                GameObject newTile = Instantiate(tile, positionInWorld, tile.transform.rotation);
                ThisTile t = newTile.GetComponent<ThisTile>();
                t.x = x;
                t.y = y;
                Grid[x, y] = new Tile(newTile, GroundState.Water, x, y);
            }
            pos = !pos;
        }

        if(islandSize > xSize * ySize - (edgeOffset * 4)) {

            Debug.Log("The Size Of The Island Is To Big For The Map Size");
            return;
        }

        Tile startTile = Grid[xSize / 2, ySize / 2];
        startTile.ChangeMaterial(GroundState.Grass, "Grass Is A Fast Growing Plant Witch Can Grow If There Is Enough Water Around.");

        Queue<Tile> toCheck = new Queue<Tile>();
        List<Tile> grassTiles = new List<Tile>();

        for(int i = -1; i < 2; i++) {
            for(int ii = -1; ii < 2; ii++) {
                if(Grid[startTile.gridPosX + i, startTile.gridPosY + ii].gridPosX > edgeOffset && Grid[startTile.gridPosX + i, startTile.gridPosY + ii].gridPosX < xSize - edgeOffset && Grid[startTile.gridPosX + i, startTile.gridPosY + ii].gridPosY > edgeOffset && Grid[startTile.gridPosX + i, startTile.gridPosY + ii].gridPosY < ySize - edgeOffset) {
                    toCheck.Enqueue(Grid[startTile.gridPosX + i, startTile.gridPosY + ii]);
                }
            }
        }


        int landAmounts = 0;

        while(islandSize > landAmounts) {
            Tile dequeueTile = null;
            if(toCheck.Count > 0) {
                dequeueTile = toCheck.Dequeue();
            }
            else {
                return;
            }

            if(dequeueTile != null) {
                if(dequeueTile.currentState == GroundState.Water && Random.Range(0, 100) <= 10) {
                    dequeueTile.ChangeMaterial(GroundState.Grass, "Grass Is A Fast Growing Plant Witch Can Grow If There Is Enough Water Around.");
                    dequeueTile.myTile.layer = LayerMask.NameToLayer("ground");

                    grassTiles.Add(dequeueTile);
                    landAmounts++;

                    for(int i = -1; i < 2; i++) {
                        for(int ii = -1; ii < 2; ii++) {
                            if(Grid[dequeueTile.gridPosX + i, dequeueTile.gridPosY + ii].gridPosX > edgeOffset && Grid[dequeueTile.gridPosX + i, dequeueTile.gridPosY + ii].gridPosX < xSize - edgeOffset && Grid[dequeueTile.gridPosX + i, dequeueTile.gridPosY + ii].gridPosY > edgeOffset && Grid[dequeueTile.gridPosX + i, dequeueTile.gridPosY + ii].gridPosY < ySize - edgeOffset) {
                                toCheck.Enqueue(Grid[dequeueTile.gridPosX + i, dequeueTile.gridPosY + ii]);
                            }
                        }
                    }
                }
                else if(Random.Range(0, 100) <= 80) {
                    toCheck.Enqueue(dequeueTile);
                }
            }
        }
        List<Tile> phase2GrassTiles = new List<Tile>();

        foreach(Tile t in grassTiles) {
            for(int i = -1; i < 2; i++) {
                for(int ii = -1; ii < 2; ii++) {
                    if(i == 0 || ii == 0) {
                        if(Grid[t.gridPosX + i, t.gridPosY + ii].currentState == GroundState.Water) {
                            if(Random.Range(0, 100) <= 70) {
                                t.ChangeMaterial(GroundState.Sand, "Water Pushes The Small Rocks To Land Witch Formes Beaches");
                                t.myTile.layer = LayerMask.NameToLayer("ground");
                            }
                            break;
                        }
                    }
                }
            }
            if(Grid[t.gridPosX, t.gridPosY].currentState == GroundState.Grass) {
                phase2GrassTiles.Add(t);
            }
        }

        for(int i = 0; i < toSpawnList.Count; i++) {
            int localToSpawn = toSpawnList[i].amountToSpawn;

            while(localToSpawn > 0) {
                if(phase2GrassTiles.Count == 0) {
                    Debug.Log("Not enough space for trees");
                    return;
                }

                Tile toPlant = phase2GrassTiles[Random.Range(0, phase2GrassTiles.Count)];

                toSpawnList[i].spawnedObjects.Add(Instantiate(toSpawnList[i].objectToSpawn, toPlant.myTile.transform.position, Quaternion.Euler(new Vector3(toSpawnList[i].x == true ? Random.Range(0, 360) : toSpawnList[i].objectToSpawn.transform.localEulerAngles.x, toSpawnList[i].y == true ? Random.Range(0, 360) : toSpawnList[i].objectToSpawn.transform.localEulerAngles.y, toSpawnList[i].z == true ? Random.Range(0, 360) : toSpawnList[i].objectToSpawn.transform.localEulerAngles.z))));
                localToSpawn--;

                phase2GrassTiles.Remove(toPlant);
            }
        }

        grassTiles = phase2GrassTiles;
    }

    public void DestroyMap() {
        for(int i = 0; i < toSpawnList.Count; i++) {
            for(int ii = 0; ii < toSpawnList[i].spawnedObjects.Count; ii++) {
                if(toSpawnList[i].spawnedObjects[ii] != null) {
                    DestroyImmediate(toSpawnList[i].spawnedObjects[ii]);
                }
            }
            toSpawnList[i].spawnedObjects.Clear();
        }

        if(Grid != null) {
            foreach(Tile tile in Grid) {
                if(tile.myTile != null) {
                    DestroyImmediate(tile.myTile);
                }
            }
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

    public void DryGrounds(Vector3 dryPosition){

    }

    public void BurnGrounds(Vector3 burnPosition){

    }

    public void StartFire(Vector3 pos){

    }
}