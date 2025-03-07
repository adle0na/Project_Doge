using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class TileGenerator : MonoBehaviour
{
    [LabelText("일반 바닥")]
    [SerializeField] private GameObject hexPrefabNormal;
    [LabelText("이동 불가 바닥")]
    [SerializeField] private GameObject hexPrefabWater;
    [LabelText("맵크기")]
    [SerializeField] private int mapSize;
    
    private float hexWidth = 1.732f;
    private float hexHeight = 1.5f;
    private float spacingFactor = 1.15f;

    void Start()
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
            Instantiate(hexPrefabNormal, worldPos, Quaternion.identity);
        }
    }

    Vector3 HexToWorldPosition(int q, int r)
    {
        float x = hexWidth * spacingFactor * (q + r * 0.5f); // x 좌표 (간격 증가)
        float z = hexHeight * spacingFactor * r; // z 좌표 (간격 증가)
        return new Vector3(x, 0, z); // y는 0 (2D 기준)
    }
}