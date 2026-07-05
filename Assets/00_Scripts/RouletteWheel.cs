using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RouletteWheel : MonoBehaviour
{
    [SerializeField] private RectTransform wheelRoot;
    [SerializeField] private Text labelTemplate;
    [SerializeField] private string[] customerNames =
    {
        "손님 A", "손님 B", "손님 C", "손님 D", "손님 E", "손님 F"
    };
    [SerializeField] private float labelRadius = 220f;
    [SerializeField] private float spinDuration = 3f;
    [SerializeField] private int minFullSpins = 4;

    private bool _spinning;

    private void Awake()
    {
        labelTemplate.gameObject.SetActive(false);
        BuildLabels();
    }

    private void BuildLabels()
    {
        int count = customerNames.Length;
        float segmentAngle = 360f / count;

        for (int i = 0; i < count; i++)
        {
            Text label = Instantiate(labelTemplate, wheelRoot);
            label.gameObject.SetActive(true);
            label.text = customerNames[i];

            float angleRad = i * segmentAngle * Mathf.Deg2Rad;
            label.rectTransform.anchoredPosition =
                new Vector2(Mathf.Sin(angleRad), Mathf.Cos(angleRad)) * labelRadius;
        }
    }

    public void Spin(Action<int, string> onComplete)
    {
        if (_spinning)
            return;

        _spinning = true;

        int winningIndex = UnityEngine.Random.Range(0, customerNames.Length);
        float segmentAngle = 360f / customerNames.Length;
        float startAngle = wheelRoot.localEulerAngles.z;
        float totalRotation = 360f * minFullSpins + winningIndex * segmentAngle;

        StartCoroutine(SpinRoutine(startAngle, totalRotation, winningIndex, onComplete));
    }

    private IEnumerator SpinRoutine(float startAngle, float totalRotation, int winningIndex, Action<int, string> onComplete)
    {
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / spinDuration);
            float eased = 1f - Mathf.Pow(1f - t, 3f);
            wheelRoot.localRotation = Quaternion.Euler(0f, 0f, startAngle + totalRotation * eased);
            yield return null;
        }

        wheelRoot.localRotation = Quaternion.Euler(0f, 0f, startAngle + totalRotation);
        _spinning = false;
        onComplete?.Invoke(winningIndex, customerNames[winningIndex]);
    }
}
