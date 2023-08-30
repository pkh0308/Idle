using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPopUp : UI_PopUp
{
    #region 변수 및 열거형
    enum Texts
    {
        NickNameText,
        StageText,
        EnhanceBtnText,
        WeaponBtnText,
        TreasureBtnText,
        ShopBtnText,
        GoldText,
        GemText
    }
    TextMeshProUGUI _nicknameText;
    TextMeshProUGUI _stageText;
    TextMeshProUGUI _goldText;
    TextMeshProUGUI _gemText;
    const string STAGE = "Stage ";

    enum Buttons
    {
        EnhanceBtn,
        WeaponBtn,
        TreasureBtn,
        ShopBtn
    }
    enum Images
    {
        Field01,
        Field02,
        Enemy01,
        Enemy02,
        HpBarBg,
        HpBar,
        Player,
        EnhanceBtn,
        WeaponBtn,
        TreasureBtn,
        ShopBtn,
        GoldIcon,
        GemIcon
    }
    Image _front;
    Image _back;
    Image _frontEnemy;
    Image _backEnemy;
    Image _hpBar;
    Image _hpBarBg;
    bool _onCombat = false;
    const int FieldMax = 1440;

    enum AnimVar
    {
        OnCombat,
        DoAttack
    }
    Animator _playerAnimator;

    enum State
    {
        Enhance,
        Weapon,
        Treasure,
        Shop
    }
    State curState;
    #endregion

    #region 초기화
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.EnhanceBtn).gameObject).BindEvent(Btn_OnClickEnhance);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.WeaponBtn).gameObject).BindEvent(Btn_OnClickWeapon);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.TreasureBtn).gameObject).BindEvent(Btn_OnClickTreasure);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ShopBtn).gameObject).BindEvent(Btn_OnClickShop);

        _nicknameText = GetText((int)Texts.NickNameText);
        _nicknameText.text = Managers.Game.NickName;
        _stageText = GetText((int)Texts.StageText);
        _goldText = GetText((int)Texts.GoldText);
        _goldText.text = Managers.Game.CurGold.ToString();
        _gemText = GetText((int)Texts.GemText);
        _gemText.text = Managers.Game.CurGold.ToString();

        _front = GetImage((int)Images.Field01);
        _back = GetImage((int)Images.Field02);
        _frontEnemy = GetImage((int)Images.Enemy01);
        _backEnemy = GetImage((int)Images.Enemy02);
        _hpBar = GetImage((int)Images.HpBar);
        _hpBarBg = GetImage((int)Images.HpBarBg);
        _playerAnimator = GetImage((int)Images.Player).gameObject.GetComponent<Animator>();

        Managers.UI.OpenPopUp<UI_EnhancePopUp>(typeof(UI_EnhancePopUp).Name, transform);
        curState = State.Enhance;

        // UI 초기화
        UpdateGoldGem();
        UpdateStageText();
        InitialSetActiveFalse();
        // 이동 & 전투 초기화
        Managers.Game.InitStage();
        StartCoroutine(Idle());
        StartCoroutine(Combat());
        SpawnEnemy();
        // Bgm
        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_Main);
        return true;
    }

    // 초기에 비활성화할 오브젝트들
    void InitialSetActiveFalse()
    { 
        _hpBarBg.gameObject.SetActive(false);
        _frontEnemy.gameObject.SetActive(false);
    }
    #endregion

    #region 버튼
    public void Btn_OnClickEnhance()
    {
        if (curState == State.Enhance)
            return;

        Managers.UI.OpenPopUp<UI_EnhancePopUp>(typeof(UI_EnhancePopUp).Name, transform);
        curState = State.Enhance;
    }
    public void Btn_OnClickWeapon()
    {
        if (curState == State.Weapon)
            return;

        Managers.UI.OpenPopUp<UI_WeaponPopUp>(typeof(UI_WeaponPopUp).Name, transform);
        curState = State.Weapon;
    }
    public void Btn_OnClickTreasure()
    {
        if (curState == State.Treasure)
            return;

        Managers.UI.OpenPopUp<UI_TreasurePopUp>(typeof(UI_TreasurePopUp).Name, transform);
        curState = State.Treasure;
    }
    public void Btn_OnClickShop()
    {
        if (curState == State.Shop)
            return;

        Managers.UI.OpenPopUp<UI_ShopPopUp>(typeof(UI_ShopPopUp).Name, transform);
        curState = State.Shop;
    }
    #endregion

    #region 전투 
    IEnumerator Idle()
    {
        while(true)
        {
            if (_onCombat)
            {
                yield return null;
                continue;
            }

            Move();
            yield return null;
        }
    }

    void Move()
    {
        if (_back.rectTransform.anchoredPosition.x == 0)
        {
            // 필드 순서 반전 및 몹 설정
            ReverseField();
            SpawnEnemy();

            // 전투 진입
            _onCombat = true;
            _playerAnimator.SetBool(AnimVar.OnCombat.ToString(), true);
            _hpBarBg.gameObject.SetActive(true);
            _hpBar.rectTransform.localScale = Vector2.one;
            return;
        }

        _front.rectTransform.anchoredPosition += Vector2.left * 5;
        _back.rectTransform.anchoredPosition += Vector2.left * 5;
    }

    void ReverseField()
    {
        _front.rectTransform.anchoredPosition = Vector2.right * FieldMax;
        Image temp = _front;
        _front = _back;
        _back = temp;

        temp = _frontEnemy;
        _frontEnemy = _backEnemy;
        _backEnemy = temp;
        _backEnemy.gameObject.SetActive(true);
    }

    void SpawnEnemy()
    {
        // ToDo: _backEnemy를 랜덤 몹 스프라이트로 갱신
        //Managers.Resc.Load<Sprite>("", (op) =>
        //{
        //    _backEnemy.sprite = op;
        //});
    }

    IEnumerator Combat()
    {
        while (true)
        {
            if (_onCombat == false)
            {
                yield return null;
                continue;
            }

            Attack();
            // ToDo: 공격속도 반영하도록 수정
            yield return new WaitForSeconds(1.0f);
        }
    }

    Vector2 hpVec = Vector2.one;
    void Attack()
    {
        _playerAnimator.SetTrigger(AnimVar.DoAttack.ToString());
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Attack, 2.0f);
        Managers.Game.Attack();

        // 체력바 갱신
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
        _onCombat = false;
        _playerAnimator.SetBool(AnimVar.OnCombat.ToString(), false);
        _frontEnemy.gameObject.SetActive(false);
        _hpBarBg.gameObject.SetActive(false);

        if(Managers.Game.OnEnemyDie())
            UpdateStageText();
        UpdateGoldGem();
    }
    #endregion

    #region UI 갱신
    void UpdateGoldGem()
    {
        _goldText.text = Custom.CalUnit(Managers.Game.CurGold);
        _gemText.text = Custom.CalUnit(Managers.Game.CurGem);
    }

    void UpdateStageText()
    {
        int floor = 1,  stage = 1;
        int stageIdx = Managers.Game.CurStageIdx;

        while(stageIdx > 100)
        {
            stageIdx /= 100;
            floor++;
        }
        stage += stageIdx;
        _stageText.text = STAGE + floor + " - " + stage;
    }
    #endregion
}