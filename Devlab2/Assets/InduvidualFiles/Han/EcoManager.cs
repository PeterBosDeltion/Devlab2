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
    }

    public int xSize, ySize;
    public float xStepSize, yStepSize;
    public GameObject tile;
    Tile[,] Grid;

    Material[] groundTextures;

    public List<Ground> groundTexturesInput = new List<Ground>();

    void Awake() {
        instance = this;
    }

    void Start() {
        GenerateMap();
    }

    public void GenerateMap() {
        groundTextures = new Material[groundTexturesInput.Count];
        for(int i = 0; i < groundTexturesInput.Count; i++) {
            groundTextures[(int)groundTexturesInput[i].state - 1] = groundTexturesInput[i].tex;
        }

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
                Grid[x, y] = new Tile(newTile, (GroundState)Random.Range(0,5)); //          ***Not Random
            }
            pos = !pos;
        }
    }

    [System.Serializable]
    public class Tile {
        public List<GroundState> myTimeLine = new List<GroundState>();
        GameObject myTile;
        Renderer myRenderer;

        public Tile(GameObject newTile,GroundState newState) {
            myTile = newTile;
            myRenderer = myTile.GetComponent<Renderer>();
            ChangeMaterial(newState);
        }

        public void ChangeMaterial(GroundState newState){
            myRenderer.material = instance.groundTextures[(int)newState];
            myTimeLine.Add(newState);
        }

    }

    [System.Serializable]
    public class Ground {
        public GroundState state;
        public Material tex;
    }
}

