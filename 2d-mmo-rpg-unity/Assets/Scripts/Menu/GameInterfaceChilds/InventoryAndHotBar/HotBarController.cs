using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotBarController : MonoBehaviour
{
    public GameObject hotBarUI;
    public List<GameObject> hotBarSlots;

    void Start()
    {
        foreach (GameObject hotBarSlot in hotBarSlots)
        {
            SetCanvasAlpha(hotBarSlot);
        }

        SetIdsForSlots();
    }

    private void SetCanvasAlpha(GameObject hotBarSlot)
    {
        hotBarSlot.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void SetIdsForSlots()
    {
        for (int i = 0; i < hotBarSlots.Count; i++)
        {
            hotBarSlots[i].GetComponent<HotBarSlotController>().id = i+1;
        }
    }

    void Update()
    {
        
    }

}