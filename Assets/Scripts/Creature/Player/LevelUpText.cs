using UnityEngine.UI;
using UnityEngine;

public class LevelUpText : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] private float _moveSpeed = 1.0f;
    [SerializeField] private float _time = 1.0f;

    void Awake()
    {
        _text.text = "Level Up!";
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(70.0f, 0f, 0f);

        _time -= Time.deltaTime;
        if(_time <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
