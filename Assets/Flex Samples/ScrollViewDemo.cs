using UnityEngine;
using UnityEngine.UIElements;

public class ScrollViewDemo : MonoBehaviour
{
    private VisualElement root;
    private ScrollView scrollView;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        scrollView = new ScrollView(ScrollViewMode.Vertical);
        scrollView.style.width = 400;
        scrollView.style.height = 350;
        scrollView.style.marginLeft = 20;
        scrollView.style.marginTop = 20;
        scrollView.style.borderTopWidth = 2;
        scrollView.style.borderBottomWidth = 2;
        scrollView.style.borderLeftWidth = 2;
        scrollView.style.borderRightWidth = 2;

        root.Add(scrollView);

        AddLabel("ScrollView Demo", 20, FontStyle.Bold);
        AddBox(300, 80, Color.cyan);
        AddToggle("Enable Shadows");
        AddBox(200, 120, Color.green);
        AddSlider("Volume", 0, 100);
        AddTextField("Player Name");
        AddBox(350, 60, Color.magenta);
        AddLabel("End of Content", 14, FontStyle.Italic);
    }

    void AddLabel(string text, int fontSize, FontStyle style)
    {
        Label label = new Label(text);
        label.style.fontSize = fontSize;
        label.style.unityFontStyleAndWeight = style;
        label.style.marginBottom = 10;
        scrollView.Add(label);
    }

    void AddBox(float width, float height, Color color)
    {
        VisualElement box = new VisualElement();
        box.style.width = width;
        box.style.height = height;
        box.style.backgroundColor = color;
        box.style.marginBottom = 10;
        scrollView.Add(box);
    }

    void AddToggle(string text)
    {
        Toggle toggle = new Toggle(text);
        toggle.style.marginBottom = 10;
        scrollView.Add(toggle);
    }

    void AddSlider(string label, float min, float max)
    {
        Slider slider = new Slider(label, min, max);
        slider.style.marginBottom = 10;
        scrollView.Add(slider);
    }

    void AddTextField(string label)
    {
        TextField field = new TextField(label);
        field.style.marginBottom = 10;
        scrollView.Add(field);
    }
}
