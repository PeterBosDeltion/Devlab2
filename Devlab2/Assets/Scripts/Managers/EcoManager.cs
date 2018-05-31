using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcoManager : MonoBehaviour {
    public static EcoManager instance;

    [System.Serializable]
    public enum GroundState {
        Grass = 1,
        Sand = 2,
        Water = 3,
        burned = 4,
        fertile = 5,
        driedGround = 6
    }

    Tile[,] Grid;
    GameObject[] treesInScene;

    static Material[] groundTextures;
    public List<Ground> groundTexturesInput = new List<Ground>();

    void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else {
            Destroy(gameObject);
        }
    }

    #region Tile Genorator
    public int edgeOffset;
    public int islandSize;
    public int amountOfTrees;
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
                Grid[x, y] = new Tile(newTile, GroundState.Water, x, y);
            }
            pos = !pos;
        }

        if(islandSize > xSize * ySize - (edgeOffset * 4)) {

            Debug.Log("The Size Of The Island Is To Big For The Map Size");
            return;
        }

        Tile startTile = Grid[xSize / 2, ySize / 2];
        startTile.ChangeMaterial(GroundState.Grass);

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
                    dequeueTile.ChangeMaterial(GroundState.Grass);
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
                                t.ChangeMaterial(GroundState.Sand);
                                t.myTile.layer = LayerMask.NameToLayer("NonGrass");
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
        public List<GroundState> myTimeLine = new List<GroundState>();
        public GroundState currentState;
        public int gridPosX, gridPosY;
        public GameObject myTile;
        Renderer myRenderer;

        public Tile(GameObject newTile, GroundState newState, int gridX, int gridY) {
            myTile = newTile;
            myRenderer = myTile.GetComponent<Renderer>();
            ChangeMaterial(newState);
            gridPosX = gridX;
            gridPosY = gridY;
        }

        public void ChangeMaterial(GroundState newState) {
            myRenderer.material = groundTextures[(int)newState - 1];
            myTimeLine.Add(newState);
            currentState = newState;
        }

    }

    [System.Serializable]
    public class Ground {
        public GroundState state;
        public Material tex;
    }
}