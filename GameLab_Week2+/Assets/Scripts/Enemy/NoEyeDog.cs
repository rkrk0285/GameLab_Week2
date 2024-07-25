using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEyeDog : Enemy
{
    public float rushSpeed;
    public float rushCooldown;

    private float rushTimer;
    private bool isRushing = false;

    enum State
    {
        Idle,
        Chase,        
        Attack
    }

    State state = State.Idle;    

    private void FixedUpdate()
    {        
        StateCheck();
        Flip();
        if (state == State.Idle)
        {
            return;
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

    void StateCheck()
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
        transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, moveSpeed);
    }

    void Attack()
    {        
        if (!isRushing && rushTimer <= 0f)
        {
            isRushing = true;
            rushTimer = rushCooldown;
            StartCoroutine("Rush");
        }
        else
        {
            rushTimer -= Time.fixedDeltaTime;
            Debug.Log(rushTimer);
            Chase();
        }
    }

    IEnumerator Rush()
    {
        yield return null;
        Vector3 target = Player.transform.position;
        while(true)
        {            
            yield return new WaitForFixedUpdate();
            transform.position = Vector2.MoveTowards(transform.position, target, rushSpeed);

            float distance = Vector3.Distance(transform.position, target);
            if (distance < 0.01f)
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
