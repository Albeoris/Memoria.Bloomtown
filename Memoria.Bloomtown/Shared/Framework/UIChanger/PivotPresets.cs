using System;
using UnityEngine;

namespace Memoria.Bloomtown.Shared.Framework.UIChanger;

/// <summary>
/// Provides methods for setting pivot presets on RectTransform.
/// </summary>
public static class PivotPresets
{
    /// <summary>
    /// Sets the pivot preset on the specified RectTransform.
    /// </summary>
    /// <param name="rectTransform">The RectTransform to set the pivot on.</param>
    /// <param name="pivotPreset">The pivot preset to apply.</param>
    /// <param name="keepCurrentRect">
    /// If true, the current RectTransform's size and local position will be preserved.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified pivot preset is not supported.
    /// </exception>
    public static void SetPivot(this RectTransform rectTransform, PivotPreset pivotPreset, Boolean keepCurrentRect)
    {
        Vector2 pivot = GetPivot(pivotPreset);
        SetPivot(rectTransform, pivot, keepCurrentRect);
    }

    /// <summary>
    /// Sets the pivot position on the specified RectTransform.
    /// </summary>
    /// <param name="rectTransform">The RectTransform to set the pivot on.</param>
    /// <param name="pivot">The pivot position.</param>
    /// <param name="keepCurrentRect">
    /// If true, the current RectTransform's size and local position will be preserved.
    /// </param>
    public static void SetPivot(this RectTransform rectTransform, Vector2 pivot, Boolean keepCurrentRect)
    {
        if (!keepCurrentRect)
        {
            rectTransform.pivot = pivot;
            return;
        }

        Vector3 tmpLocalPosition = rectTransform.localPosition;
        Vector2 tmpSize = rectTransform.sizeDelta;

        Vector2 deltaPivot = rectTransform.pivot - pivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * tmpSize.x, deltaPivot.y * tmpSize.y);

        rectTransform.pivot = pivot;

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tmpSize.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tmpSize.y);
        rectTransform.localPosition = tmpLocalPosition - deltaPosition;
    }

    /// <summary>
    /// Gets the pivot position for the specified PivotPreset.
    /// </summary>
    /// <param name="preset">The pivot preset to get the pivot position for.</param>
    /// <returns>The pivot position.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the specified pivot preset is not supported.
    /// </exception>
    public static Vector2 GetPivot(this PivotPreset preset)
    {
        switch (preset)
        {
            case PivotPreset.TopLeft:
                return new Vector2(0, 1);
            case PivotPreset.TopCenter:
                return new Vector2(0.5f, 1);
            case PivotPreset.TopRight:
                return new Vector2(1, 1);
            case PivotPreset.MiddleLeft:
                return new Vector2(0, 0.5f);
            case PivotPreset.MiddleCenter:
                return new Vector2(0.5f, 0.5f);
            case PivotPreset.MiddleRight:
                return new Vector2(1, 0.5f);
            case PivotPreset.BottomLeft:
                return new Vector2(0, 0);
            case PivotPreset.BottomCenter:
                return new Vector2(0.5f, 0);
            case PivotPreset.BottomRight:
                return new Vector2(1, 0);
            default:
                throw new ArgumentOutOfRangeException(nameof(preset), preset, $"The specified PivotPreset [{preset}] is not supported.");
        }
    }
}