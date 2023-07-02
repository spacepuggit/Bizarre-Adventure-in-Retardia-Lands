using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController
{
    [SerializeField] private MapGenerator mapGenerator;

    public void GenerateMap()
    {
        mapGenerator.RecreateBoard();
    }

    public PointOfInterest GetFirstPointOfInterest()
    {
        return mapGenerator.GetFirstPointOfInterest();
    }
}

