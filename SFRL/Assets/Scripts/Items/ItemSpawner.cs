using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct Spawnable
    {
        public GameObject gameobject;
        public float weight;
    }
    public List<Spawnable> items = new List<Spawnable>();
    float _totalWeight;

    void Awake()
    {
        _totalWeight = 0;
        foreach(var spawnable in items)
        {
            _totalWeight += spawnable.weight;
        }
    }

    void Start()
    {
        float pick = Random.value * _totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;

        while (pick > cumulativeWeight && chosenIndex < items.Count - 1)
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }

        GameObject i = Instantiate(items[chosenIndex].gameobject, transform.position, transform.rotation) as GameObject;
        Destroy(gameObject);
    }
}
