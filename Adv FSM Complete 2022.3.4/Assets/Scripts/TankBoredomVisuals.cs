using UnityEngine;
using System.Collections;

public class TankBoredomVisuals : MonoBehaviour
{
    private ParticleSystem frustrationParticles;
    private Transform iconTransform;
    private MeshRenderer iconRenderer;
    private bool isInitialized = false;
    
    private const float IconDisplayDuration = 2f;
    private const float IconFloatHeight = 3f;
    private const float IconBobSpeed = 2f;
    private const float IconBobAmount = 0.3f;

    public void ShowBoredomEffect()
    {
        if (!isInitialized)
        {
            Initialize();
        }
        
        if (frustrationParticles != null)
        {
            frustrationParticles.Play();
        }
        
        if (iconTransform != null)
        {
            StartCoroutine(DisplayBoredomIcon());
        }
    }

    private void Initialize()
    {
        CreateFrustrationParticles();
        CreateBoredomIcon();
        isInitialized = true;
    }

    private void CreateFrustrationParticles()
    {
        GameObject particleObj = new GameObject("FrustrationParticles");
        particleObj.transform.SetParent(transform);
        particleObj.transform.localPosition = Vector3.up * 2f;
        
        frustrationParticles = particleObj.AddComponent<ParticleSystem>();
        
        var main = frustrationParticles.main;
        main.startLifetime = 1.5f;
        main.startSpeed = 5f;
        main.startSize = 0.5f;
        main.startColor = new Color(0.8f, 0.3f, 0.3f, 1f);
        main.gravityModifier = -0.5f;
        main.maxParticles = 20;
        main.playOnAwake = false;
        main.loop = false;
        
        var emission = frustrationParticles.emission;
        emission.rateOverTime = 0;
        emission.SetBursts(new ParticleSystem.Burst[] { new ParticleSystem.Burst(0f, 15, 20, 0.1f) });
        
        var shape = frustrationParticles.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle = 25f;
        shape.radius = 0.5f;
        
        var colorOverLifetime = frustrationParticles.colorOverLifetime;
        colorOverLifetime.enabled = true;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { 
                new GradientColorKey(new Color(1f, 0.4f, 0.4f), 0f),
                new GradientColorKey(new Color(0.6f, 0.6f, 0.6f), 1f)
            },
            new GradientAlphaKey[] { 
                new GradientAlphaKey(1f, 0f), 
                new GradientAlphaKey(0f, 1f) 
            }
        );
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(gradient);
        
        var sizeOverLifetime = frustrationParticles.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        AnimationCurve curve = new AnimationCurve();
        curve.AddKey(0f, 1f);
        curve.AddKey(1f, 0.2f);
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, curve);
        
        var renderer = frustrationParticles.GetComponent<ParticleSystemRenderer>();
        renderer.renderMode = ParticleSystemRenderMode.Billboard;
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        
        frustrationParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    private void CreateBoredomIcon()
    {
        GameObject iconObj = new GameObject("BoredomIcon");
        iconObj.transform.SetParent(transform);
        iconObj.transform.localPosition = Vector3.up * IconFloatHeight;
        iconObj.transform.localRotation = Quaternion.identity;
        iconTransform = iconObj.transform;
        
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.SetParent(iconObj.transform);
        quad.transform.localPosition = Vector3.zero;
        quad.transform.localScale = Vector3.one * 1.5f;
        
        MeshCollider meshCollider = quad.GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            DestroyImmediate(meshCollider);
        }
        
        iconRenderer = quad.GetComponent<MeshRenderer>();
        Material iconMaterial = new Material(Shader.Find("Sprites/Default"));
        
        Texture2D iconTexture = CreateSleepyFaceTexture();
        iconMaterial.mainTexture = iconTexture;
        iconMaterial.color = Color.white;
        iconRenderer.material = iconMaterial;
        
        iconObj.SetActive(false);
    }

    private Texture2D CreateSleepyFaceTexture()
    {
        int size = 128;
        Texture2D texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Bilinear;
        
        Color transparent = new Color(1, 1, 1, 0);
        Color yellow = new Color(1f, 0.9f, 0.3f, 1f);
        Color darkBrown = new Color(0.2f, 0.1f, 0.05f, 1f);
        
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                texture.SetPixel(x, y, transparent);
            }
        }
        
        DrawCircle(texture, size / 2, size / 2, size / 2 - 4, yellow);
        
        DrawLine(texture, size / 3, size * 2 / 3, size / 3 + 15, size * 2 / 3 - 5, 3, darkBrown);
        DrawLine(texture, size * 2 / 3, size * 2 / 3, size * 2 / 3 - 15, size * 2 / 3 - 5, 3, darkBrown);
        
        DrawArc(texture, size / 2, size / 3, 20, 180, 360, 3, darkBrown);
        
        DrawText(texture, size / 2 + 20, size / 2 + 10, "Z", darkBrown, 0.8f);
        DrawText(texture, size / 2 + 30, size / 2 + 20, "z", darkBrown, 0.6f);
        DrawText(texture, size / 2 + 38, size / 2 + 28, "z", darkBrown, 0.4f);
        
        texture.Apply();
        return texture;
    }

    private void DrawCircle(Texture2D texture, int centerX, int centerY, int radius, Color color)
    {
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    int px = centerX + x;
                    int py = centerY + y;
                    if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                    {
                        texture.SetPixel(px, py, color);
                    }
                }
            }
        }
    }

    private void DrawLine(Texture2D texture, int x0, int y0, int x1, int y1, int thickness, Color color)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            for (int ty = -thickness / 2; ty <= thickness / 2; ty++)
            {
                for (int tx = -thickness / 2; tx <= thickness / 2; tx++)
                {
                    int px = x0 + tx;
                    int py = y0 + ty;
                    if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                    {
                        texture.SetPixel(px, py, color);
                    }
                }
            }

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }
    }

    private void DrawArc(Texture2D texture, int centerX, int centerY, int radius, float startAngle, float endAngle, int thickness, Color color)
    {
        for (float angle = startAngle; angle < endAngle; angle += 1f)
        {
            float rad = angle * Mathf.Deg2Rad;
            int x = centerX + (int)(Mathf.Cos(rad) * radius);
            int y = centerY + (int)(Mathf.Sin(rad) * radius);
            
            for (int ty = -thickness / 2; ty <= thickness / 2; ty++)
            {
                for (int tx = -thickness / 2; tx <= thickness / 2; tx++)
                {
                    int px = x + tx;
                    int py = y + ty;
                    if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                    {
                        texture.SetPixel(px, py, color);
                    }
                }
            }
        }
    }

    private void DrawText(Texture2D texture, int x, int y, string text, Color color, float scale)
    {
        int charSize = (int)(20 * scale);
        for (int dy = 0; dy < charSize; dy++)
        {
            for (int dx = 0; dx < charSize; dx++)
            {
                int px = x + dx;
                int py = y + dy;
                if (px >= 0 && px < texture.width && py >= 0 && py < texture.height)
                {
                    if ((dx + dy) % 3 == 0)
                    {
                        texture.SetPixel(px, py, color);
                    }
                }
            }
        }
    }

    private IEnumerator DisplayBoredomIcon()
    {
        iconTransform.gameObject.SetActive(true);
        
        float elapsed = 0f;
        Vector3 basePosition = iconTransform.localPosition;
        
        while (elapsed < IconDisplayDuration)
        {
            elapsed += Time.deltaTime;
            
            float bobOffset = Mathf.Sin(elapsed * IconBobSpeed) * IconBobAmount;
            iconTransform.localPosition = basePosition + Vector3.up * bobOffset;
            
            iconTransform.rotation = Camera.main != null 
                ? Quaternion.LookRotation(iconTransform.position - Camera.main.transform.position)
                : Quaternion.identity;
            
            float alpha = 1f;
            if (elapsed > IconDisplayDuration - 0.5f)
            {
                alpha = (IconDisplayDuration - elapsed) / 0.5f;
            }
            
            Color currentColor = iconRenderer.material.color;
            currentColor.a = alpha;
            iconRenderer.material.color = currentColor;
            
            yield return null;
        }
        
        iconTransform.gameObject.SetActive(false);
        iconRenderer.material.color = Color.white;
    }
}
