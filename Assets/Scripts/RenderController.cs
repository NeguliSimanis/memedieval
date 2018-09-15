using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    UnitSortingLayers unitSortingLayers;
    private string battleControllerObjectTag = "BattleController";

    void Start()
    {
        GetComponents();
        SetRenderLayer();
    }

    void GetComponents()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        unitSortingLayers = GameObject.FindGameObjectWithTag(battleControllerObjectTag).GetComponent<UnitSortingLayers>();
    }

    void SetRenderLayer()
    {
        //spriteRenderer.sortingOrder = unitSortingLayers.GetLayer();
    }
}
