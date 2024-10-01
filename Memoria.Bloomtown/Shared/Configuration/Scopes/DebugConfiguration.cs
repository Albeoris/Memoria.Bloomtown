using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Debug")]
public abstract partial class DebugConfiguration
{
    [ConfigEntry("When enabled, allows you to use the debug features from the main menu (the button in the upper-right corner of the screen)." +
                 "$[Russian]: При включении, позволяет использовать отладочные команды из главного меню игры (кнопка в верхнем-правом углу экрана).")]
    public virtual Boolean EnableDebugMenu => false;
    
    public abstract void CopyFrom(DebugConfiguration configuration);
    public abstract void OverrideFrom(DebugConfiguration configuration);
}