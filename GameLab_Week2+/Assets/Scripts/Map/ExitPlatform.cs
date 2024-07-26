using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPlatform : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    // 충돌 중인 객체를 저장할 리스트
    private List<GameObject> collidingTreasures = new List<GameObject>();

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 중인 객체를 리스트에 추가
        if (other.CompareTag("Item") || other.CompareTag("Core"))
        {
            if (!collidingTreasures.Contains(other.gameObject))
            {
                collidingTreasures.Add(other.gameObject);
            }                        
            gameManager.CheckExitPlatformWeight(GetWeightOfCollidingObjects());
        }
    }

    // 트리거에서 벗어났을 때 리스트에서 제거
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Item") || other.CompareTag("Core"))
        {
            if (collidingTreasures.Contains(other.gameObject))
            {
                collidingTreasures.Remove(other.gameObject);
            }
            gameManager.CheckExitPlatformWeight(GetWeightOfCollidingObjects());
        }
    }

    // 충돌 중인 객체들을 배열로 반환하는 메서드
    public GameObject[] GetCollidingObjects()
    {
        return collidingTreasures.ToArray();
    }

    public int GetWeightOfCollidingObjects()
    {
        int result = 0;
        foreach (GameObject obj in collidingTreasures)
        {
            int weight = obj.GetComponent<Item>().weight;
            result += weight;
        }        
        return result;
    }
}
