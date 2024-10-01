using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Cheats")]
public abstract partial class CheatsConfiguration
{
    [ConfigEntry("Enables cheats built into the game. Most of them are available as a separate option in a menu, for example, on combat or store screens." +
                 "$[Russian]: Включает встроенные в игру читы. Большинство из них доступны в виде отдельной опции в диалоговом меню, например в бою или на экране магазина.")]
    public virtual Boolean Enabled => false;

    public abstract void CopyFrom(CheatsConfiguration configuration);
    public abstract void OverrideFrom(CheatsConfiguration configuration);
}