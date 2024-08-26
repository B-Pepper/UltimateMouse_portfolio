using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Canvas内のPanel管理クラス
/// </summary>
public class PanelManager : MonoBehaviour
{

    /// <summary>
    /// インスタンス格納フィールド変数
    /// </summary>
    public static PanelManager panelManager;
    
    /// <summary>
    /// キャラクター選択パネル
    /// </summary>
    [SerializeField] GameObject csp;

    /// <summary>
    /// ゲーム前演出パネル
    /// </summary>
    [SerializeField] GameObject ba;

    /// <summary>
    /// 動画演出パネル
    /// </summary>
    [SerializeField] GameObject mp;

    /// <summary>
    /// ゲーム本編パネル
    /// </summary>
    [SerializeField] GameObject gp;

    /// <summary>
    /// 結果表示パネル
    /// </summary>
    [SerializeField] GameObject rp;

    /// <summary>
    /// 巨大化演出パネル
    /// </summary>
    [SerializeField] GameObject gvp;

    /// <summary>
    /// インスタンス初期化
    /// </summary>
    void Awake()
    {
        if (panelManager == null)
        {
            panelManager = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// インスタンスの取得(シングルトンパターン適用)
    /// </summary>
    /// <returns>PanelManagerインスタンス</returns>
    public static PanelManager GetInstance()
    {
        return panelManager;
    }

    /// <summary>
    /// パネル切り替え
    /// </summary>
    /// <param name="toActiveStatus">有効化パネル</param>
    /// <param name="toDisActiveStatus">無効化パネル</param>
    public void ChangePanel(GameStatus toActiveStatus, GameStatus toDisActiveStatus)
    {
        SearchPanel(toActiveStatus).SetActive(true);
        SearchPanel(toDisActiveStatus).SetActive(false);
    }

    /// <summary>
    /// GameStatusに対応したPanelの検索
    /// </summary>
    /// <param name="status">GameStatus列挙型</param>
    /// <returns>パネル</returns>
    public GameObject SearchPanel(GameStatus status)
    {
        switch (status)
        {
            case GameStatus.CharacterSelect:
                return csp;
            case GameStatus.BeforeAct:
                return ba;
            case GameStatus.Movie:
                return mp;
            case GameStatus.Game:
                return gp;
            case GameStatus.GiantVideo:
                return gvp;
            case GameStatus.Result:
                return rp;
            default:
                return csp;
        }
    }

}
