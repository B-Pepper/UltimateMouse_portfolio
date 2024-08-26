using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// タイトルシーンで動画を再生
/// </summary>
public class TitleMovie : MovieManager<TitleMovie>
{
    /// <summary>
    /// アイドル状態とみなす秒数
    /// </summary>
    [SerializeField] float idleTime = 15f;
    /// <summary>
    /// メインパネル
    /// </summary>
    [SerializeField] GameObject mainPanel;
    /// <summary>
    /// 動画を再生するパネル
    /// </summary>
    [SerializeField] GameObject videoPanel; 
    /// <summary>
    /// 前フレームのマウス位置
    /// </summary>
    private Vector3 mousePosition;
    /// <summary>
    /// アイドル状態の時間経過
    /// </summary>
    [SerializeField] float timeElapsed;

    /// <summary>
    /// マウスポジションの初期化、放置時間の初期化、再生フラグの初期化
    /// </summary>
    void Awake()
    {
        mousePosition = Input.mousePosition;
        timeElapsed = 0f;
        isPlay = false;
    }

    /// <summary>
    /// キー入力、マウス動作があったときや、設定画面を開いているときはムービーを再生しない
    /// </summary>
    void Update()
    {
        // キー入力、マウス動作があったときや、設定画面を開いているときはムービーを再生しない
        if (Input.anyKeyDown || Input.mousePosition != mousePosition || MusicManager.GetInstance().IsSettingNow)
        {
            // アイドル状態の時間をリセットする
            timeElapsed = 0f;
            mousePosition = Input.mousePosition;
            isPlay = false;
            StopMovie();
        }
        else if (!isPlay)
        {
            // アイドル状態の時間を更新する
            timeElapsed += Time.deltaTime;
            // アイドル状態が一定時間以上続いた場合、動画を再生する
            if (timeElapsed >= idleTime)
            {
                isPlay = true;
                PlayMovie();
            }
        }
        else
        { }
    }

    /// <summary>
    /// パネルを表示することで動画を再生
    /// </summary>
    public override void PlayMovie() {
        //Debug.Log("PlayMovie");
        mainPanel.SetActive(false);
        videoPanel.SetActive(true);
    }

    /// <summary>
    /// パネルを非表示にすることで動画を停止
    /// </summary>
    public override void StopMovie() {
        //Debug.Log("StopMovie");
        mainPanel.SetActive(true);
        videoPanel.SetActive(false);
    }

}
