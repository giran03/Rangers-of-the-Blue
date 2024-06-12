using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PickUp_Handler : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField] GameObject hud_icon_grab;
    [SerializeField] GameObject hud_icon_deposit;
    [SerializeField] GameObject hud_button_grab;
    [SerializeField] GameObject hud_button_deposit;
    [SerializeField] TMP_Text hud_label_trashPickUp;
    [SerializeField] TMP_Text hud_label_netBag;
    [SerializeField] GameObject hud_label_netBagFull;


    // object pooling
    [Header("Object Pooling Configs")]
    [SerializeField] GameObject prefab_trash; // Prefab to instantiate for the pool
    [SerializeField] int initialPoolSize = 8; // Target pool size
    [SerializeField] List<GameObject> pool;
    [SerializeField] Vector3 min_SpawnOffset;
    [SerializeField] Vector3 max_SpawnOffset;
    [SerializeField] Transform poolSpawnOrigin;
    GameObject spawned_trash;
    GameObject trashObject;

    [Header("Net Configs")]
    [SerializeField] int netBag_capacity = 10;
    int netBag_currentCapacity = 0;
    bool isBagFullText;

    // scores
    int score_trashPickUp = 0;

    Vector3 screenCenter = new(0.5f, 0.5f, 0f);

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Trash"))
            {
                trashObject = hit.collider.gameObject;
                hud_icon_grab.SetActive(true);
                hud_button_grab.SetActive(true);
            }
            if (hit.collider.CompareTag("Deposit"))
            {
                hud_icon_deposit.SetActive(true);
                hud_button_deposit.SetActive(true);
                Debug.Log("Deposit trash Here~!");
            }
        }
        else
        {
            hud_button_deposit.SetActive(false);
            hud_icon_deposit.SetActive(false);
            hud_icon_grab.SetActive(false);
            hud_button_grab.SetActive(false);
        }

        // sets score
        hud_label_trashPickUp.SetText($"Score: {score_trashPickUp}");
        hud_label_netBag.SetText($"Net-bag: {netBag_currentCapacity}");
    }

    public void PickUpButton()
    {
        if (trashObject != null)
        {
            if (netBag_currentCapacity != netBag_capacity)
            {
                netBag_currentCapacity++;

                Destroy(trashObject);
                OnObjectDestroyed(trashObject);
            }
            else
            {
                Debug.Log("You'r net bag is full!!!\nDeposit trash first");
                if (!isBagFullText)
                    StartCoroutine(netBagFull());
            }
        }
    }

    public void DepositButton()
    {
        score_trashPickUp += netBag_currentCapacity;
        netBag_currentCapacity = 0;
    }

    IEnumerator netBagFull()
    {
        isBagFullText = true;
        hud_label_netBagFull.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        isBagFullText = false;
        hud_label_netBagFull.SetActive(false);
    }

    public GameObject GetObject()
    {
        GameObject obj = FindInactiveObject();
        if (obj == null)
        {
            GrowPool();
            obj = GetObject(); // Call GetObject again to retrieve a newly created object
        }
        obj.SetActive(true);
        return obj;
    }

    private GameObject FindInactiveObject()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }
        return null;
    }

    private void GrowPool()
    {
        if (pool.Count < initialPoolSize)
        {
            float x;
            float y;
            float z;
            Vector3 pos;

            // spawning trash
            spawned_trash = Instantiate(prefab_trash, transform);
            spawned_trash.SetActive(true);
            pool.Add(spawned_trash);

            // random position
            x = Random.Range(min_SpawnOffset.x, max_SpawnOffset.x);
            y = -.5f;
            z = Random.Range(min_SpawnOffset.z, max_SpawnOffset.z);
            pos = new Vector3(x, y, z);
            spawned_trash.transform.position = pos;

            Debug.Log("Spawned another trash~!");
        }
    }

    // Function to handle object destruction (optional, call from your script)
    public void OnObjectDestroyed(GameObject obj)
    {
        pool.Remove(obj); // Remove destroyed object from the pool
        GrowPool(); // Ensure pool size is maintained
    }
}
