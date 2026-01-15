using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
