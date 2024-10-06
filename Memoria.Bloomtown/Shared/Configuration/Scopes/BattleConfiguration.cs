using System;
using Memoria.Bloomtown.Configuration.Hotkey;
using UnityEngine;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Battle")]
public abstract partial class BattleConfiguration
{
    [ConfigEntry($"Visibility setting for enemy heart." +
                 "$[Russian]: Настройка видимости сердца врага.")]
    public virtual EnemyHeartVisibility EnemyHeartVisibility { get; set; } = EnemyHeartVisibility.Default;

    public abstract void CopyFrom(BattleConfiguration configuration);
    public abstract void OverrideFrom(BattleConfiguration configuration);
}

public enum EnemyHeartVisibility
{
    Default = 1,
    ShowAlways,
    HideAlways,
}