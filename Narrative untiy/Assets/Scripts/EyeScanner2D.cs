using UnityEngine;
using UnityEngine.Events;

public class EyeScanner2D : MonoBehaviour
{
    [Header("View Settings")]
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private float viewAngle = 45f;

    [Header("Sweep Settings")]
    [Tooltip("Rotationshastighed i grader pr. sekund")]
    [SerializeField] private float rotationSpeed = 30f;

    [Tooltip("Min. offset fra startvinkel (grader, negativ)")]
    [SerializeField] private float minSweepAngle = -60f;

    [Tooltip("Max. offset fra startvinkel (grader, positiv)")]
    [SerializeField] private float maxSweepAngle = 60f;

    [Header("Layers")]
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask; // cover

    [Header("Events")]
    public UnityEvent OnPlayerSpotted;

    private float baseAngle;    // start-rotation (Z)
    private float sweepAngle;   // nuværende offset fra baseAngle
    private int sweepDir = 1;   // 1 = mod max, -1 = mod min

    private void Awake()
    {
        // Gem startvinkel (Z-komponenten)
        baseAngle = transform.eulerAngles.z;
        sweepAngle = 0f;
    }

    private void Update()
    {
        UpdateSweepRotation();
        ScanForPlayer();
    }

    private void UpdateSweepRotation()
    {
        // Opdater offset
        sweepAngle += rotationSpeed * sweepDir * Time.deltaTime;

        // Skift retning ved endepunkter
        if (sweepAngle > maxSweepAngle)
        {
            sweepAngle = maxSweepAngle;
            sweepDir = -1;
        }
        else if (sweepAngle < minSweepAngle)
        {
            sweepAngle = minSweepAngle;
            sweepDir = 1;
        }

        // Sæt rotation (kun Z)
        float z = baseAngle + sweepAngle;
        transform.rotation = Quaternion.Euler(0f, 0f, z);
    }

    private void ScanForPlayer()
    {
        // Find alle spillere inden for radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, viewRadius, playerMask);

        // Din cone peger NED: forward = -transform.up
        Vector2 forward = -transform.up;

        foreach (var hit in hits)
        {
            Vector2 dirToTarget = (hit.transform.position - transform.position).normalized;
            float angleToTarget = Vector2.Angle(forward, dirToTarget);

            // Kun inden for keglevinklen
            if (angleToTarget < viewAngle * 0.5f)
            {
                float distToTarget = Vector2.Distance(transform.position, hit.transform.position);

                // Raycast der kan ramme både Player og Cover
                int mask = playerMask | obstacleMask;

                RaycastHit2D firstHit = Physics2D.Raycast(
                    transform.position,
                    dirToTarget,
                    distToTarget,
                    mask
                );

                if (firstHit.collider != null)
                {
                    // Hvis det første hit er spilleren → set
                    if (firstHit.collider.CompareTag("Player"))
                    {
                        Debug.Log("[EyeScanner2D] Player spotted (no cover in front).");
                        OnPlayerSpotted?.Invoke();
                        return;
                    }

                    // Hvis første hit er cover → spilleren er skjult
                    // Debug.Log("[EyeScanner2D] View blocked by: " + firstHit.collider.name);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
