using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SI_ObjectPooling : MonoBehaviour
{
    
    GameObject spawned_trash;

    public static SI_ObjectPooling Instance;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    
}
