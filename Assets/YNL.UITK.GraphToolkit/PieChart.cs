using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

// -----------------------------
// PieChart VisualElement
// -----------------------------
public class PieChart : VisualElement
{
    public struct Slice
    {
        public float value;
        public Color color;
        public string label;

        public Slice(float value, Color color, string label = null)
        {
            this.value = value;
            this.color = color;
            this.label = label;
        }
    }

    private float _zoom = 1f;
    private float _minZoom = 0.25f;
    private float _maxZoom = 2.5f;

    private readonly List<Slice> _slices = new();
    private float _total;
    private float _padding = 4f;

    public float Padding
    {
        get => _padding;
        set { _padding = value; MarkDirtyRepaint(); }
    }

    public PieChart()
    {
        generateVisualContent += OnGenerateVisualContent;
        style.flexGrow = 1;

        RegisterCallback<WheelEvent>(OnWheel);

        this.Add(new Label("Hello world"));
    }

    private void OnWheel(WheelEvent evt)
    {
        float oldZoom = _zoom;

        _zoom *= evt.delta.y > 0 ? 0.9f : 1.1f;
        _zoom = Mathf.Clamp(_zoom, _minZoom, _maxZoom);

        if (Mathf.Approximately(oldZoom, _zoom)) return;

        this.style.scale = new Scale(Vector2.one * _zoom);

        MarkDirtyRepaint();
        evt.StopPropagation();
    }

    public void SetData(IEnumerable<Slice> slices)
    {
        _slices.Clear();
        _total = 0f;

        foreach (var s in slices)
        {
            if (s.value <= 0f) continue;
            _slices.Add(s);
            _total += s.value;
        }

        MarkDirtyRepaint();
    }

    private void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        if (_slices.Count == 0 || _total <= 0f)
            return;

        var painter = ctx.painter2D;

        Rect r = contentRect;
        float size = Mathf.Min(r.width, r.height) - _padding * 2f;
        if (size <= 0f) return;

        Vector2 center = r.center;
        float radius = size * 0.5f;

        float startAngle = -90f; // start at top

        foreach (var slice in _slices)
        {
            float angle = slice.value / _total * 360f;
            DrawSlice(painter, center, radius, startAngle, angle, slice.color);
            startAngle += angle;
        }
    }

    private static void DrawSlice(
        Painter2D painter,
        Vector2 center,
        float radius,
        float startAngleDeg,
        float sweepAngleDeg,
        Color color)
    {
        int segments = Mathf.Max(3, Mathf.CeilToInt(sweepAngleDeg / 6f));
        float step = sweepAngleDeg / segments;

        painter.BeginPath();
        painter.MoveTo(center);

        for (int i = 0; i <= segments; i++)
        {
            float a = (startAngleDeg + step * i) * Mathf.Deg2Rad;
            Vector2 p = center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius;
            painter.LineTo(p);
        }

        painter.ClosePath();
        painter.fillColor = color;
        painter.Fill();
    }
}
