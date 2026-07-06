using UnityEngine;

/// <summary>
/// Moves the bucket left/right with A/D or the arrow keys, and clamps it so it
/// never drifts off-screen. Bounds are calculated from the camera's actual visible
/// width at runtime, so this works correctly regardless of screen resolution or
/// aspect ratio, instead of relying on a hardcoded number.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PlayerController : MonoBehaviour
{
    [Tooltip("Horizontal movement speed in world units per second.")]
    public float moveSpeed = 8f;

    private float halfWidth;
    private float minX;
    private float maxX;

    private void Start()
    {
        CalculateBounds();
    }

    private void CalculateBounds()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        // Half the sprite's width in world units, used to keep the whole bucket on screen.
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        halfWidth = sr != null ? sr.bounds.extents.x : 0.5f;

        float camHalfHeight = cam.orthographicSize;
        float camHalfWidth = camHalfHeight * cam.aspect;

        minX = cam.transform.position.x - camHalfWidth + halfWidth;
        maxX = cam.transform.position.x + camHalfWidth - halfWidth;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal"); // A/D and Left/Right both map here by default.
        Vector3 pos = transform.position;
        pos.x += horizontal * moveSpeed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
