using System;
using UnityEngine;

public class ObjectItemPickup : MonoBehaviour
{
    private SpriteRenderer sr;

    [SerializeField] private ItemDataSO itemData;

    private void OnValidate()
    {
        if (itemData == null)
        {
            return;
        }

        sr = GetComponent<SpriteRenderer>();
        sr.sprite = itemData.itemIcon;
        gameObject.name = "Object ItemPickup - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player pick up item: " + itemData.itemName);
        Destroy(gameObject);
    }
}
