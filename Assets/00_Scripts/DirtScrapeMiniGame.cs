using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class DirtScrapeMiniGame : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    public event Action<float> OnCompleted;


    [Header("Textures")]
    [SerializeField] private Texture2D dirtTexture;
    [SerializeField] private int textureSize = 256;

    [Header("Brush")]
    [SerializeField] private float brushRadius = 18f;
    [SerializeField] private float brushSoftness = 8f;
    [SerializeField, Range(0f, 1f)] private float eraseThreshold = 0.5f;

    [Header("Progress")]
    [SerializeField, Range(0f, 1f)] private float completionThreshold = 0.6f;
    [SerializeField] private Image progressFillImage;
    [SerializeField] private Text progressLabel;
    [SerializeField] private GameObject completeBanner;

    private RawImage _rawImage;
    private RectTransform _rectTransform;
    private Texture2D _runtimeTexture;
    private Color32[] _pixels;
    private bool[] _paintable;
    private bool[] _erased;
    private int _paintableCount;
    private int _erasedCount;
    private bool _completed;

    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
        _rectTransform = (RectTransform)transform;

        BuildRuntimeTexture();
        UpdateProgressUI(0f);
        if (completeBanner != null)
            completeBanner.SetActive(false);
    }

    private void BuildRuntimeTexture()
    {
        int size = textureSize;
        _runtimeTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        _pixels = new Color32[size * size];
        _paintable = new bool[size * size];
        _erased = new bool[size * size];

        if (dirtTexture != null)
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float u = (x + 0.5f) / size;
                    float v = (y + 0.5f) / size;
                    Color c = dirtTexture.GetPixelBilinear(u, v);
                    int i = y * size + x;
                    _pixels[i] = c;
                    _paintable[i] = c.a > 0.05f;
                }
            }
        }
        else
        {
            Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
            float radius = size * 0.45f;
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    int i = y * size + x;
                    bool inside = Vector2.Distance(new Vector2(x, y), center) <= radius;
                    _pixels[i] = inside ? new Color32(120, 90, 60, 255) : new Color32(0, 0, 0, 0);
                    _paintable[i] = inside;
                }
            }
        }

        _paintableCount = 0;
        for (int i = 0; i < _paintable.Length; i++)
        {
            if (_paintable[i])
                _paintableCount++;
        }

        _runtimeTexture.SetPixels32(_pixels);
        _runtimeTexture.Apply(false);
        _rawImage.texture = _runtimeTexture;
    }

    public void OnPointerDown(PointerEventData eventData) => Scrape(eventData);

    public void OnDrag(PointerEventData eventData) => Scrape(eventData);

    private void Scrape(PointerEventData eventData)
    {
        if (_completed)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _rectTransform, eventData.position, eventData.pressEventCamera, out Vector2 local))
            return;

        Rect rect = _rectTransform.rect;
        float u = (local.x - rect.x) / rect.width;
        float v = (local.y - rect.y) / rect.height;
        if (u < 0f || u > 1f || v < 0f || v > 1f)
            return;

        int size = textureSize;
        int cx = Mathf.RoundToInt(u * size);
        int cy = Mathf.RoundToInt(v * size);

        int r = Mathf.CeilToInt(brushRadius + brushSoftness);
        int minX = Mathf.Max(0, cx - r);
        int maxX = Mathf.Min(size - 1, cx + r);
        int minY = Mathf.Max(0, cy - r);
        int maxY = Mathf.Min(size - 1, cy + r);

        for (int y = minY; y <= maxY; y++)
        {
            for (int x = minX; x <= maxX; x++)
            {
                int i = y * size + x;
                if (!_paintable[i] || _erased[i])
                    continue;

                float dist = Vector2.Distance(new Vector2(x, y), new Vector2(cx, cy));
                float falloff = 1f - Mathf.Clamp01((dist - brushRadius) / Mathf.Max(0.01f, brushSoftness));
                if (falloff <= 0f)
                    continue;

                Color32 p = _pixels[i];
                float newAlpha01 = Mathf.Clamp01(p.a / 255f - falloff);
                p.a = (byte)Mathf.RoundToInt(newAlpha01 * 255f);
                _pixels[i] = p;

                if (newAlpha01 <= eraseThreshold)
                {
                    _erased[i] = true;
                    _erasedCount++;
                }
            }
        }

        int blockWidth = maxX - minX + 1;
        int blockHeight = maxY - minY + 1;
        var block = new Color32[blockWidth * blockHeight];
        for (int y = 0; y < blockHeight; y++)
        {
            for (int x = 0; x < blockWidth; x++)
            {
                block[y * blockWidth + x] = _pixels[(minY + y) * size + (minX + x)];
            }
        }

        _runtimeTexture.SetPixels32(minX, minY, blockWidth, blockHeight, block);
        _runtimeTexture.Apply(false);

        float progress = _paintableCount > 0 ? (float)_erasedCount / _paintableCount : 0f;
        UpdateProgressUI(progress);

        if (!_completed && progress >= completionThreshold)
        {
            _completed = true;
            if (completeBanner != null)
                completeBanner.SetActive(true);
            OnCompleted?.Invoke(progress);
        }
    }

    private void UpdateProgressUI(float progress)
    {
        if (progressFillImage != null)
            progressFillImage.fillAmount = progress;

        if (progressLabel != null)
            progressLabel.text = Mathf.RoundToInt(progress * 100f) + "%";
    }

    public void ResetGame()
    {
        _completed = false;
        _erasedCount = 0;
        if (completeBanner != null)
            completeBanner.SetActive(false);

        BuildRuntimeTexture();
        UpdateProgressUI(0f);
    }
}
