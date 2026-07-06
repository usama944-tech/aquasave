using UnityEngine;

/// <summary>
/// Lightweight, code-only "splash" feedback effect. Spawned at the point a
/// drop is caught, it quickly scales up and fades out, then destroys itself.
/// Used for both the clean-water catch (blue) and the pollution catch (brown)
/// so every catch gives clear, immediate visual feedback without needing a
/// hand-authored particle system asset.
/// </summary>
public class SplashEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    private readonly float duration = 0.35f;
    private float timer;
    private Vector3 startScale;
    private Vector3 endScale;

    /// <summary>
    /// Creates and starts a splash effect at the given world position.
    /// </summary>
    public static void Spawn(Sprite sprite, Vector3 position, Color color)
    {
        if (sprite == null) return;

        GameObject go = new GameObject("SplashEffect");
        go.transform.position = position;

        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite;
        renderer.color = color;
        renderer.sortingOrder = 10;

        SplashEffect effect = go.AddComponent<SplashEffect>();
        effect.Init(renderer);
    }

    private void Init(SpriteRenderer renderer)
    {
        sr = renderer;
        startScale = Vector3.one * 0.3f;
        endScale = Vector3.one * 1.6f;
        transform.localScale = startScale;
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);

        transform.localScale = Vector3.Lerp(startScale, endScale, t);

        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
