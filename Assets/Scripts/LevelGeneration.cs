using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    
    Vector2 worldSize = new Vector2(4, 4);
    Room[,] rooms;
    List<Vector2> takenPositions = new List<Vector2>();
    int gridSizeX, gridSizeY;
    int numberOfRooms = 16;
    public GameObject roomWhiteObj;

    private void Start() {
        if(numberOfRooms >= (worldSize.x  * worldSize.y)) {
            numberOfRooms = Mathf.RoundToInt(worldSize.x * worldSize.y);
        }

        gridSizeX = Mathf.RoundToInt(worldSize.x); 
        gridSizeY = Mathf.RoundToInt(worldSize.y);

        CreateRooms();
    }

    void CreateRooms(){

    }
}
