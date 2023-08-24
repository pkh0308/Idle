using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static GameManager gameManager = new GameManager();
    private static UIManager uiManager = new UIManager();
    private static ResourceManager resourceManager = new ResourceManager();
    private static DataManager dataManager = new DataManager();
    private static SoundManager soundManager = new SoundManager();

    public static GameManager Game {  get { return gameManager; } }
    public static UIManager UI { get {  return uiManager; } }
    public static ResourceManager Resc { get {  return resourceManager; } }
    public static DataManager Data { get { return dataManager; } }
    public static SoundManager Sound { get { return soundManager; } }

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        gameManager.Init();
        uiManager.Init();
        resourceManager.Init();
        dataManager.Init();
        soundManager.Init();
    }

    void OnApplicationQuit()
    {
        dataManager.SaveGameData();
    }
}
