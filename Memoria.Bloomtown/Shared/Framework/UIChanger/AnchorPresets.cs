using System;
using UnityEngine;

namespace Memoria.Bloomtown.Shared.Framework.UIChanger;

/// <summary>
/// Provides methods for setting anchor presets on RectTransform.
/// </summary>
public static class AnchorPresets
{
    /// <summary>
    /// Sets the anchor preset on the specified RectTransform.
    /// </summary>
    /// <param name="rectTransform">The RectTransform to set the anchor on.</param>
    /// <param name="preset">The anchor preset to apply.</param>
    /// <param name="keepCurrentRect">
    /// If true, the current RectTransform's size and local position will be preserved.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified anchor preset is not supported.
    /// </exception>
    public static void SetAnchors(this RectTransform rectTransform, AnchorPreset preset, Boolean keepCurrentRect)
    {
        GetAnchors(preset, out Vector2 anchorMin, out Vector2 anchorMax);
        SetAnchors(rectTransform, anchorMin, anchorMax, keepCurrentRect);
    }

    /// <summary>
    /// Sets the anchor positions on the specified RectTransform.
    /// </summary>
    /// <param name="rectTransform">The RectTransform to set the anchors on.</param>
    /// <param name="anchorMin">The minimum anchor position.</param>
    /// <param name="anchorMax">The maximum anchor position.</param>
    /// <param name="keepCurrentRect">
    /// If true, the current RectTransform's size and local position will be preserved.
    /// </param>
    public static void SetAnchors(this RectTransform rectTransform, Vector2 anchorMin, Vector2 anchorMax, Boolean keepCurrentRect)
    {
        if (!keepCurrentRect)
        {
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            return;
        }

        Vector3 tmpLocalPosition = rectTransform.localPosition;
        Vector2 tmpSize = rectTransform.sizeDelta;

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tmpSize.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tmpSize.y);
        rectTransform.localPosition = tmpLocalPosition;
    }

    /// <summary>
    /// Gets the anchor positions for the specified AnchorPreset.
    /// </summary>
    /// <param name="preset">The anchor preset to get the anchor positions for.</param>
    /// <param name="anchorMin">The minimum anchor position.</param>
    /// <param name="anchorMax">The maximum anchor position.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified anchor preset is not supported.
    /// </exception>
    public static void GetAnchors(this AnchorPreset preset, out Vector2 anchorMin, out Vector2 anchorMax)
    {
        switch (preset)
        {
            case AnchorPreset.TopLeft:
                anchorMin = new Vector2(0, 1);
                anchorMax = new Vector2(0, 1);
                break;
            case AnchorPreset.TopCenter:
                anchorMin = new Vector2(0.5f, 1);
                anchorMax = new Vector2(0.5f, 1);
                break;
            case AnchorPreset.TopRight:
                anchorMin = new Vector2(1, 1);
                anchorMax = new Vector2(1, 1);
                break;
            case AnchorPreset.MiddleLeft:
                anchorMin = new Vector2(0, 0.5f);
                anchorMax = new Vector2(0, 0.5f);
                break;
            case AnchorPreset.MiddleCenter:
                anchorMin = new Vector2(0.5f, 0.5f);
                anchorMax = new Vector2(0.5f, 0.5f);
                break;
            case AnchorPreset.MiddleRight:
                anchorMin = new Vector2(1, 0.5f);
                anchorMax = new Vector2(1, 0.5f);
                break;
            case AnchorPreset.BottomLeft:
                anchorMin = new Vector2(0, 0);
                anchorMax = new Vector2(0, 0);
                break;
            case AnchorPreset.BottomCenter:
                anchorMin = new Vector2(0.5f, 0);
                anchorMax = new Vector2(0.5f, 0);
                break;
            case AnchorPreset.BottomRight:
                anchorMin = new Vector2(1, 0);
                anchorMax = new Vector2(1, 0);
                break;
            case AnchorPreset.StretchAll:
                anchorMin = new Vector2(0, 0);
                anchorMax = new Vector2(1, 1);
                break;
            case AnchorPreset.StretchTop:
                anchorMin = new Vector2(0, 1);
                anchorMax = new Vector2(1, 1);
                break;
            case AnchorPreset.StretchMiddle:
                anchorMin = new Vector2(0, 0.5f);
                anchorMax = new Vector2(1, 0.5f);
                break;
            case AnchorPreset.StretchBottom:
                anchorMin = new Vector2(0, 0);
                anchorMax = new Vector2(1, 0);
                break;
            case AnchorPreset.StretchLeft:
                anchorMin = new Vector2(0, 0);
                anchorMax = new Vector2(0, 1);
                break;
            case AnchorPreset.StretchCenter:
                anchorMin = new Vector2(0.5f, 0);
                anchorMax = new Vector2(0.5f, 1);
                break;
            case AnchorPreset.StretchRight:
                anchorMin = new Vector2(1, 0);
                anchorMax = new Vector2(1, 1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(preset), preset, $"The specified AnchorPreset [{preset}] is not supported.");
        }
    }

    public static void ResizeAndMoveParentToChild(RectTransform childRectTransform)
    {
        RectTransform parentRectTransform = (RectTransform)childRectTransform.parent;

        Vector3 positionDelta = childRectTransform.localPosition;
        Vector2 sizeDelta = childRectTransform.sizeDelta;
        
        childRectTransform.offsetMin = Vector2.zero;
        childRectTransform.offsetMax = Vector2.zero;
        
        parentRectTransform.localPosition += positionDelta;
        parentRectTransform.sizeDelta += sizeDelta;
    }
}