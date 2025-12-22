using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Painter2DExample : MonoBehaviour
{
    public void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        doc.rootVisualElement.generateVisualContent += Draw;
    }

    void Draw(MeshGenerationContext ctx)
    {
        var painter = ctx.painter2D;
        painter.lineWidth = 10.0f;
        painter.lineCap = LineCap.Round;
        painter.strokeGradient = new Gradient()
        {
            colorKeys = new GradientColorKey[] {
                 new GradientColorKey() { color = Color.red, time = 0.0f },
                 new GradientColorKey() { color = Color.blue, time = 1.0f }
             }
        };
        painter.BeginPath();
        painter.MoveTo(new Vector2(10, 10));
        painter.BezierCurveTo(new Vector2(100, 100), new Vector2(200, 0), new Vector2(300, 100));
        painter.Stroke();
    }
}


class QuadMeshElement : VisualElement
{
    public Color color = Color.red;

    public QuadMeshElement()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var mesh = mgc.Allocate(4, 6);

        var p0 = Vector2.zero;
        var p1 = new Vector2(layout.width, 0);
        var p2 = new Vector2(layout.width, layout.height);
        var p3 = new Vector2(0, layout.height);

        mesh.SetNextVertex(new Vertex() { position = new Vector3(p0.x, p0.y, Vertex.nearZ), tint = color });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(p1.x, p1.y, Vertex.nearZ), tint = color });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(p2.x, p2.y, Vertex.nearZ), tint = color });
        mesh.SetNextVertex(new Vertex() { position = new Vector3(p3.x, p3.y, Vertex.nearZ), tint = color });

        mesh.SetNextIndex(0);
        mesh.SetNextIndex(1);
        mesh.SetNextIndex(2);
        mesh.SetNextIndex(0);
        mesh.SetNextIndex(2);
        mesh.SetNextIndex(3);
    }
}

class QuadVectorElement : VisualElement
{
    public Color color = Color.red;

    public QuadVectorElement()
    {
        generateVisualContent += OnGenerateVisualContent;
    }

    void OnGenerateVisualContent(MeshGenerationContext mgc)
    {
        var p0 = Vector2.zero;
        var p1 = new Vector2(layout.width, 0);
        var p2 = new Vector2(layout.width, layout.height);
        var p3 = new Vector2(0, layout.height);

        var painter2D = mgc.painter2D;

        painter2D.fillColor = color;

        painter2D.BeginPath();
        painter2D.MoveTo(p0);
        painter2D.LineTo(p1);
        painter2D.LineTo(p2);
        painter2D.LineTo(p3);
        painter2D.ClosePath();
        painter2D.Fill();
    }
}
