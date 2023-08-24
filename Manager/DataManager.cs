using System.IO;
using System.Xml;
using UnityEngine;

public class DataManager 
{
    const string Root = "root";
    const string GameDataPath = "/XML/GameDatas.xml";

    public GameData CurGameData { get; private set; }

    public void Init()
    {
        LoadGameData();
    }

    #region GameData
    public void LoadGameData()
    {
        //데이터를 형성할 문서 생성 및 파일읽기
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + GameDataPath;
        if (File.Exists(path) == false)
        {
            CurGameData = null;
            return;
        }

        // 루트 설정
        doc.Load(Application.dataPath + GameDataPath);
        XmlElement nodes = doc[Root];

        foreach (XmlElement node in nodes.ChildNodes)
        {
            // 속성 읽어오기
            int atkPowLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkPowLv.ToString()));
            int atkSpeedLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkSpdLv.ToString()));
            int critChanceLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritChanceLv.ToString()));
            int critDmgLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritDmgLv.ToString()));
            int goldUpLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.GoldUpLv.ToString()));

            //가져온 데이터 저장
            GameData data = new GameData(atkPowLv, atkSpeedLv, critChanceLv, critDmgLv, goldUpLv);
            CurGameData = data;
        }
    }

    public void SaveGameData()
    {
        if (CurGameData == null)
        {
            Debug.Log("There is no GameData");
            return;
        }

        XmlDocument doc = new XmlDocument();
        XmlElement root = doc.CreateElement(Root);
        doc.AppendChild(root);

        XmlElement element = doc.CreateElement("Data");
        string[] names = System.Enum.GetNames(typeof(ConstValue.GameDataVal));
        for (int i = 0; i < names.Length; i++)
            element.SetAttribute(names[i], CurGameData.GetName(i));
        root.AppendChild(element);
        
        doc.Save(Application.dataPath + GameDataPath);
    }

    // 신규 게임데이터 생성
    public void CreateNewGameData()
    {
        GameData gameData = new GameData(0, 0, 0, 0, 0);
        CurGameData = gameData;
    }
    #endregion
}
