using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("캔버스")]
    public GameObject JumpScareCanvas;
    public GameObject HitCanvas;
    
    [Header("파라미터")]
    private int HP = 100;
    public float moveSpeed = 10f;

    private float DamageDelay = 1f;
    private float DamageTimer = 0f;
    private float moveDefault = 10f;
    private float moveDeclineOffset = 30f;
    private Rigidbody2D rb;
    private Vector2 movement;    

    private bool isDead;    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
    }
    void Update()
    {
        // 이동 입력 받기
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        
        if (DamageTimer > 0)
        {
            DamageTimer -= Time.deltaTime;
        }
    }
    void FixedUpdate()
    {
        if (!isDead)
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
        if (isDead)
            return;
        
        if (DamageTimer <= 0)
        {
            DamageTimer = DamageDelay;
            HP -= damage;
            StartCoroutine(RedScreen());
        }

        if (HP <= 0)                    
            Dead(obj);        
    }
    void Dead(GameObject obj)
    {
        // 죽인 오브젝트에 따라 나오는 점프 스퀘어 다름.
        isDead = true;
        Debug.Log(obj.tag);
        if (obj.CompareTag("Leviathan"))
        {
            JumpScareCanvas.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (obj.CompareTag("NoEyeDog"))
        {
            JumpScareCanvas.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
    public void CalculateMoveSpeed(int weight)
    {
        moveSpeed = moveDefault - (weight / moveDeclineOffset);
    }
    IEnumerator RedScreen()
    {        
        float timer = 0;
        HitCanvas.SetActive(true);
        while(true)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.fixedDeltaTime;

            if (timer >= 0.15f)            
                break;            
        }
        HitCanvas.SetActive(false);
        StopCoroutine(RedScreen());
    }
}
