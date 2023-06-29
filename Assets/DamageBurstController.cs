using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBurstController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cleanup());
    }

    private IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
