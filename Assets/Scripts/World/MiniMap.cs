using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public static MiniMap Instance;
    [SerializeField] private RectTransform _roomContainer;
    [SerializeField] private Image _roomPrefab;
    [SerializeField] private Image _bossRoomPrefab;

    private Color _currentRoomColor = new Color(0.8f, 0.8f, 0.8f);
    private Color _normalRoomColor = new Color(0.25f, 0.25f, 0.25f);

    private float _roomSize = 35f; // 방 간 최소 거리
    private float _spacing = 5f;   // 방 간 여유 공간

    private Dictionary<RoomController, Image> _rooms = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void AddRoom(RoomController room)
    {
        if (_rooms.ContainsKey(room)) return;

        Image roomImage;

        if (room._roomType == RoomType.Boss)
        {
            roomImage = Instantiate(_bossRoomPrefab, _roomContainer);
        }
        else
        {
            roomImage = Instantiate(_roomPrefab, _roomContainer);
        }

        roomImage.color = roomTypeColor(room);
        _rooms.Add(room, roomImage);

        UpdateAllRoomPositions();
    }

    private Color roomTypeColor(RoomController room)
    {
        if (room.CurrentRoom)
            return _currentRoomColor;
        return _normalRoomColor;
    }

    private void UpdateAllRoomPositions()
    {
        if (_rooms.Count == 0) return;

        // 전체 방 최소/최대 위치 계산
        Vector2 minPos = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 maxPos = new Vector2(float.MinValue, float.MinValue);

        foreach (var room in _rooms.Keys)
        {
            minPos = Vector2.Min(minPos, room.gridPos);
            maxPos = Vector2.Max(maxPos, room.gridPos);
        }

        // 중앙 기준 offset
        Vector2 center = (minPos + maxPos) / 2f;

        foreach (var kvp in _rooms)
        {
            Vector2 pos = (Vector2)kvp.Key.gridPos * (_roomSize + _spacing) - center * (_roomSize + _spacing);
            kvp.Value.rectTransform.localPosition = pos;
        }
    }

    public void SetCurrentRoom(RoomController currentRoom)
    {
        foreach (var kvp in _rooms)
        {
            kvp.Value.color = roomTypeColor(kvp.Key);
        }
    }

    public void ResetMiniMap()
    {
        foreach(Transform child in _roomContainer)
        {
            Destroy(child.gameObject);
        }
        _rooms.Clear();
    }
}
