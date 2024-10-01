using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Localization")]
public abstract partial class LocalizationConfiguration
{
    [ConfigEntry("When enabled, prohibits changing character names during the game." +
                 "$[Russian]: При включении, запрещает менять имена персонажей во время игры.")]
    public virtual Boolean ProhibitsChangingCharacterNames => false;
    
    [ConfigEntry("When enabled, prohibits changing character call signs during the game." +
                 "$[Russian]: При включении, запрещает менять позывные персонажей во время игры.")]
    public virtual Boolean ProhibitsChangingCharacterCallSigns => false;
    
    [ConfigEntry("Use character names instead of call signs." +
                 "$[Russian]: Использовать имена героев вместо позывных.")]
    public virtual CallSignsFromNames UseCharacterNamesInsteadCallSigns => CallSignsFromNames.Disabled;

    [ConfigEntry("Reduces the black outline of labels such as AP in combat to improve the appearance of Unicode characters." +
                 "$[Russian]: Уменьшает чёрную обводку надписей, таких как AP в бою, для улучшения внешнего вида юникодных символов.")]
    public virtual Boolean ReduceBlackOutlines => false;
    
    [ConfigEntry("Enables localization even if it is disabled by the developers." +
                 "$[Russian]: Включает локализацию даже в том случае, если она отключена разработчиками.")]
    public virtual Boolean ForceLocalization => false;

    public abstract void CopyFrom(LocalizationConfiguration configuration);
    public abstract void OverrideFrom(LocalizationConfiguration configuration);
    
    public enum CallSignsFromNames
    {
        Disabled = 0,
        NormalCase = 1,
        UpperCase = 2
    }
}