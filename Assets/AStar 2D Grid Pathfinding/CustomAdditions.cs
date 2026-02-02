using System.Collections.Generic;
using AStar;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CustomAdditions : MonoBehaviour
{
    public int mapX;
    public int mapY;
    public Tilemap unwalkableTerrain;
    public bool[,] walkableMap;

    public static bool walkableMapWasGenerated = false;

    public static CustomAdditions Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindFirstObjectByType<CustomAdditions>();
            }
            return instance;
        }
    }
    private static CustomAdditions instance;

    public static List<Vector3> GetWorldCoordinatePath(Vector3 start, Vector3 end)
    {
        
        List<Vector3> path = new List<Vector3>();
        if (!walkableMapWasGenerated)
        {
            return path;
        }
        (int, int) startIndices = GetIndexFromWorldPoint(start);
        (int, int) endIndices = GetIndexFromWorldPoint(end);

        (int, int)[] indexPath = AStarPathfinding.GeneratePathSync(startIndices.Item1, startIndices.Item2, 
            endIndices.Item1, endIndices.Item2, Instance.walkableMap, true, true);

        foreach((int,int) coordinate in indexPath)
        {
            path.Add(GetWorldFromIndexPoint(coordinate));
            Debug.Log("[" +coordinate.Item1 +", "+ coordinate.Item2 + "]");
        }
        return path;
    }

    public static Vector3 GetWorldFromIndexPoint((int, int) coordinate)
    {
        return Instance.unwalkableTerrain.GetCellCenterWorld(new Vector3Int(coordinate.Item1, coordinate.Item2, 0));

    }
    public static (int, int) GetIndexFromWorldPoint(Vector3 coordinate)
    {
        Vector3Int vector = Instance.unwalkableTerrain.WorldToCell(coordinate);
        return (vector.x, vector.y);

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        walkableMap = GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool[,] GenerateGrid()
    {
        // 1. Compress bounds to only include cells with tiles
        unwalkableTerrain.CompressBounds();
        BoundsInt bounds = unwalkableTerrain.cellBounds;
        Debug.Log("bounds: " + bounds.size.x + " " + bounds.size.y);
        bool[,] boolGrid = new bool[bounds.size.x, bounds.size.y];

        // 2. Iterate through all cells in the bounds
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                // Calculate actual tilemap position
                Vector3Int localPlace = new Vector3Int(x + bounds.x, y + bounds.y, 0);

                // 3. Check if tile exists (true if it has a collider)
                boolGrid[x, y] = !unwalkableTerrain.HasTile(localPlace);
            }
        }
        
        /*string grid = "";
        int i = 0;
        foreach (bool b in boolGrid)
        {
            grid += b ? "1" : "0";
            i++;
            if(i > 237)
            {
                i = 0;
                grid += '\n';
            }
        }
        Debug.Log(grid);*/
        walkableMapWasGenerated = true;
        return boolGrid;
    }
}
