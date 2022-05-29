using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilitaryPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string Ability;
        public OffensiveStatsClass WeaponStats;
        public GameObject prefab;
        public int size;
    }

    public static MilitaryPool instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.parent = gameObject.transform;
                obj.GetComponent<Bullet>().Dmg = pool.WeaponStats.Damage;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Ability, objectPool);
        }

    }

    public GameObject SpawnFromPool(string Ability, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(Ability))
        {
            print("pool doestn exist");
            return null;
        }

        GameObject objToSpawn = poolDictionary[Ability].Dequeue();

        objToSpawn.SetActive(true);
        objToSpawn.transform.position = position;
        objToSpawn.transform.rotation = rotation;

        IPooledObject pooledObj = objToSpawn.GetComponent<IPooledObject>();
        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[Ability].Enqueue(objToSpawn);

        return objToSpawn;
    }
}
