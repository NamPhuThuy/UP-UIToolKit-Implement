using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListViewDemo : MonoBehaviour
{
    private VisualElement root;
    private ListView listView;
    private Label infoLabel;
    private Button resizeButton;

    private List<string> items = new List<string>();

    private Dictionary<int, float> itemHeights = new Dictionary<int, float>();

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        for (int i = 0; i < 50; i++)
        {
            items.Add($"Inventory Item {i}");
            itemHeights[i] = 30f;
        }

        infoLabel = new Label("Select an item");
        infoLabel.style.marginLeft = 20;
        infoLabel.style.marginTop = 20;
        infoLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        root.Add(infoLabel);

        resizeButton = new Button(OnRandomResizeClicked)
        {
            text = "Random Resize Items"
        };
        resizeButton.style.marginLeft = 20;
        resizeButton.style.marginTop = 10;
        root.Add(resizeButton);

        listView = new ListView
        {
            itemsSource = items,
            selectionType = SelectionType.Single,

            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight
        };

        listView.style.marginLeft = 20;
        listView.style.marginTop = 10;
        listView.style.width = 300;
        listView.style.height = 300;

        listView.makeItem = () =>
        {
            var label = new Label();
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.paddingLeft = 8;
            label.style.whiteSpace = WhiteSpace.Normal;
            label.style.flexGrow = 1;
            label.style.borderBottomWidth = label.style.borderTopWidth = label.style.borderLeftWidth = label.style.borderRightWidth = 1;
            label.style.borderBottomColor = label.style.borderTopColor = label.style.borderLeftColor = label.style.borderRightColor = new Color(0.3f, 0.3f, 0.3f, 1);

            return label;
        };

        listView.bindItem = (element, index) =>
        {
            var label = (Label)element;
            label.text = items[index];

            label.style.height = itemHeights[index];
        };

        listView.selectionChanged += objects =>
        {
            foreach (var obj in objects)
            {
                infoLabel.text = $"Selected: {obj}";
            }
        };

        root.Add(listView);
    }

    void OnRandomResizeClicked()
    {
        for (int i = 0; i < items.Count; i++)
            itemHeights[i] = 30f;

        for (int i = 0; i < 5; i++)
        {
            int randomIndex = Random.Range(0, items.Count);
            itemHeights[randomIndex] = Random.Range(60f, 90f);
        }

        listView.RefreshItems();
    }
}
