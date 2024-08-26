using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// 動画管理クラス
/// </summary>
/// <typeparam name="T">インスタンス継承先(Movies/)</typeparam>
public class MovieManager<T> : MonoBehaviour where T : MovieManager<T>
{
    /// <summary>
    /// インスタンス格納フィールド変数
    /// </summary>
    private static T instance;

    /// <summary>
    /// 再生中フラグ
    /// </summary>
    protected bool isPlay;

    /// <summary>
    /// ビデオ配列
    /// </summary>
    [SerializeField] List<VideoPlayer> allVideos;

    /// <summary>
    /// インスタンス初期化
    /// </summary>
    public MovieManager()
    {
        instance = (T)this;
    }

    /// <summary>
    /// インスタンスの取得(シングルトンパターン適用)
    /// </summary>
    /// <returns>MovieManager系インスタンス</returns>
    public static T GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// 動画再生
    /// </summary>
    public virtual void PlayMovie() {}

    /// <summary>
    /// 指定IDで動画再生
    /// </summary>
    /// <param name="id">動画リストID</param>
    public virtual void PlayMovie(int id) {}
    
    /// <summary>
    /// 動画停止
    /// </summary>
    public virtual void StopMovie() {}

    /// <summary>
    /// 指定IDで動画停止
    /// </summary>
    /// <param name="id">動画リストID</param>
    public virtual void StopMovie(int id) {}

    /// <summary>
    /// 指定IDで動画スキップ
    /// </summary>
    /// <param name="id">動画リストID</param>
    public virtual void SkipMovie(int id) {}

    /// <summary>
    /// 動画音量設定
    /// </summary>
    public virtual void VolumeMovie() {}

}
