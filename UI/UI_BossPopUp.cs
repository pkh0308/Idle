using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossPopUp : UI_PopUp
{
    #region 변수 및 열거형
    enum Texts
    {
        BossNameText,
        NickNameText,
        HpRateText,
        CheerAnnounceText,
        CheerBtnText,
        CheerGuageText,
        RewardAnnounceText,
        Reward_GemText,
        Reward_GoldText,
        ExitBtnText
    }
    TextMeshProUGUI _hpRateText;
    TextMeshProUGUI _cheerGuageText;
    TextMeshProUGUI[] _dmgTexts;
    const string Cheering = "응원 게이지";

    enum Buttons
    {
        CheerBtn,
        ExitBtn
    }
    enum Images
    {
        Player,
        HpBar,
        CheerGuageBar,
        Reward_GemIcon,
        Reward_GoldIcon
    }
    Image _boss;
    Image _bossHpBar;
    Image _cheerGuageBar;

    enum AnimVar
    {
        OnCombat,
        DoAttack,
        DoNextStage,
        DoBossAttack
    }
    Animator _playerAnimator;
    #endregion

    #region 초기화
    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindImage(typeof(Images));

        // 텍스트 초기화
        GetText((int)Texts.NickNameText).text = Managers.Game.NickName;
        GetText((int)Texts.BossNameText).text = Managers.Game.CurBossData.BossName;
        GetText((int)Texts.Reward_GemText).text = Custom.CalUnit(Managers.Game.CurBossData.DropGem);
        GetText((int)Texts.Reward_GoldText).text = Custom.CalUnit(Managers.Game.CurBossData.DropGold);
        _hpRateText = GetText((int)Texts.HpRateText);
        _cheerGuageText = GetText((int)Texts.CheerGuageText);
        TextPooling();

        // 이미지 초기화
        _bossHpBar = GetImage((int)Images.HpBar);
        _cheerGuageBar = GetImage((int)Images.CheerGuageBar);

        // 애니메이터 초기화
        _playerAnimator = GetImage((int)Images.Player).gameObject.GetComponent<Animator>();

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CheerBtn).gameObject).BindEvent(Btn_OnClickCheer, EventType.OnPressed);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        // Bgm
        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_Boss);
        BossStageInit();
        return true;
    }

    Coroutine _CombatRoutine;
    void BossStageInit()
    {
        string name = ConstValue.Boss + Managers.Game.CurBossData.BossId;
        Managers.Resc.Instantiate(name, transform, (op) => {
            _boss = op.GetComponent<Image>();
        });
        Managers.Game.InitBoss();

        UpdateHpBar();
        UpdateCheerGuage();
        _CombatRoutine = StartCoroutine(Combat());
    }

    void TextPooling()
    {
        _dmgTexts = new TextMeshProUGUI[100];
        for(int i = 0; i < _dmgTexts.Length; i++)
        {
            Managers.Resc.InstantiateByIdx(ConstValue.DmgText, i, transform, (op, idx) =>
            {
                _dmgTexts[idx] = op.GetComponent<TextMeshProUGUI>();
                op.SetActive(false);
            });
        }
    }
    #endregion

    #region 버튼
    public void Btn_OnClickCheer()
    {
        CheerGuageUp();
    }

    public void Btn_OnClickExit()
    {
        Managers.UI.OpenPopUp<UI_BossExitPopUp>();
    }
    #endregion

    #region 전투 
    int _curDelay;
    IEnumerator Combat()
    {
        _curDelay = 0;
        while (true)
        {
            if (_curDelay >= ConstValue.MaxAttackDelay)
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

    void Attack()
    {
        _playerAnimator.SetTrigger(AnimVar.DoBossAttack.ToString());
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_Attack, 2.0f);
        int dmg = Managers.Game.Attack(out bool critical, _cheerComplete);
        ShowDmgText(dmg, critical);

        // 체력바 갱신
        float hpRate = UpdateHpBar();
        if (hpRate <= 0)
        {
            OnBossDie();
            return;
        }
    }

    void OnBossDie()
    {
        string name = ConstValue.Sprite_BossDie + Managers.Game.CurBossData.BossId;
        Managers.Resc.Load<Sprite>(name, (op) =>
        {
            _boss.sprite = op;
        });
        Managers.Game.DefeatBoss();
        StopCoroutine(_CombatRoutine);
        StartCoroutine(BossDisappear());
    }

    const float DisappearColor = 0.05f;
    const float DisappearCycle = 0.1f;
    IEnumerator BossDisappear()
    {
        Color curColor = _boss.color;
        while(_boss.color.a > 0)
        {
            curColor.a -= DisappearColor;
            _boss.color = curColor;
            yield return Managers.Wfs.GetWaitForSeconds(DisappearCycle);
        }
        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_BossClear);
        Managers.UI.OpenPopUp<UI_BossClearPopUp>();
    }
    #endregion

    #region UI 갱신
    Vector2 hpBarScale = Vector2.one;
    float UpdateHpBar()
    {
        float hpRate = (float)Managers.Game.EnemyHp / Managers.Game.MaxEnemyHp;
        hpBarScale.x = hpRate;
        _bossHpBar.rectTransform.localScale = hpBarScale;
        _hpRateText.text = string.Format("{0:0.00}%", hpRate * 100);
        return hpRate;
    }

    Vector2 cheerGuageScale = Vector2.one;
    int cheerCount = 0;
    int maxCheerCount = 1000;
    void UpdateCheerGuage()
    {
        cheerGuageScale.x = (float)cheerCount / maxCheerCount;
        _cheerGuageBar.rectTransform.localScale = cheerGuageScale;
    }

    void ShowDmgText(int value, bool critical)
    {
        TextMeshProUGUI dmgText = null;
        // 비활성 상태인 텍스트 찾기
        for(int i = 0; i < _dmgTexts.Length; i++) 
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

    #region 응원 게이지
    bool _cheerComplete = false;
    void CheerGuageUp()
    {
        if (_cheerComplete)
            return;

        cheerCount += ConstValue.CheerValue;
        UpdateCheerGuage();
        if (cheerCount >= maxCheerCount)
        {
            _cheerComplete = true;
            _cheerGuageText.text = $"응원 버프 발동! (공격력 x{ConstValue.CheerValue})";
            StartCoroutine(CheerUp());
        }
    }

    IEnumerator CheerUp()
    {
        while(cheerCount > 0)
        {
            cheerCount--;
            UpdateCheerGuage();
            yield return null;
        }
        _cheerComplete = false;
        _cheerGuageText.text = Cheering;
    }
    #endregion
}