using UnityEngine.SceneManagement;

namespace Memoria.Bloomtown;

public interface ISceneModifier
{
    void OnSceneLoaded(Scene scene, LoadSceneMode mode);
}