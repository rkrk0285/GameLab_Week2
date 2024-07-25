using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Rendering;

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

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }
}
