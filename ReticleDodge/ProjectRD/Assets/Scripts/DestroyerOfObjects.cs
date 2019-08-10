using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerOfObjects : MonoBehaviour
{
    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
