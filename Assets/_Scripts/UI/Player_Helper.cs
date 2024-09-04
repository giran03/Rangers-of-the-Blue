using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Helper : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Transform defaultSpawn;

    public void Button_Resetplayerition()
    {
        player.transform.SetPositionAndRotation(defaultSpawn.position, Quaternion.identity);
        Debug.Log($"Player position reset!");
    }
}
