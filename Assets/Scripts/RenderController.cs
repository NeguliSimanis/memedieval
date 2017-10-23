using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderController : MonoBehaviour
{

    SpriteRenderer spriteRenderer;

    void Start()
    {
        //Debug.Log("EnvironmentObj" + spriteRenderer.sortingOrder);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = (int)Camera.main.WorldToScreenPoint(spriteRenderer.bounds.min).y * -1;
    }
}
