using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class PieChartPainterDemo : MonoBehaviour
{
    private VisualElement chartElement;

    private List<float> values = new List<float> { 40, 25, 20, 15 };
    private List<Color> colors = new List<Color>
    {
        new Color(0.9f, 0.3f, 0.3f),
        new Color(0.3f, 0.7f, 0.9f),
        new Color(0.3f, 0.9f, 0.5f),
        new Color(0.9f, 0.8f, 0.3f)
    };

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        chartElement = new VisualElement();
        chartElement.style.width = 300;
        chartElement.style.height = 300;
        chartElement.style.marginLeft = 100;
        chartElement.style.marginTop = 100;

        chartElement.generateVisualContent += DrawPieChart;

        root.Add(chartElement);
    }

    void DrawPieChart(MeshGenerationContext ctx)
    {
        var painter = ctx.painter2D;

        Vector2 center = new Vector2(150, 150);
        float radius = 120f;

        float total = 0f;
        foreach (var v in values)
        {
            total += v;
        }

        float startAngle = -Mathf.PI / 2f;

        for (int i = 0; i < values.Count; i++)
        {
            float sliceAngle = (values[i] / total) * Mathf.PI * 2f;
            float endAngle = startAngle + sliceAngle;

            DrawSlice(
                painter,
                center,
                radius,
                startAngle,
                endAngle,
                colors[i]
            );

            startAngle = endAngle;
        }
    }

    void DrawSlice(
        Painter2D painter,
        Vector2 center,
        float radius,
        float startAngle,
        float endAngle,
        Color color)
    {
        int segments = 40;
        painter.fillColor = color;

        painter.BeginPath();
        painter.MoveTo(center);

        for (int i = 0; i <= segments; i++)
        {
            float t = Mathf.Lerp(startAngle, endAngle, i / (float)segments);
            Vector2 point = new Vector2(
                center.x + Mathf.Cos(t) * radius,
                center.y + Mathf.Sin(t) * radius
            );

            painter.LineTo(point);
        }

        painter.ClosePath();
        painter.Fill();
    }
}
