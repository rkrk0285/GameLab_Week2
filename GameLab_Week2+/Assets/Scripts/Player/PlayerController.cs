using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int HP = 100;
    public float moveSpeed = 5f;

    private float DamageDelay = 1f;    
    private Rigidbody2D rb;
    private Vector2 movement;
    private float DamageTimer = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // �̵� �Է� �ޱ�
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (DamageTimer > 0)
        {
            DamageTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        // ���� ����� FixedUpdate���� ó��
        MovePlayer();
    }

    void MovePlayer()
    {
        if (movement.x == 0 && movement.y == 0)
            rb.velocity = Vector2.zero;
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void TakeDamage(int damage, GameObject obj)
    {
        if (DamageTimer <= 0)
        {
            DamageTimer = DamageDelay;
            HP -= damage;
        }

        if (HP <= 0)
        {
            Dead(obj);
        }
    }

    void Dead(GameObject obj)
    {
        // ���� ������Ʈ�� ���� ������ ���� ������ �ٸ�.

    }
}
