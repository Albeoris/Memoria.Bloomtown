using System;
using System.Collections;
using FMODUnity;
using HarmonyLib;
using Memoria.Bloomtown;
using Memoria.Bloomtown.BeepInEx;
using Memoria.Bloomtown.Core;
using UnityEngine.Events;
using UnityEngine.UI;
using Uroboros.Extension;

[HarmonyPatch(typeof(SceneLoading), "ReturnToTitleScreenScene")]
public static class SceneLoading_ReturnToTitleScreenScene
{
    public static void Prefix()
    {
        try
        {
            GameSaveControl.TrySaveOnExit();
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}

[HarmonyPatch(typeof(SystemPanel), "ShowExitPanel")]
public static class SystemPanel_ShowExitPanel
{
    [HarmonyPostfix]
    private static void ShowExitPanel(SystemPanel __instance,
        ConfirmPanel ___confirmPanel)
    {
        try
        {
            if (!ModComponent.Instance.Config.Saves.AutoSaveOnExit)
                return;
                
            Button component = __instance.exitPanel.transform.Find("ButtonYes").GetComponent<Button>();
            Button.ButtonClickedEvent originalEvent = component.onClick;
            
            component.SetOnClick(() =>
            {
                if (!GameSaveControl.IsSaveLoadInteractable(onExit: true))
                {
                    originalEvent.Invoke();
                    return;
                }
                
                RuntimeManager.PlayOneShot("event:/Interface/PauseMenuButtonPush");

                String label;
                if (GameSaveControl.IsSavesBlocked(out String reason))
                {
                    label = $"<color=#FF6347>{LocalizationManager.L("all unsaved data is deleted") + '\n' + reason}</color>";
                }
                else
                {
                    const String localizationKey = "Memoria.Saves.GameWillBeSavedOnExitMessage";
                    LocalizationManager.cur_lng.dict.TryAdd(localizationKey, "The current game will be saved.");
                    label = $"<color=#1E90FF>{LocalizationManager.L(localizationKey)}</color>";
                }
                
                Type systemPanelType = typeof(SystemPanel);

                String buttonOkName = LocalizationManager.L("exit");

                UnsubscribeSwitchCategory();

                ___confirmPanel.Show(label, ActionOnOkButton, buttonOkName, ActionOnCancelButton);

                void ActionOnOkButton()
                {
                    __instance.StartCoroutine(ShowAnimationAndLoadMainMenu());
                }

                void ActionOnCancelButton()
                {
                    SelectedButtonOnSelect();
                    SubscribeSwitchCategory();
                }
                
                void SelectedButtonOnSelect() => AccessTools.Method(systemPanelType, "SelectedButtonOnSelect").Invoke(__instance, []);
                void SubscribeSwitchCategory() => AccessTools.Method(systemPanelType, "SubscribeSwitchCategory").Invoke(__instance, []);
                void UnsubscribeSwitchCategory() => AccessTools.Method(systemPanelType, "UnsubscribeSwitchCategory").Invoke(__instance, []);
                IEnumerator ShowAnimationAndLoadMainMenu() => (IEnumerator)AccessTools.Method(systemPanelType, "ShowAnimationAndLoadMainMenu").Invoke(__instance, []);
            });
        }
        catch (Exception ex)
        {
            ModComponent.Log.LogException(ex);
        }
    }
}