using UnityEngine;
using UnityEngine.UIElements;

public class AGraphNode : VisualElement
{
    // Logical world position (in world coordinates you use for layout)
    public Vector2 WorldPosition { get; set; }

    // base size in world space (this will be scaled by zoom)
    public float BaseWidth { get; set; } = 120f;
    public float BaseHeight { get; set; } = 60f;

    public AGraphNode()
    {
        // absolute positioning so we can set left/top directly
        style.position = Position.Absolute;

        // default visual
        style.width = BaseWidth;
        style.height = BaseHeight;
        style.backgroundColor = new Color(0.2f, 0.4f, 0.7f, 1f);
        style.borderTopLeftRadius = 6;
        style.borderTopRightRadius = 6;
        style.borderBottomLeftRadius = 6;
        style.borderBottomRightRadius = 6;

        // allow events on the node
        pickingMode = PickingMode.Position;

        this.Add(new Label("Hello world"));
        this.Add(new TextField("Input"));

    }
}
