using System;

namespace Memoria.Bloomtown.Configuration;

[ConfigScope("UI")]
public abstract partial class UIConfiguration
{
    [ConfigEntry("Positive value sets the height of the [Garage.PaintWanzerPanel]. 0 to return the default size." +
                 "$[Russian]: Положительное значение устанавливает высоту [Garage.PaintWanzerPanel]. 0 для возврата стандартного размера.")]
    public virtual Single GaragePaintWanzerPanelHeight => 540;
    
    [ConfigEntry("Positive value sets the height of the [Garage.ChooseWanzerPanel]. 0 to return the default size." +
                 "$[Russian]: Положительное значение устанавливает высоту [Garage.ChooseWanzerPanel]. 0 для возврата стандартного размера.")]
    public virtual Single GarageChooseWanzerPanelHeight => 300;
    
    [ConfigEntry("Positive value sets the width of the [Garage.EquipWeaponPanel]. 0 to return the default size." +
                 "$[Russian]: Положительное значение устанавливает ширину [Garage.EquipWeaponPanel]. 0 для возврата стандартного размера.")]
    public virtual Single GarageEquipWeaponPanelWidth => 150;
    
    [ConfigEntry("Positive value sets the height of the [Garage.EquipWeaponPanel]. 0 to return the default size." +
                 "$[Russian]: Положительное значение устанавливает высоту [Garage.EquipWeaponPanel]. 0 для возврата стандартного размера.")]
    public virtual Single GarageEquipWeaponPanelHeight => 600;
    
    [ConfigEntry("Positive value sets the width of the [Garage.EquipPartPanel]. 0 to return the default size." +
                 "$[Russian]: Положительное значение устанавливает ширину [Garage.EquipPartPanel]. 0 для возврата стандартного размера.")]
    public virtual Single GarageEquipPartPanelWidth => 150;
    
    [ConfigEntry("Positive value sets the height of the [Garage.EquipPartPanel]. 0 to return the default size." +
                 "$[Russian]: Положительное значение устанавливает высоту [Garage.EquipPartPanel]. 0 для возврата стандартного размера.")]
    public virtual Single GarageEquipPartPanelHeight => 600;

    public abstract void CopyFrom(UIConfiguration configuration);
    public abstract void OverrideFrom(UIConfiguration configuration);
    
    public enum CallSignsFromNames
    {
        Disabled = 0,
        NormalCase = 1,
        UpperCase = 2
    }
}