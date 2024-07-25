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
public class MapCreater : MonoBehaviour
{
    [SerializeField]
    private int sperateCount;
    [SerializeField]
    private Tilemap FieldMap;
    [SerializeField]
    private Tilemap WallMap;
    [SerializeField]
    private GameObject shadowCasterPrefab;

    [Header("≈∏¿œ")]
    public Tile GroundTile;
    public Tile WallTile;

    private const int maxX = 96;
    private const int minX = -96;    
    private const int maxY = 60;
    private const int minY = -60;

    List<Room> rooms = new List<Room>();

    private void Start()
    {
        SeperateRoom();
        ApplyRoomToTilemap();
        ConnectRoom();
        AddShadowCastToWallMap();
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
        FieldMap.ClearAllTiles();
        WallMap.ClearAllTiles();
        int roomCount = rooms.Count;
        for (int i = 0; i < roomCount; i++)
        {
            Room room = rooms[i];            
            for(int x = room.DownLeft.x; x <= room.UpRight.x; x++)
            {
                for(int y = room.DownLeft.y; y <= room.UpRight.y; y++)
                {
                    FieldMap.SetTile(new Vector3Int(x, y), GroundTile);
                    if (x == room.DownLeft.x || x == room.UpRight.x || y == room.DownLeft.y || y == room.UpRight.y)                    
                        WallMap.SetTile(new Vector3Int(x, y), WallTile);                                            
                }
            }
        }
    }
    
    void ConnectRoom()
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
                    offset = Random.Range(2, distY - 2);

                    if (WallMap.GetTile(new Vector3Int(roomMinX - 2, roomMinY + offset)) == WallTile || 
                        WallMap.GetTile(new Vector3Int(roomMinX - 2, roomMinY + offset + 1)) == WallTile)
                        continue;
                    else
                        break;
                }
                SetWallTile(roomMinX - 1, roomMinX, roomMinY + offset, roomMinY + offset + 1, null);
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
                    offset = Random.Range(2, distX - 2);

                    if (WallMap.GetTile(new Vector3Int(roomMinX + offset, roomMinY - 2)) == WallTile ||
                        WallMap.GetTile(new Vector3Int(roomMinX + offset + 1, roomMinY - 2)) == WallTile)
                        continue;
                    else
                        break;
                }                
                SetWallTile(roomMinX + offset, roomMinX + offset + 1, roomMinY - 1, roomMinY, null);
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

    public void OnClickGenerateMap()
    {
        SeperateRoom();
        ApplyRoomToTilemap();
        ConnectRoom();        
    }
}
