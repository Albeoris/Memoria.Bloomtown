using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Configuration;
using UnityEngine.UI;

namespace Memoria.Bloomtown.HarmonyHooks.AlwaysRun;

[HarmonyPatch(typeof(BattleUI), "UnhoverTarget")]
public static class BattleUI_UnhoverTarget
{
    public static void Postfix(BattleParticipantView targetView,
        BattleUI __instance,
        Dictionary<BattleParticipantView, Slider> ___m_hearts)
    {
        if (!BattleUI_HoverTarget.IsEnabled)
            return;
        
        try
        {
            EnemyHeartVisibility visibility = ModComponent.Instance.Config.Battle.EnemyHeartVisibility;
            if (visibility != EnemyHeartVisibility.ShowAlways)
                return;

            BattleManager battleManager = BattleManager.instance;
            if (battleManager is null)
                return;

            foreach (BattleParticipantController controller in battleManager.monsters.Where(m => m.hpPercent > 0))
            {
                if (___m_hearts.TryGetValue(controller.view, out Slider heart))
                    heart.gameObject.SetActive(true);
            }
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}