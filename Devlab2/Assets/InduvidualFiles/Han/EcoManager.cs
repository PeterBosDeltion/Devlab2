using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EcoManager : MonoBehaviour {
    public static EcoManager instance;

    public enum GroundState {
        Grass = 1,
        Sand = 2,
        Water = 3,
        burned = 4,
        fertile = 5,
    }

    public int xSize, ySize;
    public float xStepSize, yStepSize;
    public GameManager tile;
    Tile[,] Grid;


    public void GenerateMap() {
        Grid = new Tile[xSize, ySize];

        for(int x = 0; x < xSize; x++) {
            for(int y = 0; y < ySize; y++) {

                // spot = new Vector3(Mathf.);
                //GameObject newTile = Instantiate(tile,new Vector3(x * xStepSize,))
                //Grid[x, y] = new Tile(newTile);
            }
        }
    }

    [System.Serializable]
    public class Tile {
        public List<GroundState> myTimeLine = new List<GroundState>();
        GameObject myTile;

        public Tile(GameObject newTile) {
            myTile = newTile;
        }

    }
}

