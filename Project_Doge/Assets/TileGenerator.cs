using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileGenerator : MonoBehaviour
{
    [LabelText("기본 타일")]
    [SerializeField] private GameObject hexPrefab;
    [LabelText("물 타일")]
    [SerializeField] private GameObject hexPrefabWater;
    [LabelText("맵 크기")]
    [SerializeField] private int mapSize = 3; // 맵 크기
    [LabelText("타일 생성위치")]
    [SerializeField] private Transform tileSpawnPoint;
    
    private float hexWidth = 1.732f;
    private float hexHeight = 1.5f;
    private float spacingFactor = 1.15f;

    private Dictionary<Vector3, GameObject> hexTiles = new Dictionary<Vector3, GameObject>();
    
    [LabelText("물로 변경 변수")]
    [SerializeField] private bool isPlacingWater = false;

    [Button, LabelText("맵 생성")]
    public void GenerateTileMap()
    {
        GenerateHexMap(Mathf.Max(mapSize, 1));
    }

    void Update()
    {
        if (isPlacingWater && Mouse.current.leftButton.isPressed)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 pos = hit.collider.transform.position;
                
                if (hexTiles.ContainsKey(pos) && hexTiles[pos].CompareTag("HexTile"))
                {
                    Destroy(hexTiles[pos]);
                    GameObject newTile = Instantiate(hexPrefabWater, pos, Quaternion.identity, tileSpawnPoint);
                    hexTiles[pos] = newTile;
                }
            }
        }
    }

    void GenerateHexMap(int size)
    {
        hexTiles.Clear();
        
        foreach (Transform child in tileSpawnPoint)
        {
            Destroy(child.gameObject);
        }
        
        HashSet<Vector2Int> hexCoords = new HashSet<Vector2Int>(); // 중복 방지

        for (int q = -size; q <= size; q++)
        {
            for (int r = -size; r <= size; r++)
            {
                if (Mathf.Abs(q + r) > size) continue; // 육각형 범위 제한
                hexCoords.Add(new Vector2Int(q, r));
            }
        }

        foreach (Vector2Int coord in hexCoords)
        {
            Vector3 worldPos = HexToWorldPosition(coord.x, coord.y);
            GameObject tile = Instantiate(hexPrefab, tileSpawnPoint.position + worldPos, Quaternion.identity, tileSpawnPoint);
            tile.tag = "HexTile"; // 태그 설정
            hexTiles.Add(tileSpawnPoint.position + worldPos, tile);
        }
    }

    Vector3 HexToWorldPosition(int q, int r)
    {
        float x = hexWidth * spacingFactor * (q + r * 0.5f);
        float z = hexHeight * spacingFactor * r;
        return new Vector3(x, 0, z);
    }
}
