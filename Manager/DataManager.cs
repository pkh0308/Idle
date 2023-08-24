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
        //�����͸� ������ ���� ���� �� �����б�
        XmlDocument doc = new XmlDocument();
        string path = Application.dataPath + GameDataPath;
        if (File.Exists(path) == false)
        {
            CurGameData = null;
            return;
        }

        // ��Ʈ ����
        doc.Load(Application.dataPath + GameDataPath);
        XmlElement nodes = doc[Root];

        foreach (XmlElement node in nodes.ChildNodes)
        {
            // �Ӽ� �о����
            int atkPowLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkPowLv.ToString()));
            int atkSpeedLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.AtkSpdLv.ToString()));
            int critChanceLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritChanceLv.ToString()));
            int critDmgLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.CritDmgLv.ToString()));
            int goldUpLv = System.Convert.ToInt32(node.GetAttribute(ConstValue.GameDataVal.GoldUpLv.ToString()));

            //������ ������ ����
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

    // �ű� ���ӵ����� ����
    public void CreateNewGameData()
    {
        GameData gameData = new GameData(0, 0, 0, 0, 0);
        CurGameData = gameData;
    }
    #endregion
}
