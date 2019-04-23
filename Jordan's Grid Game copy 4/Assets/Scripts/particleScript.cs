using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyParticle", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DestroyParticle ()
    {
        Destroy(gameObject);
    }
}
