using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainPopUp : UI_PopUp
{
    enum Texts
    {
        StageText,
        EnhanceBtnText,
        WeaponBtnText,
        TreasureBtnText,
        ShopBtnText,
        GoldText,
        GemText
    }
    TextMeshProUGUI _goldText;
    TextMeshProUGUI _gemText;

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

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.EnhanceBtn).gameObject).BindEvent(Btn_OnClickEnhance);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.WeaponBtn).gameObject).BindEvent(Btn_OnClickWeapon);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.TreasureBtn).gameObject).BindEvent(Btn_OnClickTreasure);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ShopBtn).gameObject).BindEvent(Btn_OnClickShop);

        _goldText = GetText((int)Texts.GoldText);
        _gemText = GetText((int)Texts.GemText);
        _front = GetImage((int)Images.Field01);
        _back = GetImage((int)Images.Field02);
        _frontEnemy = GetImage((int)Images.Enemy01);
        _backEnemy = GetImage((int)Images.Enemy02);
        _playerAnimator = GetImage((int)Images.Player).gameObject.GetComponent<Animator>();

        Managers.UI.OpenPopUp<UI_EnhancePopUp>(typeof(UI_EnhancePopUp).Name, transform);
        curState = State.Enhance;

        StartCoroutine(Idle());
        StartCoroutine(Combat());
        GoldGemUpdate();
        return true;
    }

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
            // 필드 순서 이동
            _front.rectTransform.anchoredPosition = Vector2.right * FieldMax;
            Image temp = _front;
            _front = _back;
            _back = temp;

            // 전투 진입
            _onCombat = true;
            _playerAnimator.SetBool(AnimVar.OnCombat.ToString(), true);
            return;
        }

        _front.rectTransform.anchoredPosition += Vector2.left * 5;
        _back.rectTransform.anchoredPosition += Vector2.left * 5;
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
            yield return new WaitForSeconds(1.0f);
        }
    }

    int _hp = 5;
    void Attack()
    {
        _playerAnimator.SetTrigger(AnimVar.DoAttack.ToString());
        _hp--;
        if(_hp == 0)
        {
            _onCombat = false;
            _playerAnimator.SetBool(AnimVar.OnCombat.ToString(), false);
            _hp = 5;
        }
    }

    void SpawnEnemy()
    {
        // ToDo: _backEnemy를 랜덤 몹 스프라이트로 갱신
        // 체력 UI 갱신
    }
    #endregion

    #region UI 갱신
    void GoldGemUpdate()
    {
        _goldText.text = CalUnit(Managers.Game.CurGold);
        _gemText.text = CalUnit(Managers.Game.CurGem);
    }

    string CalUnit(int value)
    {
        string[] units = { "", "A", "B", "C", "D", "E", "F" };
        int count = 0;
        while(value > 1000)
        {
            value /= 1000;
            count++;
        }

        return value + units[count];
    }
    #endregion
}