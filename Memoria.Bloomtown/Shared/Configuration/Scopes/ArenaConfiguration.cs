using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("Arena")]
public abstract partial class ArenaConfiguration
{
    [ConfigEntry("If enabled, each pilot will be able to fight only one time with each opponent in the arena. You no longer lose money when you lose." +
                 "$[Russian]: При включении, каждый пилот сможет сразиться лишь один раз с каждым противником на арене. Вы больше не теряете деньги при поражении.")]
    public virtual Boolean DisableRepeatedBattles => false;

    public abstract void CopyFrom(ArenaConfiguration configuration);
    public abstract void OverrideFrom(ArenaConfiguration configuration);
}