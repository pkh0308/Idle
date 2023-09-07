using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using SliderEvent = UnityEngine.UI.Slider.SliderEvent;

public class UI_SettingsPopUp : UI_PopUp
{
    enum Texts
    {
        TitleText,
        BgmText,
        SfxText,
        BackToTitleText,
        BackToTitleBtnText,
        ExitBtnText
    }
    enum Buttons
    {
        BackToTitleBtn,
        ExitBtn
    }
    enum Sliders
    {
        BgmSlider,
        SfxSlider
    }

    Slider _bgmSlider;
    Slider _sfxSlider;

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        BindSlider(typeof(Sliders));

        _bgmSlider = GetSlider((int)Sliders.BgmSlider); 
        _bgmSlider.onValueChanged = OnSlideBgm();
        _sfxSlider = GetSlider((int)Sliders.SfxSlider);
        _sfxSlider.onValueChanged = OnSlideSfx();

        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.BackToTitleBtn).gameObject).BindEvent(Btn_OnClickBackToTitle);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        return true;
    }

    #region 버튼
    public void Btn_OnClickBackToTitle()
    {
        Managers.UI.OpenPopUp<UI_BackToTilePopUp>();
    }
    public void Btn_OnClickExit()
    {
        Managers.UI.ClosePopUp(this);
    }
    #endregion

    #region 슬라이더 핸들러
    SliderEvent OnSlideBgm()
    {
        SliderEvent se = new SliderEvent();
        se.AddListener((value) => { Managers.Sound.SetBgmVolume(value); });
        return se;
    }

    SliderEvent OnSlideSfx()
    {
        SliderEvent se = new SliderEvent();
        se.AddListener((value) => { Managers.Sound.SetSfxVolume(value); });
        return se;
    }
    #endregion
}