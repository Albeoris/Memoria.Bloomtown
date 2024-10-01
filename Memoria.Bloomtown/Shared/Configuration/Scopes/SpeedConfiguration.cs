using System;
using Memoria.Bloomtown.Configuration.Hotkey;
using UnityEngine;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Speed")]
public abstract partial class SpeedConfiguration
{
    [ConfigEntry($"Speed up key." +
                 "$[Russian]: Кнопка для включения ускорения.")]
    [ConfigConverter(nameof(KeyConverter))]
    public virtual HotkeyGroup Key { get; } = HotkeyGroup.Create(new[] { new Hotkey.Hotkey(KeyCode.F1), new Hotkey.Hotkey(KeyCode.F1) { MustHeld = true } });

    [ConfigEntry($"Speed up toggle factor." +
                 "$[Russian]: Коэффициент ускорения при включении.")]
    public virtual Single ToggleFactor { get; set; } = 3.0f;

    [ConfigEntry($"Speed up hold factor." +
                 "$[Russian]: Коэффициент ускорения при удержании.")]
    public virtual Single HoldFactor { get; set; } = 5.0f;

    public abstract void CopyFrom(SpeedConfiguration configuration);
    public abstract void OverrideFrom(SpeedConfiguration configuration);

    protected IAcceptableValue<HotkeyGroup> KeyConverter { get; } = new AcceptableHotkeyGroup(canHold: true);
}