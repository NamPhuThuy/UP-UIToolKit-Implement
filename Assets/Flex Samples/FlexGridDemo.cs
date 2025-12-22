using UnityEngine;
using UnityEngine.UIElements;

public class FlexGridDemo : MonoBehaviour
{
    private VisualElement root;
    private VisualElement grid;

    [SerializeField] private int gridWidth = 400;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        grid = new VisualElement();
        grid.name = "GridContainer";

        grid.style.flexDirection = FlexDirection.Row;
        grid.style.flexWrap = Wrap.Wrap;
        grid.style.alignContent = Align.FlexStart;

        grid.style.width = gridWidth;
        grid.style.height = 300;
        grid.style.marginLeft = 20;
        grid.style.marginTop = 20;

        grid.style.borderTopWidth = 2;
        grid.style.borderBottomWidth = 2;
        grid.style.borderLeftWidth = 2;
        grid.style.borderRightWidth = 2;

        grid.style.backgroundColor = Color.white;

        root.Add(grid);

        for (int i = 0; i < 20; i++)
        {
            grid.Add(CreateItem(i));
        }
    }

    void Update()
    {
        if (grid != null)
            grid.style.width = gridWidth;
    }

    VisualElement CreateItem(int index)
    {
        VisualElement item = new VisualElement();
        item.name = $"GridItem_{index}";

        item.style.width = 80;
        item.style.height = 80;
        item.style.marginRight = 10;
        item.style.marginBottom = 10;
        item.style.backgroundColor = new Color(0.4f, 0.5f, 0.6f, 1);// Random.ColorHSV(0, 1, 0.6f, 1, 0.6f, 1);

        Label label = new Label(index.ToString());
        label.style.unityTextAlign = TextAnchor.MiddleCenter;
        label.style.flexGrow = 1;

        item.Add(label);

        item.RegisterCallback<PointerDownEvent>(_ =>
        {
            Debug.Log($"Clicked {item.name}");
        });

        return item;
    }
}
    