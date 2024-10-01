using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Battlefield")]
public abstract partial class BattlefieldConfiguration
{
    [ConfigEntry("When enabled, displays attack radius in addition to movement radius (Left Alt on a unit)." +
                 "$[Russian]: При включении, отображает радиус атаки в дополнение к радиусу перемещения (Левый Alt на юните).")]
    public virtual Boolean DisplayAttackRangeWithMovement => true;
    
    [ConfigEntry("When enabled, displays the attack range of the selected unit when the cursor is hovered over another cell (Left Ctrl)." +
                 "$[Russian]: Когда включено, отображает радиус атаки выбранного юнита, когда курсор наведён на другую ячейку (Левый Ctrl).")]
    public virtual Boolean DisplayAttackRangeOnCell => true;

    public abstract void CopyFrom(BattlefieldConfiguration configuration);
    public abstract void OverrideFrom(BattlefieldConfiguration configuration);
}