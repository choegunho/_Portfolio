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
    // Floating world-space texts (EXP popup) should be parented here (World Space canvas).
    public Transform worldCanvasTransform;
    private Vector3 _offset = new Vector3(0.0f, 0.5f, 0.0f);
    private Transform _playerTransform;

    private void Awake()
    {
        _playerTransform = _player.GetComponent<Transform>();
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
    }

    public void ShowExpText(float exp)
    {
        _offset.x = Random.Range(-0.5f, 0.5f);
        _offset.y = Random.Range(0.1f, 0.5f);

        Transform parent = worldCanvasTransform != null ? worldCanvasTransform : null;
        GameObject obj = Instantiate(
            _expTextPrefab,
            _playerTransform.position + _offset,
            Quaternion.identity,
            parent);

        EXPText text = obj.GetComponent<EXPText>();
        text.SetExp(exp);
    }
}
