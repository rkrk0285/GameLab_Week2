using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leviathan : Enemy
{
    private const int Damage = 110;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");        
    }        

    private void FixedUpdate()
    {
        Chase();
    }

    void Chase()
    {
        this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float distance = Vector3.Distance(transform.position, Player.transform.position);        
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(Damage, gameObject);
    }
}
