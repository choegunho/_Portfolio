using UnityEngine;

public class EliteMonsterOutline : MonoBehaviour
{
    private SkinnedMeshRenderer _outline;

    private void Awake()
    {
        _outline = GetComponent<SkinnedMeshRenderer>();
    }

    public void Activate()
    {
        _outline.enabled = true;
    }
}
