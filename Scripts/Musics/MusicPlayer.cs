using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音楽再生管理クラス
/// </summary>
public class MusicPlayer : MonoBehaviour
{
    /// <summary>
    /// シーン切替時Awakeで流れる
    /// -1:再生しない場合
    /// </summary>
    [SerializeField] int awake_bgmNum;

    /// <summary>
    /// シーン切替時Awakeで流れる
    /// -1:再生しない場合
    /// </summary>
    [SerializeField] int awake_seNum;

    /// <summary>
    /// 呼び出し最初に、設定したIDに対応したBGM、SEの再生
    /// </summary>
    private void Start()
    {
        if(awake_bgmNum != -1)
        {
            Debug.Log("BGM => AwakePlayStart");
            MusicManager.GetInstance().PlayBGM(awake_bgmNum);
        }
        if(awake_seNum != -1)
        {
            Debug.Log("SE => AwakePlayStart");
            MusicManager.GetInstance().PlaySE(awake_seNum);
        }
    }

    /// <summary>
    /// クリックしたときにSEを流す
    /// </summary>
    /// <param name="button_seNum">SEのID</param>
    public void OnPlayClick(int button_seNum)
    {
        Debug.Log("SE => ClickPlayStart");
        MusicManager.GetInstance().PlaySE(button_seNum);
    }
}
