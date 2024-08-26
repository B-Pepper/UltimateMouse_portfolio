using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.UI;

/// <summary>
/// 音楽管理するクラス
/// </summary>
public class MusicManager : MonoBehaviour
{
    /// <summary>
    /// インスタンス格納フィールド変数
    /// </summary>
    private static MusicManager instance;

    /// <summary>
    /// audioMixer
    /// </summary>
    [SerializeField] AudioMixer audioMixer;
    
    /// <summary>
    /// BGM格納用配列
    /// </summary>
    [SerializeField] AudioClip[] allBGMs = null;

    /// <summary>
    /// SE格納用配列
    /// </summary>
    [SerializeField] AudioClip[] allSEs = null;

    /// <summary>
    /// 音量調節パネルの表示/非表示(true/false)
    /// </summary>
    private bool isSettingNow = false;
    
    /// <summary>
    /// 音楽を鳴らすComponent
    /// </summary>
    [SerializeField] AudioSource[] audioSources = null;
    
    /// <summary>
    /// 音量調節スライダー
    /// </summary>
    [SerializeField] Slider[] sliders = null;

    /// <summary>
    /// DontDestroy化するパネル
    /// </summary>
    [SerializeField] GameObject configPanel;
    
    /// <summary>
    /// コルーチンのインスタンスを格納
    /// </summary>
    private Coroutine bgmLoop = null;

    /// <summary>
    /// パネルをDontDestroy化しているか
    /// していない場合、初期BGM再生
    /// </summary>
    private void Start()
    {
        if(!Config.isMusicManagerDontDestroy)
        {
            DontDestroyOnLoad(this.gameObject);
            Config.isMusicManagerDontDestroy = true;
        }
        if (!Config.isConfigPanelDontDestroy)
        {
            DontDestroyOnLoad(configPanel);
            Config.isConfigPanelDontDestroy = true;
        }
        if (!Config.isPlayFirstBGM)
        {
            // 起動時のBGM再生
            Config.isLoop = true;
            MusicManager.GetInstance().PlayBGM(0);
            MusicManager.GetInstance().IsBGMLoop(0, Config.isLoop);
            MusicManager.GetInstance().InitSliders();
            Config.isPlayFirstBGM = true;
        }
    }

    /// <summary>
    /// インスタンス初期化
    /// </summary>
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// インスタンスの取得(シングルトンパターン適用)
    /// </summary>
    /// <returns>SoloGameManagerインスタンス</returns>
    public static MusicManager GetInstance()
    {
        if(instance == null)
        {
            instance = new MusicManager();
        }
        return instance;
    }

    /// <summary>
    /// IDに対応したBGMの再生
    /// </summary>
    /// <param name="id">設定したBGMのID</param>
    public void PlayBGM(int id)
    {
        Debug.Log("BGM => PlayStart");
        audioSources[0].PlayOneShot(allBGMs[id]);
    }


    /// <summary>
    /// IDに対応したSEの再生
    /// </summary>
    /// <param name="id">設定したSEのID</param>
    public void PlaySE(int id)
    {
        Debug.Log("SE => PlayStart");
        audioSources[1].PlayOneShot(allSEs[id]);
    }

    /// <summary>
    /// BGMの一時停止
    /// </summary>
    public void PauseBGM()
    {
        audioSources[0].Pause();
    }

    /// <summary>
    /// SEの一時停止
    /// </summary>
    public void PauseSE()
    {
        audioSources[1].Pause();
    }

    /// <summary>
    /// IDに対応したBGMの停止
    /// </summary>
    /// <param name="id">設定したBGMのID</param>
    public void StopBGM(int id)
    {
        audioSources[0].Stop();
    }

    /// <summary>
    /// IDに対応したSEの停止
    /// </summary>
    /// <param name="id">設定したSEのID</param>
    public void StopSE(int id)
    {
        audioSources[1].Stop();
    }

    /// <summary>
    /// IDに対応したBGMの取得
    /// </summary>
    /// <param name="id">設定したBGMのID</param>
    public AudioClip GetBGM(int id)
    {
        return allBGMs[id];
    }

    /// <summary>
    /// IDに対応したSEの取得
    /// </summary>
    /// <param name="id">設定したSEのID</param>
    public AudioClip GetSE(int id)
    {
        return allSEs[id];
    }

    /// <summary>
    /// BGMのループ設定
    /// </summary>
    /// <param name="id">曲の番号</param>
    /// <param name="loop">true:有効,false:無効</param>
    public void IsBGMLoop(int id, bool loop)
    {
        if(bgmLoop == null)
        {
            bgmLoop = StartCoroutine(LoopTime(id, MusicManager.GetInstance().GetBGM(id).length, loop));
        }
    }
    /// <summary>
    /// BGMのループを停止させる
    /// </summary>
    /// <param name="id">曲の番号</param>
    /// <param name="loop">true:有効,false:無効</param>
    public void IsStopBGMLoop(int id, bool loop)
    {
        if(bgmLoop != null)
        {
            StopBGM(id);
            StopCoroutine(bgmLoop);
            bgmLoop = null;
        }
    }

    /// <summary>
    /// SEのループ設定
    /// </summary>
    /// <param name="loop">true:有効,false:無効</param>
    public void IsSELoop(bool loop)
    {
        audioSources[1].loop = loop;
    }

    /// <summary>
    /// BGMループ処理
    /// </summary>
    /// <param name="id">曲の番号</param>
    /// <param name="time">曲の時間</param>
    /// <param name="loop">true:有効,false:無効</param>
    /// <returns></returns>
    private IEnumerator LoopTime(int id, float time, bool loop)
    {
        if(!loop)
        {
            StopBGM(id);
            yield break;
        }
        MusicManager.GetInstance().PlayBGM(id);
        yield return new WaitForSeconds(time);
        if(loop)
        {
            bgmLoop = StartCoroutine(LoopTime(id, time, loop));
        }
    }

    /// <summary>
    /// 音量調節パネルの表示/非表示(true/false)のプロパティ
    /// </summary>
    /// <value>フィールド変数 isSettingNow</value>
    public bool IsSettingNow
    {
        get { return isSettingNow; }
        set { isSettingNow = value; }
    }

    /// <summary>
    /// 音量のコンフィグパネルが開かれたときに呼び出し
    /// </summary>
    public void OnClickVolumePanel()
    {
        for(int i=0;i<sliders.Length;i++)
        {
            Debug.Log("登録スライダー:" + sliders[i]);
            // スライダーに音量をセット
            SetVolumeSlider(i);
            switch(i)
            {
                case 0:
                    // スライダーを動かしたときの処理を登録
                    sliders[i].onValueChanged.AddListener(UpdateBGMVolume);
                    break;
                case 1:
                    sliders[i].onValueChanged.AddListener(UpdateSEVolume);
                    Debug.Log("1を登録: "+i);
                    break;
                case 2:
                    sliders[i].onValueChanged.AddListener(UpdateMovieVolume);
                    Debug.Log("2を登録: "+i);
                    break;
                default :
                    break;
            }
        }
    }

    /// <summary>
    /// 音量をスライダーにセット
    /// </summary>
    /// <param name="i">スライダーID(0:BGM, 1:SE, 2:MOVIE)</param>
    private void SetVolumeSlider(int i)
    {
        switch (i)
            {
                case 0:
                    sliders[i].value = Config.VolumeBGM;
                    break;
                case 1:
                    sliders[i].value = Config.VolumeSE;
                    break;
                case 2:
                    sliders[i].value = Config.VolumeMovie;
                    break;
                default:
                    break;
            }
    }

    // 264行目にてデリゲートで利用しているため、三つに関数を分けています

    /// <summary>
    /// BGM音量を更新
    /// </summary>
    /// <param name="volume">音量</param>
    private void UpdateBGMVolume(float volume)
    {
        Config.VolumeBGM = volume;
        //-80~0に変換
        float volValue = Mathf.Clamp(Mathf.Log10(volume/5) * 20f,-80f,0f);
        //audioMixerに代入
        audioMixer.SetFloat("BGM",volValue);
    }

    /// <summary>
    /// SE音量を更新
    /// </summary>
    /// <param name="volume">音量</param>
    private void UpdateSEVolume(float volume)
    {
        Config.VolumeSE = volume;
        //-80~0に変換
        float volValue = Mathf.Clamp(Mathf.Log10(volume/5) * 20f,-80f,0f);
        //audioMixerに代入
        audioMixer.SetFloat("SE",volValue);
    }

    /// <summary> 
    /// Movie音量を更新
    /// </summary>
    /// <param name="volume">音量</param>
    private void UpdateMovieVolume(float volume)
    {
        Config.VolumeMovie = volume;
        //-80~0に変換
        float volValue = Mathf.Clamp(Mathf.Log10(volume/5) * 20f,-80f,0f);
        //audioMixerに代入
        audioMixer.SetFloat("Movie",volValue);
    }

    /// <summary>
    /// スライダー初期化
    /// </summary>
    public void InitSliders()
    {
        for(int i=0;i<sliders.Length;i++)
        {
            switch (i)
            {
                case 0:
                    Config.VolumeBGM = Config.INIT_VOLUME_BGM;
                    UpdateBGMVolume(Config.INIT_VOLUME_BGM);
                    break;
                case 1:
                    Config.VolumeSE = Config.INIT_VOLUME_SE;
                    UpdateSEVolume(Config.INIT_VOLUME_SE);
                    break;
                case 2:
                    Config.VolumeMovie = Config.INIT_VOLUME_MOVIE;
                    UpdateMovieVolume(Config.INIT_VOLUME_MOVIE);
                    break;
                default:
                    break;
            }
            SetVolumeSlider(i);
        }
    }
}
