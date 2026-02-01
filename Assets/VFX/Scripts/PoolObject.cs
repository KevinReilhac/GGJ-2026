using UnityEngine;

public class PoolObject : MonoBehaviour
{
    Pool pool;
    public void ReturnToPool()
    {
        transform.SetParent(pool.transform);
        gameObject.SetActive(false);
    }
    
    public void SetPool(Pool newPool)
    {
        pool = newPool;
    }
}
