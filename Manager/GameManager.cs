using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    enum GameState
    {
        Title,
        Main
    }
    GameState curState;

    enum Scenes
    {
        TitleScene,
        MainScene
    }

    public void Init()
    {
        curState = GameState.Title;
    }

    #region GameData 관리
    int atkPower;
    int atkSpeed;
    int critChance;
    int critDamage;
    int goldUp;

    public int CurGold;
    public int CurGem;

    void GetGameDataFromDataManager()
    {
        GameData data = Managers.Data.CurGameData;

        atkPower = data.AtkPowLv;
        atkSpeed = data.AtkSpdLv;
        critChance = data.CritChanceLv;
        critDamage = data.CritDmgLv;
        goldUp = data.GoldUpLv;
    }

    public void UpdateGameData()
    {
        GameData data = new GameData(atkPower, atkSpeed, critChance, critDamage, goldUp);
        Managers.Data.SetGameData(data);
    }
    #endregion

    public void StartNewGame()
    {
        Managers.Data.CreateNewGameData();
        GetGameDataFromDataManager();
        curState = GameState.Main; Debug.Log(curState);
        SceneManager.LoadScene(Scenes.MainScene.ToString());
    }

    public bool LoadGame()
    {
        if (Managers.Data.CurGameData == null)
            return false;

        // ToDo: Main 씬 로드 후 저장된 데이터로 초기화
        GetGameDataFromDataManager();
        curState = GameState.Main;
        SceneManager.LoadScene(Scenes.MainScene.ToString());
        return true;
    }
}