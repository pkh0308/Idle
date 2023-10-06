using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPopUp : UI_PopUp
{
    #region ���� �� ������
    enum Texts
    {
        NickNameText,
        StageText,
        EnhanceBtnText,
        WeaponBtnText,
        TreasureBtnText,
        ShopBtnText,
        GoldText,
        GemText,
        BossTryBtnText
    }
    TextMeshProUGUI _nicknameText;
    TextMeshProUGUI _stageText;
    TextMeshProUGUI _goldText;
    TextMeshProUGUI _gemText;
    TextMeshProUGUI[] _dmgTexts;
    Transform _dmgTextParent;

    enum Buttons
    {
        EnhanceBtn,
        WeaponBtn,
        TreasureBtn,
        ShopBtn,
        SettingsBtn,
        BossTryBtn
    }
    enum Images
    {
        Field01,
        Field02,
        EnemyField,
        HpBarBg,
        HpBar,
        Player,
        EnhanceBtn,
        WeaponBtn,
        TreasureBtn,
        ShopBtn,
        GoldIcon,
        GemIcon,
        BlackScene
    }
    Image _enhanceBtn;
    Image _weaponBtn;
    Image _treasureBtn;
    Image _shopBtn;
    GameObject[] _mainPopUps;

    Image _front;
    Image _back;
    Image _hpBar;
    Image _hpBarBg;
    Image _blackScene;
    RectTransform _enemyField;

    const int FieldMax = 1440;
    const int FieldOffset = 400;
    const int MoveSpeed = 15;

    enum AnimVar
    {
        OnCombat,
        DoAttack,
        DoNextStage
    }
    Animator _playerAnimator;

    enum Menus
    {
        Enhance,
        Weapon,
        Treasure,
        Shop
    }
    Menus curMenu;
    const int NumOfMenus = 4;

    enum State
    {
        Moving, 
        OnCombat,
        StageChange
    }
    State curState;
    #endregion

    #region �ʱ�ȭ
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.EnhanceBtn).gameObject).BindEvent(Btn_OnClickEnhance);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.WeaponBtn).gameObject).BindEvent(Btn_OnClickWeapon);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.TreasureBtn).gameObject).BindEvent(Btn_OnClickTreasure);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ShopBtn).gameObject).BindEvent(Btn_OnClickShop);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.SettingsBtn).gameObject).BindEvent(Btn_OnClickSettings);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.BossTryBtn).gameObject).BindEvent(Btn_OnClickBossTry);

        _nicknameText = GetText((int)Texts.NickNameText);
        _nicknameText.text = Managers.Game.NickName;
        _stageText = GetText((int)Texts.StageText);
        _goldText = GetText((int)Texts.GoldText);
        _goldText.text = Managers.Game.CurGold.ToString();
        _gemText = GetText((int)Texts.GemText);
        _gemText.text = Managers.Game.CurGold.ToString();

        _enhanceBtn = GetImage((int)Images.EnhanceBtn);
        _weaponBtn = GetImage((int)Images.WeaponBtn);
        _treasureBtn = GetImage((int)Images.TreasureBtn);
        _shopBtn = GetImage((int)Images.ShopBtn);

        _mainPopUps = new GameObject[NumOfMenus];
        Transform menus = transform.GetChild(0).Find(ConstValue.MenusParent);
        Managers.Resc.InstantiateByIdx(nameof(UI_EnhancePopUp), (int)Menus.Enhance, menus, (obj, idx) => { _mainPopUps[idx] = obj; CountMenuLoad(); });
        Managers.Resc.InstantiateByIdx(nameof(UI_WeaponPopUp), (int)Menus.Weapon, menus, (obj, idx) => { _mainPopUps[idx] = obj; CountMenuLoad(); });
        Managers.Resc.InstantiateByIdx(nameof(UI_TreasurePopUp), (int)Menus.Treasure, menus, (obj, idx) => { _mainPopUps[idx] = obj; CountMenuLoad(); });
        Managers.Resc.InstantiateByIdx(nameof(UI_ShopPopUp), (int)Menus.Shop, menus, (obj, idx) => { _mainPopUps[idx] = obj; CountMenuLoad(); });

        _front = GetImage((int)Images.Field01);
        _back = GetImage((int)Images.Field02);
        _hpBar = GetImage((int)Images.HpBar);
        _hpBarBg = GetImage((int)Images.HpBarBg);
        _blackScene = GetImage((int)Images.BlackScene);
        _blackScene.gameObject.SetActive(false);

        _enemyField = GetImage((int)Images.EnemyField).rectTransform;
        _playerAnimator = GetImage((int)Images.Player).gameObject.GetComponent<Animator>();

        // ���� ���ӱ���� �ִٸ� �������� ����
        if (Managers.Game.LastAccessMinutes > 0) 
            Managers.UI.OpenPopUp<UI_OfflineRewardPopUp>(typeof(UI_OfflineRewardPopUp).Name, transform);
        curMenu = Menus.Enhance;
        curState = State.Moving;

        // UI �ʱ�ȭ
        _dmgTextParent = transform.GetChild(0).Find(ConstValue.DmgTextParent);
        TextPooling();
        UpdateGoldGem();
        UpdateStageText();
        InitialSetActiveFalse();
        // �̵� & ���� �ʱ�ȭ
        InitStage();
        StartCoroutine(Idle());
        StartCoroutine(Combat());
        // Bgm
        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_Main);
        return true;
    }

    // �ʱ⿡ ��Ȱ��ȭ�� ������Ʈ��
    void InitialSetActiveFalse()
    { 
        _hpBarBg.gameObject.SetActive(false);
    }

    int _mCount = 0;
    void CountMenuLoad()
    {
        _mCount++;
        if (_mCount < NumOfMenus)
            return;

        // ù��° �޴�(��ȭ) �� ��Ȱ��ȭ
        for (int i = 1; i < NumOfMenus; i++)
            _mainPopUps[i].SetActive(false);
    }

    void TextPooling()
    {
        _dmgTexts = new TextMeshProUGUI[100];
        for (int i = 0; i < _dmgTexts.Length; i++)
        {
            Managers.Resc.InstantiateByIdx(ConstValue.DmgText, i, _dmgTextParent, (op, idx) =>
            {
                _dmgTexts[idx] = op.GetComponent<TextMeshProUGUI>();
                op.SetActive(false);
            });
        }
    }
    #endregion

    #region Update
    int _befGold;
    int _befGem;
    void Update()
    {
        if (_befGold == Managers.Game.CurGold && _befGem == Managers.Game.CurGem)
            return;

        UpdateGoldGem();
        _befGold = Managers.Game.CurGold;
        _befGem = Managers.Game.CurGem;
    }
    #endregion

    #region ��ư
    public void Btn_OnClickEnhance()
    {
        if (curMenu == Menus.Enhance)
            return;

        _mainPopUps[(int)curMenu].SetActive(false);
        curMenu = Menus.Enhance;
        _mainPopUps[(int)curMenu].SetActive(true);
        UpdateMenuButtons();
    }
    public void Btn_OnClickWeapon()
    {
        if (curMenu == Menus.Weapon)
            return;

        _mainPopUps[(int)curMenu].SetActive(false);
        curMenu = Menus.Weapon;
        _mainPopUps[(int)curMenu].SetActive(true);
        UpdateMenuButtons();
    }
    public void Btn_OnClickTreasure()
    {
        if (curMenu == Menus.Treasure)
            return;

        _mainPopUps[(int)curMenu].SetActive(false);
        curMenu = Menus.Treasure;
        _mainPopUps[(int)curMenu].SetActive(true);
        UpdateMenuButtons();
    }
    public void Btn_OnClickShop()
    {
        if (curMenu == Menus.Shop)
            return;

        _mainPopUps[(int)curMenu].SetActive(false);
        curMenu = Menus.Shop;
        _mainPopUps[(int)curMenu].SetActive(true);
        UpdateMenuButtons();
    }

    public void Btn_OnClickSettings()
    {
        Managers.UI.OpenPopUp<UI_SettingsPopUp>();
    }

    public void Btn_OnClickBossTry()
    {
        Managers.UI.OpenPopUp<UI_BossTryPopUp>();
    }
    #endregion

    #region ���� 
    Image[] _enemies;
    int _loadCount = 0;
    int _combatLine = 0;

    void InitStage()
    {
        // ������ ����
        Managers.Game.InitStage();

        _enemies = new Image[Managers.Game.MaxEnemyCount];
        _loadCount = Managers.Game.MaxEnemyCount;
        _combatLine = (-1 * FieldMax) + FieldOffset;

        int idx = 0;
        while (Managers.Game.GetEnemyId(idx) > 0)
        {
            string name = ConstValue.Enemy + Managers.Game.GetEnemyId(idx);
            Managers.Resc.InstantiateByIdx(name, idx, _enemyField, (op, idx) => {
                _enemies[idx] = op.GetComponent<Image>();
                OnEnemyLoadComplete();
            });
            idx++;
        }
    }

    void OnEnemyLoadComplete()
    {
        _loadCount--;
        if (_loadCount > 0)
            return;

        Vector2 offset = Vector2.right * FieldMax; 
        for (int i = 0; i < _enemies.Length; i++)
        {
            _enemies[i].rectTransform.anchoredPosition += offset;
            offset += Vector2.right * FieldMax;
        }
        curState = State.Moving;
    }

    void RemoveField()
    {
        for (int i = 0; i < _enemies.Length; i++)
        {
            Managers.Resc.Release(_enemies[i].gameObject.name);
            Managers.Resc.Destroy(_enemies[i].gameObject);
        }
    }

    IEnumerator Idle()
    {
        while(true)
        {
            if (curState == State.OnCombat)
            {
                yield return null;
                continue;
            }

            Move();
            yield return null;
        }
    }

    Vector2 _moveOffset = Vector2.left * MoveSpeed;
    void Move()
    {
        // �ʵ� ���� ����
        if (_back.rectTransform.anchoredPosition.x <= 0)
            ReverseField();

        _front.rectTransform.anchoredPosition += _moveOffset;
        _back.rectTransform.anchoredPosition += _moveOffset;
        _enemyField.anchoredPosition += _moveOffset;
        
        // ���� ����
        if (curState != State.StageChange && _enemyField.anchoredPosition.x <= _combatLine)
        {
            curState = State.OnCombat;
            _playerAnimator.SetBool(AnimVar.OnCombat.ToString(), true);
            _hpBarBg.gameObject.SetActive(true);
            _hpBar.rectTransform.localScale = Vector2.one;

            _combatLine -= FieldMax;
        }
    }

    // �ʵ� ���� ����
    void ReverseField()
    {
        _front.rectTransform.anchoredPosition = _back.rectTransform.anchoredPosition + (Vector2.right * FieldMax);
        Image temp = _front;
        _front = _back;
        _back = temp;
    }

    int _curDelay;
    IEnumerator Combat()
    {
        _curDelay = ConstValue.MaxAttackDelay;
        while (true)
        {
            if (curState != State.OnCombat)
            {
                yield return null;
                continue;
            }

            if(_curDelay >= ConstValue.MaxAttackDelay)
            {
                _curDelay = 0;
                Attack();
            }
            else
            {
                _curDelay += Managers.Game.GetAttackDelay();
            }
            yield return null;
        }
    }

    Vector2 hpVec = Vector2.one;
    void Attack()
    {
        _playerAnimator.SetTrigger(AnimVar.DoAttack.ToString());
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Attack, 2.0f);
        int dmg = Managers.Game.Attack(out bool critical);
        ShowDmgText(dmg, critical);

        // ü�¹� ����
        float hpRate = (float)Managers.Game.EnemyHp / Managers.Game.MaxEnemyHp;
        if(hpRate <= 0)
        {
            OnEnemyDie();
            return;
        }

        hpVec.x = hpRate;
        _hpBar.rectTransform.localScale = hpVec;
    }

    void OnEnemyDie()
    {
        curState = State.Moving;
        _playerAnimator.SetBool(AnimVar.OnCombat.ToString(), false);
        _hpBarBg.gameObject.SetActive(false);
        _curDelay = ConstValue.MaxAttackDelay; // ���ݵ����� �ʱ�ȭ
        DrawEnemyDead();

        if (Managers.Game.OnEnemyDie())
            StartCoroutine(OnNextStage());
            
        UpdateGoldGem();
    }

    // �� óġ �� ��������Ʈ ����
    void DrawEnemyDead()
    {
        int idx = Managers.Game.CurEnemyCount;
        name = ConstValue.Sprite_EnemyDie + Managers.Game.GetCurEnemyId(); 
        Managers.Resc.Load<Sprite>(name, (op) => { _enemies[idx].sprite = op; });
    }

    IEnumerator OnNextStage()
    {
        curState = State.StageChange;
        _blackScene.gameObject.SetActive(true);
        _hpBarBg.gameObject.SetActive(false);
        _playerAnimator.SetTrigger(AnimVar.DoNextStage.ToString());
        yield return Managers.Wfs.GetWaitForSeconds(ConstValue.StageChangeTime / 2);

        RemoveField();
        UpdateStageText();
        yield return Managers.Wfs.GetWaitForSeconds(ConstValue.StageChangeTime / 2);

        _blackScene.gameObject.SetActive(false);
        _enemyField.anchoredPosition = Vector2.left * FieldOffset;
        curState = State.Moving;
        InitStage();
    }
    #endregion

    #region UI ����
    void UpdateGoldGem()
    {
        _goldText.text = Custom.CalUnit(Managers.Game.CurGold);
        _gemText.text = Custom.CalUnit(Managers.Game.CurGem);
    }

    void UpdateStageText()
    {
        int floor = 1,  stage = 1;
        int stageIdx = Managers.Game.CurStageIdx;
        if(stageIdx > Managers.Data.MaxStageIdx)
            _stageText.gameObject.SetActive(false);

        while (stageIdx > 100)
        {
            stageIdx /= 100;
            floor++;
        }
        stage += stageIdx;
        _stageText.text = $"Stage {floor} - {stage}";
    }

    const string Menu_Normal = "Sprite_Menu_Normal";
    const string Menu_Selected = "Sprite_Menu_Selected";
    void UpdateMenuButtons()
    {
        string enhanceKey = curMenu == Menus.Enhance ? Menu_Selected : Menu_Normal;
        string weaponKey = curMenu == Menus.Weapon ? Menu_Selected : Menu_Normal;
        string treasureKey = curMenu == Menus.Treasure ? Menu_Selected : Menu_Normal;
        string shopKey = curMenu == Menus.Shop ? Menu_Selected : Menu_Normal;

        Managers.Resc.Load<Sprite>(enhanceKey, (op) => { _enhanceBtn.sprite = op; });
        Managers.Resc.Load<Sprite>(weaponKey, (op) => { _weaponBtn.sprite = op; });
        Managers.Resc.Load<Sprite>(treasureKey, (op) => { _treasureBtn.sprite = op; });
        Managers.Resc.Load<Sprite>(shopKey, (op) => { _shopBtn.sprite = op; });
    }

    void ShowDmgText(int value, bool critical)
    {
        TextMeshProUGUI dmgText = null;
        // ��Ȱ�� ������ �ؽ�Ʈ ã��
        for (int i = 0; i < _dmgTexts.Length; i++)
        {
            if (_dmgTexts[i].gameObject.activeSelf)
                continue;

            dmgText = _dmgTexts[i];
            break;
        }

        dmgText.color = critical ? Color.red : Color.white;
        dmgText.text = Custom.CalUnit(value);
        dmgText.gameObject.SetActive(true);
    }
    #endregion

    #region Input Action
    void OnEscape()
    {
        // �ֻ��� �˾� �ݱ�
        if (Managers.UI.ClosePopUp())
            return;

        // �˾��� ���ٸ� ���� �˾� ����
        Managers.UI.OpenPopUp<UI_ExitPopUp>(typeof(UI_ExitPopUp).Name, transform);
    }
    #endregion
}