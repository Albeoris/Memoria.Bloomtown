using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Memoria.Bloomtown.Core;

public sealed class TMPButton
{
    public GameObject GameObject { get; }
    public TextMeshProUGUI TextMeshPro { get; }
    public Button Button { get; }
    public Image Image { get; }

    public TMPButton(GameObject gameObject)
    {
        GameObject = gameObject ?? throw new ArgumentNullException(nameof(gameObject));
        TextMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>() ?? throw new ArgumentNullException(nameof(TextMeshPro));
        Button = gameObject.GetComponent<Button>() ?? throw new ArgumentNullException(nameof(Button));
        Image = gameObject.GetComponent<Image>() ?? throw new ArgumentNullException(nameof(Image));
    }
    
    public String Text
    {
        get => TextMeshPro.text;
        set => TextMeshPro.text = value;
    }
    
    public event Action Click
    {
        add => Button.onClick.AddListener(value);
        remove => Button.onClick.RemoveListener(value);
    }
}