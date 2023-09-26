using UnityEngine;

public class Managers : MonoBehaviour
{
    private static GameManager gameManager = new GameManager();
    private static UIManager uiManager = new UIManager();
    private static ResourceManager resourceManager = new ResourceManager();
    private static DataManager dataManager = new DataManager();
    private static SoundManager soundManager = new SoundManager();
    private static WfsManager wfsManager = new WfsManager();
    private static AdvManager advManager = new AdvManager();

    public static GameManager Game {  get { return gameManager; } }
    public static UIManager UI { get {  return uiManager; } }
    public static ResourceManager Resc { get {  return resourceManager; } }
    public static DataManager Data { get { return dataManager; } }
    public static SoundManager Sound { get { return soundManager; } }
    public static WfsManager Wfs { get { return wfsManager; } }
    public static AdvManager Adv { get { return advManager; } }

    static bool _initialized;
    void Start()
    {
        if (_initialized)
            return;

        _initialized = true;
        DontDestroyOnLoad(gameObject);
        
        gameManager.Init();
        uiManager.Init();
        resourceManager.Init();
        dataManager.Init();
        soundManager.Init();
        wfsManager.Init();
        advManager.Init();

        // 프레임 제한
        Application.targetFrameRate = ConstValue.MaxFrame;
    }

    void OnApplicationQuit()
    {
        // GameData 갱신 후 저장
        gameManager.UpdateGameData();
        dataManager.SaveGameData();
    }
}