using UnityEngine;

public class TerrainTreePlacer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public bool enabledPlacement;

    [Header("Trees")]
    [SerializeField] private Tree[] trees;

    [Header("Brush Settings")]
    public float treeDensity;
    public float radius;
    public int selectedTree;

    private Transform parent;
    private Terrain[] terrains;
    private Terrain terrain;

    [Header("IGNORE")]
    public bool shiftPressed;

    public void UseBrush(Vector3 mouseWorldPosition)
    {
        if(shiftPressed)
        {
            RemoveTree(radius, mouseWorldPosition);
        }
        else
        {
            for(int i = 0; i < treeDensity; i++)
            {
                Vector3 randomPosition = new Vector3(mouseWorldPosition.x + Random.Range(-radius, radius), mouseWorldPosition.y, mouseWorldPosition.z + Random.Range(-radius, radius));
                PlaceTree(randomPosition);
            }  
        }
    }

    void RemoveTree(float radius, Vector3 mouseWorldPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(mouseWorldPosition, radius);
        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("Tree"))
            {
                DestroyImmediate(collider.gameObject);
            }
        }
    }

    void PlaceTree(Vector3 mouseWorldPostion)
    {
        float widthScale = 0f;
        float heightScale = 0f;
        float rotation = 0f;

        if(parent == null)
        {
            parent = new GameObject("Trees").transform;
        }

        terrains = Terrain.activeTerrains;

        for(int i = 0; i < terrains.Length; i++)
        {
            if(mouseWorldPostion.x > terrains[i].transform.position.x || mouseWorldPostion.x < terrains[i].transform.position.x + terrains[i].terrainData.size.x || mouseWorldPostion.z > terrains[i].transform.position.z || mouseWorldPostion.z < terrains[i].transform.position.z + terrains[i].terrainData.size.z)
            {
                terrain = terrains[i];
                break;
            }
        }
        

        if (terrain != null)
        {
            Vector3 position = mouseWorldPostion;
            position.y = terrain.SampleHeight(position);
            Tree tree = trees[selectedTree];
            if (tree.randomPosition)
            {
                position.x += Random.Range(-tree.positionRange.x, tree.positionRange.x);
                position.z += Random.Range(-tree.positionRange.y, tree.positionRange.y);
            }
            if (tree.randomRotation)
            {
                rotation = Random.Range(tree.rotationRange.x, tree.rotationRange.y);
            }
            if (tree.randomWidth)
            {
                widthScale = Random.Range(tree.widthRange.x, tree.widthRange.y);
            }
            if(tree.randomHeight)
            {
                heightScale = Random.Range(tree.heightRange.x, tree.heightRange.y);
            }
            GameObject treeObject = Instantiate(tree.prefab, position, Quaternion.Euler(0, rotation, 0) , parent);
            treeObject.tag = "Tree";
            treeObject.transform.localScale = new Vector3(widthScale, heightScale, widthScale);
        }
                
    }
}

[System.Serializable]
public struct Tree
{
    public GameObject prefab;
    public bool randomRotation;
    public Vector2 rotationRange;
    public bool randomPosition;
    public Vector2 positionRange;
    public bool randomHeight;
    public Vector2 heightRange;
    public bool randomWidth;
    public Vector2 widthRange;
}
