using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private Transform boardContainer;
    [SerializeField] private List<PointOfInterest> pointsOfInterestPrefabs;
    [SerializeField] private GameObject pathPrefab;
    [SerializeField] private PointOfInterest startingPointPrefab;
    [SerializeField] private PointOfInterest finishingPointPrefab;
    private List<PointOfInterest> endPointOfInterests = new();
    [SerializeField] private int numberOfStartingPoints = 4;
    [SerializeField] private int mapLength = 10;
    [SerializeField] private int maxWidth = 5;
    [SerializeField] private float xMaxSize;
    [SerializeField] private float yPadding;
    [SerializeField] private bool allowCrisscrossing;
    [Range(0.1f, 1f), SerializeField] private float chancePathMiddle;
    [Range(0f, 1f), SerializeField] private float chancePathSide;
    [SerializeField, Range(0.9f, 5f)] private float multiplicativeSpaceBetweenLines = 2.5f;
    [SerializeField, Range(1f, 5.5f)] private float multiplicativeNumberOfMinimunConnections = 3f;

    private PointOfInterest[][] _pointOfInterestsPerFloor;
    private readonly List<PointOfInterest> pointsOfInterest = new();
    private int _numberOfConnections = 0;
    private float _lineLength;
    private float _lineHeight;

    [SerializeField] private EnvironmentData currentEnvironment;

    public void SetEnvironment(EnvironmentData environmentData)
    {
        currentEnvironment = environmentData;
    }

    public PointOfInterest GetFirstPointOfInterest()
    {
        if (pointsOfInterest.Count > 0)
        {
            return pointsOfInterest[0];
        }
        else
        {
            Debug.LogError("No points of interest generated");
            return null;
        }
    }


    public void RecreateBoard()
    {
        endPointOfInterests.Clear(); // Clear the end points list
        _lineLength = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.z *
                      pathPrefab.transform.localScale.z;
        _lineHeight = pathPrefab.GetComponent<MeshFilter>().sharedMesh.bounds.size.y *
                      pathPrefab.transform.localScale.y;
        DestroyImmediateAllChildren(boardContainer);
        _numberOfConnections = 0;
        GenerateRandomSeed();
        pointsOfInterest.Clear();
        _pointOfInterestsPerFloor = new PointOfInterest[mapLength][];
        for (int i = 0; i < _pointOfInterestsPerFloor.Length; i++)
        {
            _pointOfInterestsPerFloor[i] = new PointOfInterest[maxWidth];
        }

        CreateMap();
    }

    private void GenerateRandomSeed()
    {
        int tempSeed = (int) System.DateTime.Now.Ticks;
        Random.InitState(tempSeed);
    }

    private PointOfInterest InstantiatePointOfInterest(int floorN, int xNum, bool isStartingPoint = false)
    {
        if (_pointOfInterestsPerFloor[floorN][xNum] != null)
        {
            return _pointOfInterestsPerFloor[floorN][xNum];
        }

        float xSize = xMaxSize / maxWidth;
        float xPos = (xSize * xNum) + (xSize / 2f);
        float yPos = yPadding * floorN;

        //Add a random padding
        xPos += Random.Range(-xSize / 4f, xSize / 4f);
        yPos += Random.Range(-yPadding / 4f, yPadding / 4f);

        Vector3 pos = new Vector3(xPos, 0, yPos);
        PointOfInterest randomPOI = isStartingPoint
            ? startingPointPrefab.GetComponent<PointOfInterest>()
            : pointsOfInterestPrefabs[Random.Range(0, pointsOfInterestPrefabs.Count)];
        PointOfInterest instance = Instantiate(randomPOI, boardContainer);
        instance.sceneData = currentEnvironment.sceneDataList[Random.Range(0, currentEnvironment.sceneDataList.Count)];
        pointsOfInterest.Add(instance);

        instance.transform.localPosition = pos;
        _pointOfInterestsPerFloor[floorN][xNum] = instance;
        int created = 0;

        void InstantiateNextPoint(int index_i, int index_j)
        {
            PointOfInterest nextPOI = InstantiatePointOfInterest(index_i, index_j);
            AddLineBetweenPoints(instance, nextPOI);
            instance.NextPointsOfInterestWithPath.Add(nextPOI);
            created++;
            _numberOfConnections++;
        }

        while (created == 0 && floorN < mapLength - 1)
        {
            if (xNum > 0 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsPerFloor[floorN + 1][xNum] == null)
                {
                    InstantiateNextPoint(floorN + 1, xNum - 1);
                }
            }

            if (xNum < maxWidth - 1 && Random.Range(0f, 1f) < chancePathSide)
            {
                if (allowCrisscrossing || _pointOfInterestsPerFloor[floorN + 1][xNum] == null)
                {
                    InstantiateNextPoint(floorN + 1, xNum + 1);
                }
            }

            if (Random.Range(0f, 1f) < chancePathMiddle)
            {
                InstantiateNextPoint(floorN + 1, xNum);
            }
        }

        if (floorN == mapLength - 1 && instance.NextPointsOfInterestWithPath.Count == 0)
        {
            endPointOfInterests.Add(instance);
        }

        return instance;
    }


    private void CreateMap()
    {
        List<int> positions = GetRandomIndexes(numberOfStartingPoints);
        foreach (int j in positions)
        {
            PointOfInterest startPoint = InstantiatePointOfInterest(0, j, true);
            startPoint.sceneData = currentEnvironment.startingSceneData;
        }

        positions = GetRandomIndexes(numberOfStartingPoints);
        foreach (int j in positions)
        {
            PointOfInterest endPoint = InstantiatePointOfInterest(mapLength - 1, j);
            endPoint.sceneData = currentEnvironment.finishingSceneData;
        }
        
        if (_numberOfConnections <= mapLength * multiplicativeNumberOfMinimunConnections)
        {
            Debug.Log($"Recreating board with {_numberOfConnections} connections");
            RecreateBoard();
            return;
        }

        foreach (PointOfInterest endPoint in endPointOfInterests)
        {
            // Add a continuation point for this end point and make it a finish point
            PointOfInterest finishPoint = Instantiate(finishingPointPrefab,
                endPoint.transform.position + new Vector3(0, 0, _lineLength), Quaternion.identity, boardContainer);
            endPoint.NextPointsOfInterestWithPath.Add(finishPoint);
            AddLineBetweenPoints(endPoint, finishPoint);

            // Assign the finishingSceneData to the sceneData field of the finishing point
            finishPoint.sceneData = currentEnvironment.finishingSceneData;

            // Add the finishing point to the points of interest
            pointsOfInterest.Add(finishPoint);
        }

        Debug.Log($"Created board with {_numberOfConnections} connections");
        Debug.Log($"Created board with {pointsOfInterest.Count} points");
    }

    private void AddLineBetweenPoints(PointOfInterest thisPoint, PointOfInterest nextPoint)
    {
        float len = _lineLength;
        float height = _lineHeight;

        //Get direction from point A to B
        Vector3 dir = (nextPoint.transform.position - thisPoint.transform.position).normalized;

        //Get distance from point A to B
        float dist = Vector3.Distance(thisPoint.transform.position, nextPoint.transform.position);

        //Number of lines (with padding) that fits inside the space from point A to B
        int num = (int) (dist / (len * multiplicativeSpaceBetweenLines));

        //Find the real padding distance, since num is rounded to integer, the padding may increase
        float pad = (dist - (num * len)) / (num + 1);

        //Position of first line. the len/2f is the center of the line
        Vector3 pos_i = thisPoint.transform.position + (dir * (pad + (len / 2f)));

        //Position all the lines
        for (int i = 0; i < num; i++)
        {
            Vector3 pos = pos_i + ((len + pad) * i * dir);
            GameObject lineCreated = Instantiate(pathPrefab, pos, Quaternion.identity, boardContainer);
            lineCreated.transform.LookAt(nextPoint.transform);
            lineCreated.transform.position -= Vector3.up * (height / 2f);
        }
    }

    private List<int> GetRandomIndexes(int n)
    {
        List<int> indexes = new List<int>();
        if (n > maxWidth)
        {
            throw new System.Exception("Number of starting points greater than maxWidth!");
        }

        while (indexes.Count < n)
        {
            int randomNum = Random.Range(0, maxWidth);
            if (!indexes.Contains(randomNum))
            {
                indexes.Add(randomNum);
            }
        }

        return indexes;
    }


    private void DestroyImmediateAllChildren(Transform transform)
    {
        List<Transform> toKill = new();

        foreach (Transform child in transform)
        {
            toKill.Add(child);
        }

        for (int i = toKill.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(toKill[i].gameObject);
        }
    }
}