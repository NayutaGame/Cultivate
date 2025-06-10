
using CLLibrary;
using FMODUnity;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.EventSystems;

public class AudioManager : Singleton<AudioManager>
{
    private Bus MasterBus;
    private Bus MusicBus;
    private Bus SFXBus;

    protected override void AwakeFunction()
    {
        base.AwakeFunction();
        
        MasterBus = RuntimeManager.GetBus("bus:/");
        MusicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private AudioEntry PlayingEntry;
    private EventInstance BGMEventInstance;

    public static void Play(AudioEntry audioEntry)
    {
        if (audioEntry._audioType == AudioEntry.AudioType.Music)
        {
            Instance.PlayMusic(audioEntry);
        }
        else if (audioEntry._audioType == AudioEntry.AudioType.SFX)
        {
            Instance.PlaySFX(audioEntry);
        }
    }

    public void Stop()
    {
        BGMEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void PlaySFX(AudioEntry audioEntry)
    {
        RuntimeManager.PlayOneShot(audioEntry.EventReference);
    }

    private void PlayMusic(AudioEntry audioEntry)
    {
        if (PlayingEntry == audioEntry)
            return;

        Stop();
        BGMEventInstance.release();
        PlayingEntry = audioEntry;
        BGMEventInstance = RuntimeManager.CreateInstance(audioEntry.EventReference);
        BGMEventInstance.start();
    }

    public static float GetMasterVolume()
    {
        Instance.MasterBus.getVolume(out float volume);
        return volume * 100;
    }

    public static void SetMasterVolume(int value)
    {
        Instance.MasterBus.setVolume((float)value / 100);
    }

    public static float GetMusicVolume()
    {
        Instance.MusicBus.getVolume(out float volume);
        return volume * 100;
    }

    public static void SetMusicVolume(int value)
    {
        Instance.MusicBus.setVolume((float)value / 100);
    }

    public static float GetSFXVolume()
    {
        Instance.SFXBus.getVolume(out float volume);
        return volume * 100;
    }

    public static void SetSFXVolume(int value)
    {
        Instance.SFXBus.setVolume((float)value / 100);
    }

    #region ShortCuts
    
    public static void PlayButtonPress()
        => Play("ButtonPress");
    
    public static void PlayButtonPress(InteractBehaviour ib, PointerEventData d)
        => Play("ButtonPress");

    public static void PlayButtonHover(PointerEventData d)
        => Play("ButtonHover");

    public static void PlayButtonHover(InteractBehaviour ib, PointerEventData d)
        => Play("ButtonHover");

    public static void PlayCardHover(InteractBehaviour ib, PointerEventData d)
        => Play("CardHover");

    public static void PlayCardPlacement()
        => Play("CardPlacement");

    public static void PlayFinishGuide(Guide guide)
        => Play("FinishGuide");

    public static void PlayItemHover(PointerEventData d)
        => Play("ItemHover");

    public static void PlayItemHover()
        => Play("ItemHover");

    public static void PlayEnterSettings()
        => Play("EnterSettings");

    public static void PlayExitSettings()
        => Play("ExitSettings");

    public static void PlayGainMingYuan(int value)
        => Play("GainMingYuan");
    
    public static void PlayLoseMingYuan(int value)
        => Play("LoseMingYuan");
    
    public static void PlayGainMaxHealth(int value)
        => Play("GainMaxHealth");
    
    public static void PlayGainGold(int value)
        => Play("GainGold");
    
    public static void PlayLoseGold(int value)
        => Play("LoseGold");
    
    public static void PlayShowDiscovered()
        => Play("ShowDiscovered");
    
    public static void PlayGainSkill()
        => Play("GainSkill");

    #endregion
}
