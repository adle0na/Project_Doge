using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    [SerializeField] private GameObject hexPrefab; // 헥사곤 타일 프리팹
    [SerializeField] private int mapSize = 3; // 맵 크기 (사용자가 입력)

    [SerializeField] private float hexWidth = 1.732f; // sqrt(3) ≈ 1.732 (헥사 타일의 가로 길이)
    [SerializeField] private float hexHeight = 1.5f; // 헥사 타일의 세로 간격 (1.5배)

    public void CreateTile()
    {
        GenerateHexMap(mapSize);
    }

    void GenerateHexMap(int size)
    {
        HashSet<Vector2Int> hexCoords = new HashSet<Vector2Int>(); // 중복 방지용

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
            Instantiate(hexPrefab, worldPos, Quaternion.identity);
        }
    }

    Vector3 HexToWorldPosition(int q, int r)
    {
        float x = hexWidth * (q + r * 0.5f); // x 좌표
        float z = hexHeight * r; // z 좌표
        return new Vector3(x, 0, z); // y는 0 (2D 기준)
    }
}
