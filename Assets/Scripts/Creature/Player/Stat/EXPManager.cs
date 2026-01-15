using UnityEngine.UI;
using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class EXPManager : MonoBehaviour
{
    public static EXPManager instance;
    [SerializeField] private GameObject _player;
    [SerializeField] private LevelUpUI levelUpUI;
    [SerializeField] private GameObject _expTextPrefab;
    [SerializeField] private GameObject _levelUpPrefab;
    public Transform worldCanvasTransform;
    private float _offsetX;
    private float _offsetY;
    private Vector3 _offset = new Vector3(0.0f, 0.5f, 0.0f);
    private Transform _playerTransform;
    private PlayerStateController _playerStateController;


    private void Awake()
    {
        _playerTransform = _player.GetComponent<Transform>();
        _playerStateController = _player.GetComponent<PlayerStateController>();
        instance = this;
    }

    public void LevelUpUI()
    {
        StartCoroutine(ShowLevelUpUI());
    }

    private IEnumerator ShowLevelUpUI()
    {
        yield return new WaitForSeconds(1.0f);
        levelUpUI.Show();
        GameObject obj = Instantiate(_levelUpPrefab,
                _playerTransform.position + _offset,
                Quaternion.identity,
                worldCanvasTransform);
    }

    public void ShowExpText(float exp)
    {
        _offset.x = Random.Range(-0.5f, 0.5f);
        _offset.y = Random.Range(0.1f, 0.5f);

        GameObject obj = Instantiate(_expTextPrefab,
            _playerTransform.position + _offset, 
            Quaternion.identity,
            worldCanvasTransform);

        EXPText text = obj.GetComponent<EXPText>();
        text.SetExp(exp);
    }

    private void Update()
    {
        // 테스트용: P 키로 증강 팝업 강제 오픈
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(ShowLevelUpUI());
        }
    }
}
