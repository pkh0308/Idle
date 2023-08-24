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

    public void StartNewGame()
    {
        Managers.Data.CreateNewGameData();
        curState = GameState.Main; Debug.Log(curState);
        SceneManager.LoadScene(Scenes.MainScene.ToString());
    }

    public bool LoadGame()
    {
        if (Managers.Data.CurGameData == null)
            return false;

        // ToDo: Main �� �ε� �� ����� �����ͷ� �ʱ�ȭ
        curState = GameState.Main;
        SceneManager.LoadScene(Scenes.MainScene.ToString());
        return true;
    }
}