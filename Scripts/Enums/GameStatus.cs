/// <summary>
/// ゲーム状態列挙型
/// </summary>
public enum GameStatus
{
    /// <summary>
    /// キャラセレクト
    /// </summary>
    CharacterSelect = 0,
    
    /// <summary>
    /// 動画演出
    /// </summary>
    Movie           = 1,
    
    /// <summary>
    /// ゲーム本編
    /// </summary>
    Game            = 2,
    
    /// <summary>
    /// 結果表示
    /// </summary>
    Result          = 3,
    
    /// <summary>
    /// マウス巨大化
    /// </summary>
    GiantVideo      = 4,

    /// <summary>
    /// ゲーム前演出
    /// </summary>
    BeforeAct = 5
}