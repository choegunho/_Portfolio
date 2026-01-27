using UnityEngine;
using System.Collections.Generic;

public class WallTransparencyController : MonoBehaviour
{
    [Header("References")]
    private Transform target;      // 캐릭터
    private Camera mainCamera;

    [Header("Materials")]
    [SerializeField] private Material opaqueMaterial;       // URP/Lit
    [SerializeField] private Material transparentMaterial;  // Unlit Transparent

    [Header("Settings")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float raycastPadding = 0.2f;

    // 현재 프레임에 가려진 Renderer
    private readonly HashSet<Renderer> currentHits = new();
    private readonly HashSet<Renderer> previousHits = new();

    private void Awake()
    {
        target = GameManager.Instance.PlayerTransform;
        mainCamera = GameManager.Instance.Camera;
    }

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (target == null || mainCamera == null)
            return;

        currentHits.Clear();

        Vector3 from = mainCamera.transform.position;
        Vector3 to = target.position;
        Vector3 dir = to - from;
        float dist = dir.magnitude - raycastPadding;
        if (dist <= 0.01f) return;

        RaycastHit[] hits = Physics.RaycastAll(from, dir.normalized, dist, wallLayer);

        foreach (RaycastHit hit in hits)
        {
            Renderer r = hit.collider.GetComponentInParent<Renderer>();
            if (r == null) continue;

            currentHits.Add(r);

            // 새로 가려진 경우 → 투명
            if (!previousHits.Contains(r))
            {
                SetTransparent(r, true);
            }
        }

        // 더 이상 가리지 않는 벽 → 복구
        foreach (Renderer r in previousHits)
        {
            if (!currentHits.Contains(r))
            {
                SetTransparent(r, false);
            }
        }

        // 상태 교체
        previousHits.Clear();
        foreach (var r in currentHits)
            previousHits.Add(r);
    }

    private void SetTransparent(Renderer renderer, bool transparent)
    {
        if (renderer == null) return;

        // 여러 서브메시 대응
        Material[] mats = renderer.sharedMaterials;

        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = transparent ? transparentMaterial : opaqueMaterial;
        }

        renderer.sharedMaterials = mats;
    }

    // 디버그용
    void OnDrawGizmos()
    {
        if (target != null && mainCamera != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(mainCamera.transform.position, target.position);
        }
    }
}
