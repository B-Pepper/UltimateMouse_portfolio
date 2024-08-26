using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// たぶん使ってないけど、パネル切り替えで動画再生するやつ
/// MovieManagerを継承
/// </summary>
public class LoadMovie : MovieManager<LoadMovie>
{
    /// <summary>
    /// メインパネル
    /// </summary>
    [SerializeField] GameObject mainPanel;
    
    /// <summary>
    /// 動画用のパネルオブジェクト
    /// </summary>
    [SerializeField] GameObject videoPanel;

    /// <summary>
    /// Movieパネルを有効化
    /// </summary>
    public override void PlayMovie()
    {
        videoPanel.SetActive(true);
    }

    /// <summary>
    /// Movieパネルを非表示
    /// </summary>
    public override void StopMovie()
    {
        videoPanel.SetActive(false);
    }

}
