using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using LazyBearTechnology;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Configuration;
using UnityEngine.UI;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(BattleUI), "HoverTarget")]
public static class BattleUI_HoverTarget
{
    internal static Boolean IsEnabled { get; private set; } = true;

    public static void Postfix(BattleParticipantController targetController,
        BattleUI __instance,
        Dictionary<BattleParticipantView, Slider> ___m_hearts)
    {
        if (!IsEnabled)
            return;
        
        try
        {
            EnemyHeartVisibility visibility = ModComponent.Instance.Config.Battle.EnemyHeartVisibility;
            if (visibility == EnemyHeartVisibility.HideAlways)
            {
                if (targetController.isMonster)
                    __instance.HideHeart(targetController.view);
            }
            else if (visibility == EnemyHeartVisibility.ShowAlways)
            {
                foreach (BattleParticipantController monster in BattleManager.instance.monsters.Where(m=>m.hpPercent > 0))
                {
                    if (!___m_hearts.ContainsKey(monster.view))
                    {
                        IsEnabled = false;
                        __instance.UnhoverTarget(targetController.view);
                        __instance.HoverTarget(monster);
                        __instance.UnhoverTarget(monster.view);
                        __instance.HoverTarget(targetController);
                        IsEnabled = true;
                    }

                    ___m_hearts[monster.view].gameObject.SetActive(true);
                }
            }
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}