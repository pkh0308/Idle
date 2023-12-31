using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossPopUp : UI_PopUp
{
    #region 변수 및 열거형
    enum Texts
    {
        BossNameText,
        TimerText,
        HpRateText,
        CheerAnnounceText,
        CheerBtnText,
        CheerGuageText,
        RewardAnnounceText,
        Reward_GemText,
        Reward_GoldText,
        ExitBtnText
    }
    TextMeshProUGUI _timerText;
    TextMeshProUGUI _hpRateText;
    TextMeshProUGUI _cheerGuageText;
    TextMeshProUGUI _cheerBtnText;
    TextMeshProUGUI[] _dmgTexts;
    Transform _dmgTextParent;

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
        CheerGuageBarBg,
        Reward_GemIcon,
        Reward_GoldIcon
    }
    Image _boss;
    Image _bossHpBar;
    Image _cheerGuageBar;
    int cheerCount;
    int maxCheerCount;

    enum AnimVar
    {
        DoAttack,
        DoClear
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
        GetText((int)Texts.BossNameText).text = Managers.Game.CurBossData.BossName;
        GetText((int)Texts.Reward_GemText).text = Custom.CalUnit(Managers.Game.CurBossData.DropGem);
        GetText((int)Texts.Reward_GoldText).text = Custom.CalUnit(Managers.Game.CurBossData.DropGold);
        _timerText = GetText((int)Texts.TimerText);
        _hpRateText = GetText((int)Texts.HpRateText);
        _cheerGuageText = GetText((int)Texts.CheerGuageText);
        _cheerBtnText = GetText((int)Texts.CheerBtnText);

        _dmgTextParent = transform.GetChild(0).Find(ConstValue.DmgTextParent);
        TextPooling();

        // 이미지 초기화
        _bossHpBar = GetImage((int)Images.HpBar);
        _cheerGuageBar = GetImage((int)Images.CheerGuageBar);
        maxCheerCount = (int)_cheerGuageBar.rectTransform.sizeDelta.x;

        // 애니메이터 초기화
        _playerAnimator = GetImage((int)Images.Player).gameObject.GetComponent<Animator>();

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.CheerBtn).gameObject).BindEvent(Btn_OnClickCheer, EventType.OnPressed);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        // Bgm
        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_Boss);
        return true;
    }

    int _textLoadCount;
    void TextPooling()
    {
        _dmgTexts = new TextMeshProUGUI[10];
        _textLoadCount = 10;
        for (int i = 0; i < _dmgTexts.Length; i++)
        {
            Managers.Resc.InstantiateByIdx(ConstValue.DmgText, i, _dmgTextParent, (op, idx) =>
            {
                _dmgTexts[idx] = op.GetComponent<TextMeshProUGUI>();
                op.SetActive(false);
                TextCount();
            });
        }
    }

    void TextCount()
    {
        _textLoadCount--;
        if (_textLoadCount == 0)
            BossStageInit();
    }

    Coroutine _timerRoutine;
    Coroutine _combatRoutine;
    Coroutine _cheerRoutine;
    void BossStageInit()
    {
        string name = ConstValue.Boss + Managers.Game.CurBossData.BossId;
        Managers.Resc.Instantiate(name, transform, (op) => {
            _boss = op.GetComponent<Image>();
        });
        Managers.Game.InitBoss();

        UpdateHpBar();
        UpdateCheerGuage();
        _timerRoutine = StartCoroutine(Timer(Managers.Game.CurBossData.TimeLimit));
        _combatRoutine = StartCoroutine(Combat());
        _cheerRoutine = StartCoroutine(CheerUp());
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
        _playerAnimator.SetTrigger(AnimVar.DoAttack.ToString());
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

        StopAllCoroutines();
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
        _playerAnimator.SetTrigger(AnimVar.DoClear.ToString());
        yield return Managers.Wfs.GetWaitForSeconds(2.0f);

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

        cheerCount += ConstValue.CheerUpValue;
        UpdateCheerGuage();
        if (cheerCount >= maxCheerCount)
        {
            _cheerComplete = true;
            _cheerGuageText.text = $"응원 버프 발동! (공격력 x{ConstValue.CheerBuffRate})";
            _cheerBtnText.text = "응원중";
        }
    }

    IEnumerator CheerUp()
    {
        while (true)
        {
            if (cheerCount == 0)
            {
                yield return null;
                continue;
            }

            cheerCount--;
            UpdateCheerGuage();
            if(cheerCount == 0 && _cheerComplete)
            {
                _cheerComplete = false;
                _cheerGuageText.text = "응원 게이지";
                _cheerBtnText.text = "응원하기";
            }
            yield return null;
        }
    }

    Vector2 cheerGuageScale = Vector2.one;
    void UpdateCheerGuage()
    {
        cheerGuageScale.x = (float)cheerCount / maxCheerCount;
        _cheerGuageBar.rectTransform.localScale = cheerGuageScale;
    }
    #endregion

    #region 타이머
    IEnumerator Timer(int seconds)
    {
        while(seconds > 0)
        {
            seconds--;
            if(seconds == 10)
                _timerText.color = Color.red;

            _timerText.text = string.Format("{0:00}:{1:00}", seconds / 60, seconds % 60);
            yield return Managers.Wfs.GetWaitForSeconds(1.0f);
        }
        TimeOut();
    }

    void TimeOut()
    {
        StopCoroutine(_combatRoutine);
        Managers.UI.OpenPopUp<UI_BossClearPopUp>();
    }
    #endregion
}