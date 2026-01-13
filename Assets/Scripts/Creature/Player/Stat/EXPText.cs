using UnityEngine.UI;
using UnityEngine;

public class EXPText : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private float _moveSpeed = 0.5f;
    [SerializeField] private float _time = 0.5f;

    public void SetExp(float amount)
    {
        _text.text = $"+{amount}exp";
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(70.0f, 0f, 0f);
        _time -= Time.deltaTime;

        if (_time <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
