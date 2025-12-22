using UnityEngine;
using UnityEngine.UIElements;

public class StarPainterDemo : MonoBehaviour
{
    private VisualElement starElement;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        starElement = new VisualElement();
        starElement.style.width = 300;
        starElement.style.height = 300;
        starElement.style.marginLeft = 100;
        starElement.style.marginTop = 100;

        starElement.generateVisualContent += OnGenerateVisualContent;

        root.Add(starElement);
    }

    void OnGenerateVisualContent(MeshGenerationContext ctx)
    {
        var painter = ctx.painter2D;

        Vector2 center = new Vector2(150, 150);
        float outerRadius = 120f;
        float innerRadius = 50f;
        int points = 5;

        painter.fillColor = new Color(1f, 0.8f, 0.1f);
        painter.strokeColor = Color.black;
        painter.lineWidth = 3f;

        painter.BeginPath();

        for (int i = 0; i < points * 2; i++)
        {
            float angle = i * Mathf.PI / points - Mathf.PI / 2;
            float radius = (i % 2 == 0) ? outerRadius : innerRadius;

            Vector2 pos = new Vector2(center.x + Mathf.Cos(angle) * radius,center.y + Mathf.Sin(angle) * radius);

            if (i == 0)
            {
                painter.MoveTo(pos);
            }
            else
            {
                painter.LineTo(pos);
            }
        }

        painter.ClosePath();
        painter.Fill();
        painter.Stroke();
    }
}
