using UnityEngine;

public class EyeConeMesh2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform meshRoot;      // VisionMesh-child
    [SerializeField] private LayerMask obstacleMask;  // Cover-lag

    [Header("Shape Settings")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private int rayCount = 60;

    [Header("Facing (samme som EyeScanner2D)")]
    [Tooltip("Samme SpriteRenderer som på fjenden (bliver flipX'et).")]
    [SerializeField] private SpriteRenderer enemySprite;

    [Tooltip("TRUE hvis fjendens sprite kigger mod højre som default. FALSE hvis den kigger mod venstre.")]
    [SerializeField] private bool spriteFacesRightByDefault = true;

    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        if (meshRoot == null)
        {
            Debug.LogError("[EyeConeMesh2D] meshRoot mangler – drag VisionMesh ind i inspector.");
            enabled = false;
            return;
        }

        _meshFilter = meshRoot.GetComponent<MeshFilter>();
        if (_meshFilter == null)
            _meshFilter = meshRoot.gameObject.AddComponent<MeshFilter>();

        _meshRenderer = meshRoot.GetComponent<MeshRenderer>();
        if (_meshRenderer == null)
            _meshRenderer = meshRoot.gameObject.AddComponent<MeshRenderer>();

        _mesh = new Mesh();
        _mesh.name = "VisionConeMesh";
        _meshFilter.mesh = _mesh;

        _meshRenderer.sortingLayerName = "Default";
        _meshRenderer.sortingOrder = 50;

        if (_meshRenderer.sharedMaterial == null)
        {
            Debug.LogWarning("[EyeConeMesh2D] Intet material – giv VisionMesh et material med 'Sprites/Default' og alpha.");
        }
    }

    private void LateUpdate()
    {
        GenerateMesh();
    }

    private Vector2 GetForward()
    {
        // Samme udgangspunkt som før: cone pegede nedad
        Vector2 forward = -transform.up;

        // Spejl vandret baseret på fjendens flipX (samme logik som EyeScanner2D)
        if (enemySprite != null)
        {
            int dir;
            if (spriteFacesRightByDefault)
            {
                dir = enemySprite.flipX ? -1 : 1;
            }
            else
            {
                dir = enemySprite.flipX ? 1 : -1;
            }

            forward = new Vector2(forward.x * dir, forward.y);
        }

        return forward.normalized;
    }

    private void GenerateMesh()
    {
        if (!enabled) return;

        Vector3 origin = transform.position;
        Vector3 forward = GetForward();

        int vertexCount = rayCount + 2;
        var vertices = new Vector3[vertexCount];
        var triangles = new int[rayCount * 3];

        vertices[0] = meshRoot.InverseTransformPoint(origin);

        float halfAngle = viewAngle * 0.5f;
        float angleStep = viewAngle / (rayCount - 1);

        int vIndex = 1;
        int tIndex = 0;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = -halfAngle + angleStep * i;
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            Vector3 dir = rot * forward;

            RaycastHit2D hit = Physics2D.Raycast(origin, dir, viewRadius, obstacleMask);

            Vector3 endPoint = hit.collider ? (Vector3)hit.point : origin + dir * viewRadius;

            vertices[vIndex] = meshRoot.InverseTransformPoint(endPoint);

            if (i < rayCount - 1)
            {
                triangles[tIndex + 0] = 0;
                triangles[tIndex + 1] = vIndex;
                triangles[tIndex + 2] = vIndex + 1;
                tIndex += 3;
            }

            vIndex++;
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateBounds();
    }

    private void OnDrawGizmosSelected()
    {
        if (meshRoot == null) return;

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.35f);
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
