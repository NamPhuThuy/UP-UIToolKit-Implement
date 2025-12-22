using UnityEngine;
using UnityEngine.UIElements;

public class ChangeEventDemo : MonoBehaviour
{
    private VisualElement root;

    private Label infoLabel;
    private TextField nameField;
    private Toggle enableToggle;
    private Slider volumeSlider;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        // ===== Info Label =====
        infoLabel = new Label("ChangeEvent Demo");
        infoLabel.style.marginLeft = 20;
        infoLabel.style.marginTop = 20;
        infoLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
        infoLabel.style.color = Color.white;
        infoLabel.style.fontSize = 30;
        root.Add(infoLabel);

        // ===== TextField =====
        nameField = new TextField("Player Name");
        nameField.style.marginLeft = 20;
        nameField.style.marginTop = 20;
        nameField.style.color = Color.white;
        nameField.style.fontSize = 30;
        root.Add(nameField);

        // ===== Toggle =====
        enableToggle = new Toggle("Enable Feature");
        enableToggle.style.marginLeft = 20;
        enableToggle.style.marginTop = 10;
        enableToggle.style.color = Color.white;
        enableToggle.style.fontSize = 30;
        root.Add(enableToggle);

        // ===== Slider =====
        volumeSlider = new Slider("Volume", 0, 100);
        volumeSlider.style.marginLeft = 20;
        volumeSlider.style.marginTop = 10;
        volumeSlider.style.color = Color.white;
        volumeSlider.style.fontSize = 30;
        root.Add(volumeSlider);

        RegisterChangeEvents();
    }

    void RegisterChangeEvents()
    {
        nameField.RegisterCallback<ChangeEvent<string>>(evt =>
        {
            Debug.Log($"TextField ChangeEvent: {evt.previousValue} -> {evt.newValue}");
            infoLabel.text = $"Name changed to: {evt.newValue}";
        });

        enableToggle.RegisterCallback<ChangeEvent<bool>>(evt =>
        {
            Debug.Log($"Toggle ChangeEvent: {evt.previousValue} -> {evt.newValue}");
            infoLabel.text = $"Feature enabled: {evt.newValue}";
        });

        volumeSlider.RegisterCallback<ChangeEvent<float>>(evt =>
        {
            Debug.Log($"Slider ChangeEvent: {evt.previousValue} -> {evt.newValue}");
            infoLabel.text = $"Volume: {evt.newValue:0}";
        });
    }
}
