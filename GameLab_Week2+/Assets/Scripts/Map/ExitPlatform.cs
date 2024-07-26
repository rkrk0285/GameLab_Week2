using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitPlatform : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    // �浹 ���� ��ü�� ������ ����Ʈ
    private List<GameObject> collidingTreasures = new List<GameObject>();

    private void Start()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // �浹 ���� ��ü�� ����Ʈ�� �߰�
        if (other.CompareTag("Item") || other.CompareTag("Core"))
        {
            if (!collidingTreasures.Contains(other.gameObject))
            {
                collidingTreasures.Add(other.gameObject);
            }                        
            gameManager.CheckExitPlatformWeight(GetWeightOfCollidingObjects());
        }
    }

    // Ʈ���ſ��� ����� �� ����Ʈ���� ����
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

    // �浹 ���� ��ü���� �迭�� ��ȯ�ϴ� �޼���
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
