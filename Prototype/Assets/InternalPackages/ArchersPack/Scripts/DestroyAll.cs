using UnityEngine;
using System.Collections;

public class DestroyAll : MonoBehaviour
{

    public float gDelayTime = 3f;
    
    void Start()
    {
        Invoke("rDestroy", gDelayTime);
    }

    
    void Update()
    {

    }

    void rDestroy()
    {
        Destroy(gameObject);
    }
}
