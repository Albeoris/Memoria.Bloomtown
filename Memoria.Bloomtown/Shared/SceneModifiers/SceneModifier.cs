using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Memoria.Bloomtown;

public sealed class SceneModifier : ISceneModifier
{
    private readonly Dictionary<String, ISceneModifier> _sceneModifiers = new();
    
    private SceneModifier()
    {
    }

    public static SceneModifier Initialize()
    {
        SceneModifier modifier = new();
        SceneManager.sceneLoaded += modifier.OnSceneLoaded;
        return modifier;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        try
        {
            if (_sceneModifiers.TryGetValue(scene.name, out ISceneModifier modificator))
                modificator.OnSceneLoaded(scene, mode);
        }
        catch (Exception ex)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ModComponent.Log.LogError($"[{nameof(ModComponent)}].{nameof(OnSceneLoaded)}({scene.name}, {mode}): {ex}");
            throw;
        }
    }
}