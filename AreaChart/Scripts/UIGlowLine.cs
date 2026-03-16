using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGlowLine : MaskableGraphic
{
    [Header("資料")]
    public List<float> values = new List<float>()
    {
        10, 30, 180, 120, 150, 130
    };

    [Header("圖表設定")]
    public float maxValue = 200f;
    public float lineWidth = 3f;

    [Header("Glow 強度")]
    public int glowPass = 6;
    public float glowWidthStep = 2.5f;    // 每層加寬
    public float glowIntensity = 1.8f;    // 亮度倍率

    public RectOffset Padding;

    [Header("顏色")]
    public Color lineColor = Color.white;
    public Color glowColor = new Color(0.3f, 0.7f, 1f, 0.4f);

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

        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < count; i++)
        {
            float x = rect.xMin + Padding.left + xStep * i;
            float y = rect.yMin + Padding.bottom + (values[i] / maxValue) * usableHeight * range;
            points.Add(new Vector2(x, y));
        }

        // Glow（外圈）
        for (int g = glowPass; g > 0; g--)
        {
            float gwidth = lineWidth + g * glowWidthStep;

            float t = (float)g / glowPass;
            float alphaMul = Mathf.Pow(1f - t, 2f) * glowIntensity;

            Color c = new Color(glowColor.r, glowColor.g, glowColor.b, glowColor.a * alphaMul);
            DrawLine(vh, points, gwidth, c);
        }

        // 主線
        DrawLine(vh, points, lineWidth, lineColor);
        DrawLine(vh, points, lineWidth * 0.9f, lineColor);
    }

    void DrawLine(VertexHelper vh, List<Vector2> points, float width, Color color)
    {
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 p1 = points[i];
            Vector2 p2 = points[i + 1];

            Vector2 dir = (p2 - p1).normalized;
            Vector2 normal = new Vector2(-dir.y, dir.x) * width * 0.5f;

            int idx = vh.currentVertCount;

            vh.AddVert(p1 + normal, color, Vector2.zero);
            vh.AddVert(p1 - normal, color, Vector2.zero);
            vh.AddVert(p2 - normal, color, Vector2.zero);
            vh.AddVert(p2 + normal, color, Vector2.zero);

            vh.AddTriangle(idx, idx + 1, idx + 2);
            vh.AddTriangle(idx, idx + 2, idx + 3);
        }
    }

    public void Init()
    {
        values = new List<float>();
        this.maxValue = 90;
        SetVerticesDirty();
    }

    public void SetData(RectOffset padding, List<float> newValues, float maxScore)
    {
        Padding = padding;
        values = newValues;
        this.maxValue = maxScore;
        SetVerticesDirty();
    }

    public void SetColor(Color lineColor, Color glowColor, Color uIFX_GlowHDRColor)
    {
        this.lineColor = lineColor;
        this.glowColor = glowColor; 
    }
}
