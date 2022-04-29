using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public Vector2 gridPos;
    public int type;
    public bool[] doors;

    public Room(Vector2 _gridPos, int _type){
        this.gridPos = _gridPos;
        this.type = _type;
    }
}
