using System;
using Memoria.Bloomtown.Configuration.Hotkey;
using UnityEngine;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Saves")]
public abstract partial class SavesConfiguration
{
    [ConfigEntry("Key used for quick saving." +
                 "$[Russian]: Клавиша для быстрого сохранения.")]
    [ConfigConverter(nameof(KeyConverter))]
    public virtual HotkeyGroup QuickSaveKey { get; } = HotkeyGroup.Create([new Hotkey.Hotkey(KeyCode.F5)]);

    [ConfigEntry($"The maximum number of quick saves allowed." +
                 "$[Russian]: Максимальное количество быстрых сохранений.")]
    public virtual Int32 QuickSavesCount { get; set; } = 3;

    [ConfigEntry("Key used for quick loading the last saved game." +
                 "$[Russian]: Клавиша для быстрой загрузки последнего сохранения.")]
    [ConfigConverter(nameof(KeyConverter))]
    public virtual HotkeyGroup QuickLoadKey { get; } = HotkeyGroup.Create([new Hotkey.Hotkey(KeyCode.F9)]);

    [ConfigEntry("Key used for quick loading the exact quick saved game." +
                 "$[Russian]: Клавиша для быстрой загрузки именно быстрого сохранения.")]
    [ConfigConverter(nameof(KeyConverter))]
    public virtual HotkeyGroup QuickLoadExactKey { get; } = HotkeyGroup.Create([new Hotkey.Hotkey(KeyCode.F9) { Shift = true }]);

    [ConfigEntry($"Automatically save the game on exit." +
                 "$[Russian]: Автоматическое сохранение игры при выходе.")]
    public virtual Boolean AutoSaveOnExit { get; set; } = true;

    [ConfigEntry($"Delete the exit save on game load." +
                 "$[Russian]: Удалить автоматическое сохранение после загрузки.")]
    public virtual Boolean DeleteExitSaveOnLoad { get; set; } = true;
    
    [ConfigEntry($"Allow saving when fast travel is blocked. WARNING: This may break your gameplay!" +
                 "$[Russian]: Разрешает сохранение при заблокированном быстром перемещении. ВНИМАНИЕ: Это может сломать вам игру!")]
    public virtual Boolean AllowSavingWhenFastTravelBlocked { get; set; } = false;

    public abstract void CopyFrom(SavesConfiguration configuration);
    public abstract void OverrideFrom(SavesConfiguration configuration);

    protected IAcceptableValue<HotkeyGroup> KeyConverter { get; } = new AcceptableHotkeyGroup(canHold: false);
}