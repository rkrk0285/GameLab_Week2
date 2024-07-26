using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class Room
{    
    public Room(int roomNum, int minX, int maxX, int minY, int maxY)
    {
        this.roomNum = roomNum;
        UpRight = new Vector2Int(maxX, maxY);
        DownLeft = new Vector2Int(minX, minY);
    }
    public int roomNum;
    public Vector2Int UpRight;
    public Vector2Int DownLeft;
}

public class MapGenerator : MonoBehaviour
{
    [Header("맵")]
    [SerializeField]
    private Tilemap WallMap;
    [SerializeField]
    private GameObject shadowCasterPrefab;

    [Header("타일")]
    public Tile GroundTile;
    public Tile WallTile;
    public Tile ExitTile;

    [SerializeField]
    private int sperateCount;

    [Header("오브젝트")]
    [SerializeField]
    private GameObject[] Treasures;
    [SerializeField]
    private GameObject Core;    
    [SerializeField]
    private GameObject[] Monster;
    [SerializeField]
    private GameObject ExitPlatform;
    [SerializeField]
    private int[] countPerMonster;
    [SerializeField]
    private int countPerTreasure;    

    [Header("생성 위치")]
    [SerializeField]
    private GameObject TreasureParent;
    [SerializeField]
    private GameObject MonsterParent;

    private const int maxX = 96;
    private const int minX = -96;    
    private const int maxY = 60;
    private const int minY = -60;

    private int[] dx = new int[4] { 0, 0, -1, 1 };
    private int[] dy = new int[4] { 1, -1, 0, 0 };

    List<Room> rooms = new List<Room>();
    HashSet<Vector3Int> hs = new HashSet<Vector3Int>();

    private void Start()
    {
        SeperateRoom();
        ApplyRoomToTilemap();
        GenerateRoad();
        GenerateExit();
        AddShadowCastToWallMap();
        GenerateTreasure(countPerTreasure);
        GenerateMonster();
    }
    void SeperateRoom()
    {
        rooms.Clear();
        rooms.Add(new Room(0, minX, maxX, minY, maxY));
        for(int i = 0; i < sperateCount; i++)
        {
            int roomCount = rooms.Count;            
            int roomNum = 1;
            for(int j = 0; j < roomCount; j++)
            {
                Room room = rooms[0];
                rooms.RemoveAt(0);
                int roomMinX = room.DownLeft.x;
                int roomMaxX = room.UpRight.x;
                int roomMinY = room.DownLeft.y;
                int roomMaxY = room.UpRight.y;

                int distX = room.UpRight.x - room.DownLeft.x;
                int distY = room.UpRight.y - room.DownLeft.y;
                
                if (distX >= distY && distX > 40)
                {
                    int offset = Random.Range(20, distX - 20);                    
                    rooms.Add(new Room(roomNum++, roomMinX, roomMinX + offset, roomMinY, roomMaxY));
                    rooms.Add(new Room(roomNum++, roomMinX + offset + 1, roomMaxX, roomMinY, roomMaxY));                                       
                }
                else if (distX < distY && distY > 40)
                {
                    int offset = Random.Range(20, distY - 20);                    
                    rooms.Add(new Room(roomNum++, roomMinX, roomMaxX, roomMinY, roomMinY + offset));
                    rooms.Add(new Room(roomNum++, roomMinX, roomMaxX, roomMinY + offset + 1, roomMaxY));                                       
                }                
                else
                {
                    rooms.Add(new Room(roomNum++, roomMinX, roomMaxX, roomMinY, roomMaxY));
                }
            }
        }
    }
    void ApplyRoomToTilemap()
    {        
        WallMap.ClearAllTiles();
        int roomCount = rooms.Count;
        for (int i = 0; i < roomCount; i++)
        {
            Room room = rooms[i];            
            for(int x = room.DownLeft.x; x <= room.UpRight.x; x++)
            {
                for(int y = room.DownLeft.y; y <= room.UpRight.y; y++)
                {                    
                    if (x == room.DownLeft.x || x == room.UpRight.x || y == room.DownLeft.y || y == room.UpRight.y)                    
                        WallMap.SetTile(new Vector3Int(x, y), WallTile);                                            
                }
            }
        }
    }    
    void GenerateRoad()
    {
        int roomCount = rooms.Count;
        for (int i = 0; i < roomCount; i++)
        {
            Room room = rooms[i];
            int roomMinX = room.DownLeft.x;
            int roomMaxX = room.UpRight.x;
            int roomMinY = room.DownLeft.y;
            int roomMaxY = room.UpRight.y;

            int distX = room.UpRight.x - room.DownLeft.x;
            int distY = room.UpRight.y - room.DownLeft.y;

            int offset;
            if (roomMinX != minX)
            {                
                while (true)
                {
                    offset = Random.Range(2, distY - 3);

                    if (WallMap.GetTile(new Vector3Int(roomMinX - 2, roomMinY + offset)) == WallTile || 
                        WallMap.GetTile(new Vector3Int(roomMinX - 2, roomMinY + offset + 1)) == WallTile ||
                        WallMap.GetTile(new Vector3Int(roomMinX - 2, roomMinY + offset + 2)) == WallTile)
                        continue;
                    else
                        break;
                }
                SetWallTile(roomMinX - 1, roomMinX, roomMinY + offset, roomMinY + offset + 2, null);
            }

            if (roomMaxX != maxX)
            {
                while (true)
                {
                    offset = Random.Range(2, distY - 2);

                    if (WallMap.GetTile(new Vector3Int(roomMaxX + 2, roomMinY + offset)) == WallTile ||
                        WallMap.GetTile(new Vector3Int(roomMaxX + 2, roomMinY + offset + 1)) == WallTile)
                        continue;
                    else
                        break;
                }                                
                SetWallTile(roomMaxX, roomMaxX + 1, roomMinY + offset, roomMinY + offset + 1, null);
            }

            if (roomMinY != minY)
            {
                while (true)
                {
                    offset = Random.Range(2, distX - 3);

                    if (WallMap.GetTile(new Vector3Int(roomMinX + offset, roomMinY - 2)) == WallTile ||
                        WallMap.GetTile(new Vector3Int(roomMinX + offset + 1, roomMinY - 2)) == WallTile ||
                        WallMap.GetTile(new Vector3Int(roomMinX + offset + 2, roomMinY - 2)) == WallTile)
                        continue;
                    else
                        break;
                }                
                SetWallTile(roomMinX + offset, roomMinX + offset + 2, roomMinY - 1, roomMinY, null);
            }

            if (roomMaxY != maxY)
            {
                while (true)
                {
                    offset = Random.Range(2, distX - 2);

                    if (WallMap.GetTile(new Vector3Int(roomMinX + offset, roomMaxY + 2)) == WallTile ||
                        WallMap.GetTile(new Vector3Int(roomMinX + offset + 1, roomMaxY + 2)) == WallTile)
                        continue;
                    else
                        break;
                }                
                SetWallTile(roomMinX + offset, roomMinX + offset + 1, roomMaxY, roomMaxY + 1, null);
            }
        }
    }
    void GenerateTreasure(int count)
    {
        Vector3Int pos = new Vector3Int(0, 0);
        for (int i = 0; i < Treasures.Length; i++)
        {
            for(int j = 0; j < count; j++)
            {                                
                pos = GetPossibleSpawnPostion();
                Instantiate(Treasures[i], pos, Quaternion.identity, TreasureParent.transform);
                hs.Add(pos);
            }
        }

        pos = GetPossibleSpawnPostion();
        Instantiate(Core, pos, Quaternion.identity, TreasureParent.transform);
        hs.Add(pos);
    }
    void GenerateExit()
    {
        int direction = Random.Range(0, 4);        
        int center = 0;
        int exitMinX = 0, exitMaxX = 0, exitMinY = 0, exitMaxY = 0;

        if (direction == 0)
        {
            for(int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].UpRight.y == maxY)
                {
                    center = (rooms[i].DownLeft.x + rooms[i].UpRight.x) / 2;
                    exitMinX = center - 1;
                    exitMaxX = center + 1;
                    exitMinY = maxY;
                    exitMaxY = maxY;
                    Instantiate(ExitPlatform, new Vector3(center + 0.5f, maxY - 1.5f), Quaternion.identity, null);
                    break;
                }
            }
        }
        else if (direction == 1)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].DownLeft.y == minY)
                {
                    center = (rooms[i].DownLeft.x + rooms[i].UpRight.x) / 2;                    
                    exitMinX = center - 1;
                    exitMaxX = center + 1;
                    exitMinY = minY;
                    exitMaxY = minY;
                    Instantiate(ExitPlatform, new Vector3(center + 0.5f, minY + 2.5f), Quaternion.identity, null);
                    break;
                }
            }
        }
        else if (direction == 2)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].DownLeft.x == minX)
                {
                    center = (rooms[i].DownLeft.y + rooms[i].UpRight.y) / 2;                    
                    exitMinX = minX;
                    exitMaxX = minX;
                    exitMinY = center - 1;
                    exitMaxY = center + 1;
                    Instantiate(ExitPlatform, new Vector3(minX + 2.5f, center + 0.5f), Quaternion.identity, null);
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (rooms[i].UpRight.x == maxX)
                {
                    center = (rooms[i].DownLeft.y + rooms[i].UpRight.y) / 2;                    
                    exitMinX = maxX;
                    exitMaxX = maxX;
                    exitMinY = center - 1;
                    exitMaxY = center + 1;
                    Instantiate(ExitPlatform, new Vector3(maxX - 1.5f, center + 0.5f), Quaternion.identity, null);
                    break;
                }
            }
        }
        SetWallTile(exitMinX, exitMaxX, exitMinY, exitMaxY, ExitTile);
    }
    void GenerateMonster()
    {
        Vector3Int pos = new Vector3Int(0, 0);
        for (int i = 0; i < Monster.Length; i++)
        {
            for (int j = 0; j < countPerMonster[i]; j++)
            {
                pos = GetPossibleSpawnPostion();
                Instantiate(Monster[i], pos, Quaternion.identity, MonsterParent.transform);
                hs.Add(pos);
            }
        }        
    }
    Vector3Int GetPossibleSpawnPostion()
    {
        // 벽이 없고 이미 하나의 물건, 몬스터가 스폰되지 않은 위치.
        Vector3Int pos = new Vector3Int();
        while (true)
        {
            pos.x = Random.Range(minX, maxX);
            pos.y = Random.Range(minY, maxY);

            if (WallMap.HasTile(pos + new Vector3Int(dx[0], dy[0])) ||
                WallMap.HasTile(pos + new Vector3Int(dx[1], dy[1])) ||
                WallMap.HasTile(pos + new Vector3Int(dx[2], dy[2])) ||
                WallMap.HasTile(pos + new Vector3Int(dx[3], dy[3])))
                continue;
            if (WallMap.HasTile(pos) || hs.Contains(pos))
                continue;
            break;
        }
        hs.Add(pos);
        return pos;
    }
    void SetWallTile(int minX, int maxX, int minY, int maxY, Tile tile)
    {
        for(int i = minX; i <= maxX; i++)
        {
            for(int j = minY; j <= maxY; j++)
            {
                WallMap.SetTile(new Vector3Int(i, j), tile);
            }
        }
    }
    void AddShadowCastToWallMap()
    {
        foreach (var pos in WallMap.cellBounds.allPositionsWithin)
        {
            if (WallMap.HasTile(pos))
            {
                Vector3Int tilePosition = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 worldPosition = WallMap.CellToWorld(tilePosition);

                GameObject shadowCaster = Instantiate(shadowCasterPrefab, worldPosition, Quaternion.identity, transform);
                shadowCaster.GetComponent<ShadowCaster2D>().useRendererSilhouette = true;
            }
        }
    }            
}
