using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    // ============ VARIABLES ============
    [Header("UI References")]
    [SerializeField]
    [Tooltip("The TextMeshPro element representing the selection arrow next to the button label.")]
    private TextMeshProUGUI arrowText;

    [SerializeField]
    [Tooltip("The TextMeshPro element representing the actual text label of the button.")]
    private TextMeshProUGUI labelText;

    [Header("Color Settings")]
    [SerializeField]
    [Tooltip("The color applied to the label when the button is currently hovered or selected.")]
    private Color activeColor = new Color(0.94f, 0.93f, 0.90f);

    [SerializeField]
    [Tooltip("The color applied to the label when the button is not being interacted with.")]
    private Color inactiveColor = new Color(0.53f, 0.53f, 0.53f);

    public bool IsSelected { get; private set; }

    // ============ UNITY EVENTS ============
    private void Awake()
    {
        SetInactiveState();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetActiveState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetInactiveState();
    }

    public void OnSelect(BaseEventData eventData)
    {
        SetActiveState();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        SetInactiveState();
    }

    // ============ LOGIC ============
    private void SetActiveState()
    {
        IsSelected = true;

        if (arrowText != null)
        {
            arrowText.alpha = 1f;
        }

        if (labelText != null)
        {
            labelText.color = activeColor;
        }
    }

    private void SetInactiveState()
    {
        IsSelected = false;

        if (arrowText != null)
        {
            arrowText.alpha = 0f;
        }

        if (labelText != null)
        {
            labelText.color = inactiveColor;
        }
    }
}