using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIYAxisGenerator : MonoBehaviour
{
    [Header("UI 參考")]
    public RectTransform chartRect;
    public List<GameObject> GridLineObjs;

    [Header("刻度設定")]
    public int GridCount = 6;

    [Header("Padding")]
    public float topPadding = 32f;
    public float bottomPadding = 28f;

    public float MaxScore;

    private void OnValidate()
    {
        Generate();
    }

    public void Generate()
    {
        float height = chartRect.rect.height - topPadding - bottomPadding;

        GridCount = Mathf.Clamp(GridCount, 0, GridLineObjs.Count - 1);

        GridLineObjs.ForEach(x => x.SetActive(false));
        for (int i = 0; i <= GridCount; i++)
        {
            float t = (float)i / GridCount;
            float y = bottomPadding + t * height;

            float value = t * MaxScore;
            GridLineObjs[i].SetActive(true);
            RectTransform lr = GridLineObjs[i].GetComponent<RectTransform>();
            lr.pivot = new Vector2(0.5f, 0);
            lr.anchorMin = new Vector2(0, 0);
            lr.anchorMax = new Vector2(1, 0);
            lr.anchoredPosition = new Vector2(0, y);
            GridLineObjs[i].GetComponentInChildren<Text>().text = Mathf.Round(value).ToString();
        }
    }

    public void SetData(List<GameObject> objList, float maxScore, int gridCount)
    {
        GridLineObjs = objList;
        GridCount = gridCount;
        MaxScore = maxScore;
        Generate();
    }
}
