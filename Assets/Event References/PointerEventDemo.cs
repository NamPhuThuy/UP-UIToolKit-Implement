using UnityEngine;
using UnityEngine.UIElements;

public class PointerEventDemo : MonoBehaviour
{
    private VisualElement root;
    private VisualElement box;
    private VisualElement outerBox;
    private VisualElement moveBox;

    private Label downLabel;
    private Label upLabel;

    private int downCount;
    private int upCount;

    private bool isDragging;
    private Vector2 dragOffset;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // ===== Counter Labels =====
        downLabel = new Label("PointerDown: 0");
        upLabel = new Label("PointerUp: 0");

        downLabel.style.marginLeft = 20;
        downLabel.style.marginTop = 20;
        downLabel.style.color = Color.white;
        downLabel.style.fontSize = 50;

        upLabel.style.marginLeft = 20;
        upLabel.style.marginTop = 5;
        upLabel.style.color = Color.white;
        upLabel.style.fontSize = 50;

        root.Add(downLabel);
        root.Add(upLabel);

        // ===== Main Box =====
        box = new VisualElement();
        box.name = "PointerBox";
        box.style.width = 200;
        box.style.height = 200;
        box.style.backgroundColor = new Color(0.2f, 0.6f, 1f);
        box.style.marginTop = 100;
        box.style.marginLeft = 100;
        root.Add(box);

        // ===== Inner Box (Enter vs Over demo) =====
        outerBox = new VisualElement();
        outerBox.name = "PointerOuterBox";
        outerBox.style.width = 150;
        outerBox.style.height = 150;
        outerBox.style.backgroundColor = new Color(0.6f, 0.2f, 1f);
        outerBox.style.marginTop = 25;
        outerBox.style.marginLeft = 25;
        box.Add(outerBox);

        // ===== Move Box =====
        moveBox = new VisualElement();
        moveBox.name = "MoveBox";
        moveBox.style.width = 100;
        moveBox.style.height = 100;
        moveBox.style.backgroundColor = Color.red;
        moveBox.style.position = Position.Absolute;
        moveBox.style.left = 400;
        moveBox.style.top = 200;
        root.Add(moveBox);

        RegisterPointerEvents();
        RegisterMoveBoxEvents();
    }

    void RegisterPointerEvents()
    {
        box.RegisterCallback<PointerDownEvent>(evt =>
        {
            downCount++;
            downLabel.text = $"PointerDown: {downCount}";
            Debug.Log("PointerDownEvent");
        });

        box.RegisterCallback<PointerUpEvent>(evt =>
        {
            upCount++;
            upLabel.text = $"PointerUp: {upCount}";
            Debug.Log("PointerUpEvent");
        });

        box.RegisterCallback<PointerMoveEvent>(evt =>
        {
            Debug.Log("PointerMoveEvent (Box)");
        });

        box.RegisterCallback<PointerEnterEvent>(evt =>
        {
            Debug.Log("PointerEnterEvent");
            box.style.backgroundColor = Color.green;
        });

        box.RegisterCallback<PointerLeaveEvent>(evt =>
        {
            Debug.Log("PointerLeaveEvent");
            box.style.backgroundColor = new Color(0.2f, 0.6f, 1f);
        });

        box.RegisterCallback<PointerOverEvent>(evt =>
        {
            Debug.Log("PointerOverEvent");
        });

        box.RegisterCallback<PointerOutEvent>(evt =>
        {
            Debug.Log("PointerOutEvent");
        });
    }

    void RegisterMoveBoxEvents()
    {
        moveBox.RegisterCallback<PointerDownEvent>(evt =>
        {
            isDragging = true;
            dragOffset = new Vector2(evt.position.x, evt.position.y) -
                         new Vector2(moveBox.resolvedStyle.left, moveBox.resolvedStyle.top);
        });

        moveBox.RegisterCallback<PointerMoveEvent>(evt =>
        {
            if (!isDragging)
                return;

            moveBox.style.left = evt.position.x - dragOffset.x;
            moveBox.style.top = evt.position.y - dragOffset.y;
        });

        moveBox.RegisterCallback<PointerUpEvent>(evt =>
        {
            isDragging = false;
        });
    }
}
