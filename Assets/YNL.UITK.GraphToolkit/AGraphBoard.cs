using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AGraphBoard : VisualElement
{
    // =========================================================
    // GRID SETTINGS
    // =========================================================
    public int BaseCellSize = 100;
    public int MinorStep = 4;
    public float LineThickness = 1f;

    public Color MajorColor = new(0.55f, 0.55f, 0.55f, 1f);
    public Color MinorColor = new(0.35f, 0.35f, 0.35f, 1f);
    public Color BackgroundColor = new(0.15f, 0.15f, 0.15f, 1f);

    public bool ShowMinor = true;

    // =========================================================
    // VIEW TRANSFORM
    // =========================================================
    private Vector2 _panOffset;
    private Vector2 _origin;          // screen-space world origin
    private float _zoom = 1f;
    private const float _minZoom = 0.25f;
    private const float _maxZoom = 2.5f;

    // =========================================================
    // INPUT
    // =========================================================
    private bool _dragging;
    private Vector2 _dragStart;
    private Vector2 _panStart;

    // =========================================================
    // NODES
    // =========================================================
    private VisualElement nodeLayer;
    private readonly List<AGraphNode> _nodes = new();

    // =========================================================
    // CONSTRUCTOR
    // =========================================================
    public AGraphBoard()
    {
        style.flexGrow = 1;
        pickingMode = PickingMode.Position;

        // ----- NODE LAYER (WORLD SPACE) -----
        nodeLayer = new VisualElement
        {
            style =
            {
                position = Position.Absolute,
                left = 0,
                top = 0,
                right = 0,
                bottom = 0
            },
            pickingMode = PickingMode.Position
        };

        Add(nodeLayer);

        // ----- GRID -----
        generateVisualContent += OnGenerateVisualContent;

        // ----- INPUT -----
        RegisterCallback<PointerDownEvent>(OnPointerDown);
        RegisterCallback<PointerMoveEvent>(OnPointerMove);
        RegisterCallback<PointerUpEvent>(OnPointerUp);
        RegisterCallback<WheelEvent>(OnWheel);

        // ----- GEOMETRY (CENTER ORIGIN) -----
        RegisterCallback<GeometryChangedEvent>(evt =>
        {
            _origin = contentRect.size * 0.5f;
            ApplyViewTransform();
        });
    }

    // =========================================================
    // PAN
    // =========================================================
    private void OnPointerDown(PointerDownEvent evt)
    {
        if (evt.button != 0)
            return;

        _dragging = true;
        _dragStart = evt.localPosition;
        _panStart = _panOffset;

        evt.StopPropagation();
    }

    private void OnPointerMove(PointerMoveEvent evt)
    {
        if (!_dragging)
            return;

        Vector2 delta = new Vector2(evt.localPosition.x, evt.localPosition.y) - _dragStart;
        _panOffset = _panStart + delta;

        ApplyViewTransform();
        evt.StopPropagation();
    }

    private void OnPointerUp(PointerUpEvent evt)
    {
        if (evt.button != 0)
            return;

        _dragging = false;
        evt.StopPropagation();
    }

    // =========================================================
    // ZOOM (ABOUT MOUSE, CORRECT)
    // =========================================================
    private void OnWheel(WheelEvent evt)
    {
        float oldZoom = _zoom;
        float zoomFactor = evt.delta.y > 0 ? 0.9f : 1.1f;
        _zoom = Mathf.Clamp(_zoom * zoomFactor, _minZoom, _maxZoom);

        if (Mathf.Approximately(oldZoom, _zoom))
            return;

        Vector2 mouse = evt.localMousePosition;

        // World position under mouse BEFORE zoom
        Vector2 worldUnderMouse =
            (mouse - _origin - _panOffset) / oldZoom;

        // Recalculate pan so that world stays under mouse
        _panOffset =
            mouse - _origin - worldUnderMouse * _zoom;

        ApplyViewTransform();
        evt.StopPropagation();
    }

    // =========================================================
    // APPLY VIEW TRANSFORM
    // =========================================================
    private void ApplyViewTransform()
    {
        nodeLayer.style.transformOrigin = new TransformOrigin(Length.Percent(0), Length.Percent(0), 0);
        ;

        nodeLayer.style.scale =
            new Scale(Vector2.one * _zoom);

        nodeLayer.style.translate =
            new Translate(_origin.x + _panOffset.x,
                          _origin.y + _panOffset.y);

        MarkDirtyRepaint();
    }

    // =========================================================
    // NODES (WORLD SPACE)
    // =========================================================
    public void AddNode(AGraphNode node, Vector2 worldPosition)
    {
        node.WorldPosition = worldPosition;
        node.style.position = Position.Absolute;
        node.style.left = worldPosition.x;
        node.style.top = worldPosition.y;

        nodeLayer.Add(node);
        _nodes.Add(node);
    }

    public void RemoveNode(AGraphNode node)
    {
        if (_nodes.Remove(node))
            nodeLayer.Remove(node);
    }

    // =========================================================
    // DRAW GRID (LOCKED TO WORLD)
    // =========================================================
    private void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var painter = mgc.painter2D;
        var rect = contentRect;

        float scaledCellSize = BaseCellSize * _zoom;
        float minor = scaledCellSize / Mathf.Max(1, MinorStep);

        // ----- BACKGROUND -----
        painter.fillColor = BackgroundColor;
        painter.BeginPath();
        painter.MoveTo(rect.min);
        painter.LineTo(new(rect.xMax, rect.yMin));
        painter.LineTo(rect.max);
        painter.LineTo(new(rect.xMin, rect.yMax));
        painter.ClosePath();
        painter.Fill();

        float t = Mathf.InverseLerp(_minZoom, _maxZoom, _zoom);
        float minorAlpha = Mathf.Lerp(0.1f, 1f, t);
        float majorAlpha = Mathf.Lerp(0.3f, 1f, t);

        painter.lineCap = LineCap.Butt;

        Vector2 offset = _origin + _panOffset;

        // ----- MINOR -----
        if (ShowMinor)
        {
            var col = MinorColor;
            col.a *= minorAlpha;
            painter.strokeColor = col;
            painter.lineWidth = Mathf.Max(0.5f, LineThickness * 0.5f);

            float startX = rect.xMin + Mathf.Repeat(offset.x, minor);
            float startY = rect.yMin + Mathf.Repeat(offset.y, minor);

            for (float x = startX; x < rect.xMax; x += minor)
            {
                painter.BeginPath();
                painter.MoveTo(new(x, rect.yMin));
                painter.LineTo(new(x, rect.yMax));
                painter.Stroke();
            }

            for (float y = startY; y < rect.yMax; y += minor)
            {
                painter.BeginPath();
                painter.MoveTo(new(rect.xMin, y));
                painter.LineTo(new(rect.xMax, y));
                painter.Stroke();
            }
        }

        // ----- MAJOR -----
        var majorCol = MajorColor;
        majorCol.a *= majorAlpha;
        painter.strokeColor = majorCol;
        painter.lineWidth = LineThickness;

        float majorX = rect.xMin + Mathf.Repeat(offset.x, scaledCellSize);
        float majorY = rect.yMin + Mathf.Repeat(offset.y, scaledCellSize);

        for (float x = majorX; x < rect.xMax; x += scaledCellSize)
        {
            painter.BeginPath();
            painter.MoveTo(new(x, rect.yMin));
            painter.LineTo(new(x, rect.yMax));
            painter.Stroke();
        }

        for (float y = majorY; y < rect.yMax; y += scaledCellSize)
        {
            painter.BeginPath();
            painter.MoveTo(new(rect.xMin, y));
            painter.LineTo(new(rect.xMax, y));
            painter.Stroke();
        }
    }
}
