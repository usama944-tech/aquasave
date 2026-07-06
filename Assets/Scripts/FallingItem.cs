using UnityEngine;

/// <summary>
/// Attached to both the WaterDrop and PollutionDrop prefabs. Falls straight down,
/// reports a catch to the GameManager on hitting the bucket, and destroys itself
/// if it falls past the bottom of the screen (missed).
/// </summary>
public class FallingItem : MonoBehaviour
{
    [Tooltip("Marks whether this drop is clean water (true) or pollution (false).")]
    public bool isCleanWater = true;

    [Tooltip("Downward fall speed in world units per second.")]
    public float fallSpeed = 3f;

    [Tooltip("World Y position below which this item is considered missed and destroyed.")]
    public float destroyYThreshold = -6f;

    [Header("Catch Feedback")]
    [Tooltip("Sprite used for the quick scale-and-fade splash effect on catch (a simple white circle/pixel works well since it's tinted below).")]
    public Sprite splashSprite;
    [Tooltip("Tint applied to the splash effect: blue for clean water, brown for pollution.")]
    public Color splashColor = Color.white;

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < destroyYThreshold)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterCatch(isCleanWater);
        }

        SplashEffect.Spawn(splashSprite, transform.position, splashColor);

        Destroy(gameObject);
    }
}
