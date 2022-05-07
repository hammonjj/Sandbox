using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class ZoneGenerator : MonoBehaviourBase
{
    public enum Direction
    {
        Left,
        Right,
        Up,
        Down,
        None
    }

    [Header("ZoneGenerator")]
    public int RoomsToGenerate = 30;
    public float TileOffset = 0.5f;
    public GameObject[] RoomPrefabs;
    public LayerMask RoomMask;
    public LayerMask ZoneBoundaryMask;
    public Direction MapFlowDirection;
    public int MapFlowDirectionMultiplier;
    
    [Header("Lottery Tickets")]
    public int RoomLR;
    public int RoomLRB;
    public int RoomLRT;
    public int RoomLRTB;

    public int RoomTB;
    public int RoomLT;
    public int RoomLB;
    public int RoomRT;
    public int RoomRB;

    private Direction _sourceDirection;
    private List<GameObject> _roomsGenerated = new List<GameObject>();

    private void Start()
    {
        //Spawn First Room - Need to kill this code
        //var roomObject = Instantiate(RoomPrefabs[0], transform.position, Quaternion.identity);
        //_roomsGenerated.Add(roomObject);

        //_sourceDirection = Direction.Left;
        //transform.position = new Vector3(transform.position.x - TileOffset, transform.position.y);

        _sourceDirection = MapFlowDirection;

        //Create Main Path
        for(var i = 0; i < RoomsToGenerate; i++)
        {
            _SpawnRoom();
        }

        if(_roomsGenerated.Count < RoomsToGenerate)
        {
            LogError("Failed to find full path");
        }
        
        //Iterate through rooms and allow them to:
        //  - Spawn extra rooms
        //  - Seel off unused room exits
    }

    private void _SpawnRoom()
    {
        //Generation Workflow
        //1) Pick a direction to move based on what is valid from our previous room (and based on size constraints)
        //2) From the new direction, pick an appropriate room type
        //3) Instantiate the new room object
        //4) Cache room object for the next iteration

        var nextDirection = _GetRandomNextDirection(_sourceDirection);

        if(nextDirection == Direction.None)
        {
            LogVerbose("No Valid Room"); 
            return;
        }

        var currentRoom = _GetRandomRoom(nextDirection, _sourceDirection);

        _roomsGenerated.Add(currentRoom);
        Instantiate(currentRoom, transform.position, Quaternion.identity);

        _sourceDirection = nextDirection;
        transform.position = _GetNextRoomPosition(nextDirection);

        //Check if we've hit a change in direction
        Vector2 direction = new Vector2();
        if(MapFlowDirection == Direction.Left)
        {
            direction = Vector2.left;
        }
        else if(MapFlowDirection == Direction.Right)
        {
            direction = Vector2.right;
        }
        else if(MapFlowDirection == Direction.Up)
        {
            direction = Vector2.up;
        }
        else if(MapFlowDirection == Direction.Down)
        {
            direction = Vector2.down;
        }

        var raycastHitCollection = Physics2D.RaycastAll(transform.position, direction, 30.0f);
        for(int i = 0; i < raycastHitCollection.Length; ++i)
        {
            if(raycastHitCollection[i].collider != null && 
                raycastHitCollection[i].collider.gameObject.CompareTag("DirectionChange") &&
                raycastHitCollection[i].distance == 0)
            {
                MapFlowDirection = raycastHitCollection[i].collider.gameObject.GetComponent<ZoneDirectionChange>().Direction;
            }
        }
    }

    private Direction _GetRandomNextDirection(Direction sourceDirection)
    {
        //This would be a good method to check the bounds of my map to ensure it's the right shape
        //and to check whether there are any filled/empty spaces around it
        var nextDirectionOptions = new List<Direction> { 
            Direction.Left, Direction.Right, Direction.Up, Direction.Down};

        //Remove where we have just been
        if(sourceDirection == Direction.Left)
        {
            nextDirectionOptions.Remove(Direction.Right);
        }
        else if(sourceDirection == Direction.Right)
        {
            nextDirectionOptions.Remove(Direction.Left);
        }
        else if(sourceDirection == Direction.Up)
        {
            nextDirectionOptions.Remove(Direction.Down);
        }
        else if(sourceDirection == Direction.Down)
        {
            nextDirectionOptions.Remove(Direction.Up);
        }

        //Remove direction opposite of our flow direction
        if(MapFlowDirection == Direction.Left)
        {
            nextDirectionOptions.Remove(Direction.Right);
        }
        else if(MapFlowDirection == Direction.Right)
        {
            nextDirectionOptions.Remove(Direction.Left);
        }
        else if(MapFlowDirection == Direction.Up)
        {
            nextDirectionOptions.Remove(Direction.Down);
        }
        else if(MapFlowDirection == Direction.Down)
        {
            nextDirectionOptions.Remove(Direction.Up);
        }

        //Remove directions that are in collider range of the edge
        //1) Get all colliders within range and see which are part of the zone boundary
        //2) If a direction collides with the boundary, remove that option (or options) from the list
        //3) Profit
        if(_OnZoneGenerationBoundary(Vector2.right, TileOffset))
        {
            nextDirectionOptions.Remove(Direction.Right);
        }
        if(_OnZoneGenerationBoundary(Vector2.left, TileOffset))
        {
            nextDirectionOptions.Remove(Direction.Left);
        }
        if(_OnZoneGenerationBoundary(Vector2.up, TileOffset))
        {
            nextDirectionOptions.Remove(Direction.Up);
        }
        if(_OnZoneGenerationBoundary(Vector2.down, TileOffset))
        {
            nextDirectionOptions.Remove(Direction.Down);
        }

        if(nextDirectionOptions.Count == 0)
        {
            return MapFlowDirection;
        }

        //Make our flow direction more likely
        if(nextDirectionOptions.Contains(MapFlowDirection))
        {
            for(int i = 0; i < MapFlowDirectionMultiplier; ++i)
            nextDirectionOptions.Add(MapFlowDirection);
        }

        return nextDirectionOptions[Random.Range(0, nextDirectionOptions.Count)];
    }

    private bool _OnZoneGenerationBoundary(Vector2 direction, float offset)
    {
        return Physics2D.Raycast(transform.position, direction, offset, ZoneBoundaryMask).collider == null;
    }

    private Vector2 _GetNextRoomPosition(Direction direction)
    {
        var nextRoomPosition = new Vector2();
        if(direction == Direction.Left)
        {
            nextRoomPosition = new Vector2(transform.position.x - TileOffset, transform.position.y);
        }
        else if(direction == Direction.Right)
        {
            nextRoomPosition = new Vector2(transform.position.x + TileOffset, transform.position.y);
        }
        else if(direction == Direction.Up)
        {
            nextRoomPosition = new Vector2(transform.position.x, transform.position.y + TileOffset);
        }
        else if(direction == Direction.Down)
        {
            nextRoomPosition = new Vector2(transform.position.x, transform.position.y - TileOffset);
        }

        return nextRoomPosition;
    }

    private GameObject _GetRandomRoom(Direction nextDirection, Direction sourceDirection)
    {
        var possibleRooms = new List<GameObject>();
        if(nextDirection == Direction.Left)
        {
            if(sourceDirection == Direction.Left)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LR], RoomLR);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRB], RoomLRB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRT], RoomLRT);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
            }
            else if(sourceDirection == Direction.Up)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRB], RoomLRB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LB], RoomLB);
            }
            else if(sourceDirection == Direction.Down)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRT], RoomLRT);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LT], RoomLT);
            }
        }
        else if(nextDirection == Direction.Right)
        {
            if(sourceDirection == Direction.Right)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LR], RoomLR);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRB], RoomLRB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRT], RoomLRT);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
            }
            else if(sourceDirection == Direction.Up)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRB], RoomLRB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.RB], RoomRB);
            }
            else if(sourceDirection == Direction.Down)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRT], RoomLRT);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.RT], RoomRT);
            }
        }
        else if(nextDirection == Direction.Up)
        {
            if(sourceDirection == Direction.Left)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRT], RoomLRT);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.RT], RoomRT);
            }
            else if(sourceDirection == Direction.Right)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRT], RoomLRT);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LT], RoomLT);
            }
            else if(sourceDirection == Direction.Up)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.TB], RoomTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
            }
        }
        else if(nextDirection == Direction.Down)
        {
            if(sourceDirection == Direction.Left)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRB], RoomLRB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.RB], RoomRB);
            }
            else if(sourceDirection == Direction.Right)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRB], RoomLRB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LB], RoomLB);
            }
            else if(sourceDirection == Direction.Down)
            {
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.TB], RoomTB);
                _GetLotteryEntries(possibleRooms, RoomPrefabs[(int)RoomOpenings.LRTB], RoomLRTB);
            }
        }

        return possibleRooms[Random.Range(0, possibleRooms.Count)];
    }

    private void _GetLotteryEntries(List<GameObject> possibleRooms, GameObject roomPrefab, int lotteryEntries)
	{
        for(int i = 0; i < lotteryEntries; ++i)
        {
            possibleRooms.Add(roomPrefab);
        }
    }
}
