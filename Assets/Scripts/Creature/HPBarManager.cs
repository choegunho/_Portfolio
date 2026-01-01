using UnityEngine;

public class HPBarManager : MonoBehaviour
{
    public static HPBarManager Instance;
    public GameObject monsterHealthBarPrefab;
    public Transform worldCanvasTransform;

    private void Awake()
    {
        Instance = this;
    }
}
