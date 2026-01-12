using JetBrains.Annotations;
using UnityEngine;

public class EXPManager : MonoBehaviour
{
    public static EXPManager instance;
    private Transform _player;
    [SerializeField] private GameObject _expTextPrefab;
    [SerializeField] private GameObject _levelUpPrefab;
    public Transform worldCanvasTransform;
    private float _offsetX;
    private float _offsetY;
    private Vector3 _offset = new Vector3(0.0f, 0.5f, 0.0f);

    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        instance = this;
    }
    public void ShowExpText(float exp)
    {
        _offset.x = Random.Range(-0.5f, 0.5f);
        _offset.y = Random.Range(0.1f, 0.5f);

        GameObject obj = Instantiate(_expTextPrefab,
            _player.position + _offset, 
            Quaternion.identity,
            worldCanvasTransform);

        EXPText text = obj.GetComponent<EXPText>();
        text.SetExp(exp);
    }

    public void ShowLevelUpText()
    {
        GameObject obj = Instantiate(_levelUpPrefab,
            _player.position + _offset,
            Quaternion.identity,
            worldCanvasTransform);
    }
}
