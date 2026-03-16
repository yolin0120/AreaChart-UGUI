using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGradientChart : MaskableGraphic
{
    [Header("資料")]
    public List<float> values = new List<float>()
    {
        10, 30, 180, 120, 150, 130
    };

    [Header("圖表設定")]
    public float maxValue = 200f;

    public RectOffset Padding;

    [Header("Gradient Quality")]
    [Range(1, 100)]
    public int verticalSubdivisions = 4;

    [Header("顏色")]
    public Color topColor = new Color(0.2f, 0.6f, 1f, 0.7f);
    public Color bottomColor = new Color(0.2f, 0.6f, 1f, 0f);

    [NonSerialized]
    public float range = 1f;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (values == null || values.Count < 2)
            return;

        Rect rect = GetPixelAdjustedRect();
        float usableWidth = rect.width - Padding.left - Padding.right;
        float usableHeight = rect.height - Padding.top - Padding.bottom;
        if (usableWidth <= 0 || usableHeight <= 0)
            return;

        float width = rect.width;
        float height = rect.height;

        int count = values.Count;
        float xStep = usableWidth / (count - 1);

        for (int i = 0; i < count - 1; i++)
        {
            float x1 = rect.xMin + Padding.left + xStep * i;
            float x2 = rect.xMin + Padding.left + xStep * (i + 1);

            float yTop1 = rect.yMin + Padding.bottom + (values[i] / maxValue) * usableHeight * range;
            float yTop2 = rect.yMin + Padding.bottom + (values[i + 1] / maxValue) * usableHeight * range;

            float yBottom = rect.yMin + Padding.bottom;

            for (int s = 0; s < verticalSubdivisions; s++)
            {
                float t0 = (float)s / verticalSubdivisions;
                float t1 = (float)(s + 1) / verticalSubdivisions;

                float y1a = Mathf.Lerp(yTop1, yBottom, t0);
                float y1b = Mathf.Lerp(yTop1, yBottom, t1);

                float y2a = Mathf.Lerp(yTop2, yBottom, t0);
                float y2b = Mathf.Lerp(yTop2, yBottom, t1);

                Color cTop = Color.Lerp(topColor, bottomColor, t0);
                Color cBottom = Color.Lerp(topColor, bottomColor, t1);

                int idx = vh.currentVertCount;

                vh.AddVert(new Vector2(x1, y1a), cTop, Vector2.zero);
                vh.AddVert(new Vector2(x2, y2a), cTop, Vector2.zero);
                vh.AddVert(new Vector2(x2, y2b), cBottom, Vector2.zero);
                vh.AddVert(new Vector2(x1, y1b), cBottom, Vector2.zero);

                vh.AddTriangle(idx, idx + 1, idx + 2);
                vh.AddTriangle(idx, idx + 2, idx + 3);
            }
        }
    }

    public void Init()
    {
        values = new List<float>();
        this.maxValue = 90;
        SetVerticesDirty();
    }

    /// <summary>
    /// 外部更新資料用
    /// </summary>
    public void SetData(RectOffset padding, List<float> newValues, float maxValue)
    {
        Padding = padding;
        values = newValues;
        this.maxValue = maxValue;
        SetVerticesDirty();
    }

    public void SetColor(Color topColor, Color bottomColor)
    {
        this.topColor = topColor;
        this.bottomColor = bottomColor;
    }
}
