using System;

namespace Memoria.Bloomtown.Core;

public abstract class SafeComponent
{
    protected Boolean _isDisabled;

    protected virtual void Update()
    {
    }
    
    protected virtual void OnGUI()
    {
    }

    public void TryUpdate()
    {
        try
        {
            if (_isDisabled)
                return;

            Update();
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            ModComponent.Log.LogError($"[{GetType().Name}].{nameof(Update)}(): {ex}");
        }   
    }
    
    public void TryOnGUI()
    {
        try
        {
            if (_isDisabled)
                return;

            OnGUI();
        }
        catch (Exception ex)
        {
            _isDisabled = true;
            ModComponent.Log.LogError($"[{GetType().Name}].{nameof(OnGUI)}(): {ex}");
        }   
    }
}