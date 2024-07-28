using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float attackPower;
    public float attackDelay;
    public float attackDistance;
    public float attackRange;

    public float detectDistance;
    public float detectAngle;

    public LayerMask WallLayer;

    protected GameObject Player;
    public Tilemap WallMap;
    public Tile WallTile;
    protected bool CheckWallBetweenPlayer(float distance)
    {        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Player.transform.position - transform.position, distance, WallLayer);

        if (hit.collider != null)                    
            return true;        
        else
            return false;
    }
}
