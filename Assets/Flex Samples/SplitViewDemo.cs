using UnityEngine;
using UnityEngine.UIElements;

public class SplitViewDemo : MonoBehaviour
{
    private VisualElement root;

    [SerializeField] private StyleSheet style;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        if (style != null) root.styleSheets.Add(style);

        var horizontalSplit = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        {

            horizontalSplit.style.flexGrow = 1;
            horizontalSplit.AddToClassList("my-split");
            root.Add(horizontalSplit);
        }

        var leftPanel = CreatePanel("Left Panel", Color.gray);
        {
            horizontalSplit.Add(leftPanel);
        }
        
        var rightContainer = new VisualElement();
        {
            rightContainer.style.flexGrow = 1;
            horizontalSplit.Add(rightContainer);
        }

        var verticalSplit = new TwoPaneSplitView(0, 200, TwoPaneSplitViewOrientation.Vertical);
        {
            verticalSplit.AddToClassList("my-split");

            verticalSplit.style.flexGrow = 1;
            rightContainer.Add(verticalSplit);
        }

        var topPanel = CreatePanel("Top Panel", new Color(0.2f, 0.6f, 1f));
        {
            verticalSplit.Add(topPanel);

            var bottomPanel = CreatePanel("Bottom Panel", new Color(0.2f, 1f, 0.6f));
            verticalSplit.Add(bottomPanel);
        }
    }

    VisualElement CreatePanel(string title, Color color)
    {
        var panel = new VisualElement();
        panel.style.flexGrow = 1;
        panel.style.backgroundColor = color;
        panel.style.paddingLeft = 10;
        panel.style.paddingTop = 10;

        var label = new Label(title);
        label.style.unityFontStyleAndWeight = FontStyle.Bold;
        label.style.fontSize = 14;

        panel.Add(label);
        return panel;
    }
}
