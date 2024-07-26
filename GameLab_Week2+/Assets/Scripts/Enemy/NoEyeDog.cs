using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class NoEyeDog : Enemy
{
    public float rushSpeed;
    public float rushCooldown;
    
    private float rushTimer;
    private bool isRushing = false;
    
    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        isRushing = false;        
    }
    enum State
    {
        Idle,
        Chase,        
        Attack
    }
    
    State state = State.Idle;    

    private void FixedUpdate()
    {        
        ChangeState();
        Flip();
        if (state == State.Idle)
        {
            Idle();
        }
        else if (state == State.Chase)
        {
            Chase();
        }        
        else if (state == State.Attack)
        {            
            Attack();
        }
    }

    void Idle()
    {
        this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

    }
    void ChangeState()
    {        
        float distance = Vector3.Distance(transform.position, Player.transform.position);
        if (distance < attackDistance)
        {
            state = State.Attack;
        }
        else if (distance < detectDistance)
        {            
            state = State.Chase;
        }
        else
        {
            state = State.Idle;
        }
    }
    

    void Chase()
    {
        this.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (CheckWallBetweenPlayer(distance))
        {
            Vector3 target = transform.position * 2 - Player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed);
        }
        else
            transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveSpeed);
    }

    void Attack()
    {
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        if (CheckWallBetweenPlayer(distance))
        {
            Vector3 target = transform.position * 2 - Player.transform.position;
            transform.position = Vector2.MoveTowards(transform.position, target, moveSpeed);
            return;
        }

        if (!isRushing && rushTimer <= 0f)
        {
            isRushing = true;
            rushTimer = rushCooldown;
            StartCoroutine("Rush");
        }
        else
        {
            rushTimer -= Time.fixedDeltaTime;            
            Chase();
        }
    }

    IEnumerator Rush()
    {
        yield return null;
        Vector3 target = Player.transform.position;
        float timer = 0f;
        while(true)
        {            
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;

            transform.position = Vector2.MoveTowards(transform.position, target, rushSpeed);
            float distance = Vector3.Distance(transform.position, target);
            if (distance < 0.01f || timer > 5f)
                break;
        }
        isRushing = false;
        StopCoroutine("Rush");
    }

    void Flip()
    {
        float offset = transform.position.x - Player.transform.position.x;
        if ((offset < 0f && transform.localScale.x > 0f) || (offset > 0f && transform.localScale.x < 0f))
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
    }    
}
