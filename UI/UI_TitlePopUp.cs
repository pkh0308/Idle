using UnityEngine;

public class UI_TitlePopUp : UI_PopUp
{
    enum Texts
    {
        TitleText,
        StartBtnText,
        LoadBtnText,
        ExitBtnText
    }
    enum Buttons
    {
        StartBtn,
        LoadBtn,
        ExitBtn
    }

    public override bool Init()
    {
        BindText(typeof(Texts));
        BindButton(typeof(Buttons));
        
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.StartBtn).gameObject).BindEvent(Btn_OnClickStart);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.LoadBtn).gameObject).BindEvent(Btn_OnClickLoad);
        Custom.GetOrAddComponent<UI_Base>(GetButton((int)Buttons.ExitBtn).gameObject).BindEvent(Btn_OnClickExit);

        Managers.Sound.PlayBgm(SoundManager.Bgms.Sound_Title);
        return true;
    }
    #region ¹öÆ°
    public void Btn_OnClickStart()
    {
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_BtnClick);
        Managers.UI.OpenPopUp<UI_NewStartPopUp>(typeof(UI_NewStartPopUp).Name, transform);
    }

    public void Btn_OnClickLoad()
    {
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_BtnClick);
        if (Managers.Game.LoadGame() == false)
            Managers.UI.OpenNotice(ConstValue.Notice_NoSaveData, transform);
    }

    public void Btn_OnClickExit()
    {
        Managers.Sound.PlaySfx(SoundManager.Sfxs.Sound_BtnClick);
        Managers.UI.OpenPopUp<UI_ExitPopUp>(typeof(UI_ExitPopUp).Name, transform);
    }
    #endregion
}