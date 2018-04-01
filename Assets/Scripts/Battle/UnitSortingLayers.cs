using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSortingLayers : MonoBehaviour {

    static int topLayer = 3000;

	void Start()
    {   
        ResetLayer();
	}

    void ResetLayer()
    {
        topLayer = 3000;
    }
	
	public int GetLayer()
    {
        topLayer--;

        if (topLayer <= 0)
            ResetLayer();

        return topLayer;
    }
}
