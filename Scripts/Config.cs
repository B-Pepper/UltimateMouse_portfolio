/// <summary>
/// ゲーム内情報を保持しておくクラス
/// </summary>
public static class Config 
{

    /// <summary>
    /// BGM音量保持変数
    /// </summary>
    private static float volumeBGM = 2.5f;
    
    /// <summary>
    /// SE音量保持変数
    /// </summary>
    private static float volumeSE = 2.5f;
    
    /// <summary>
    /// 動画音量保持変数
    /// </summary>
    private static float volumeMovie = 2.5f;
    
    /// <summary>
    /// BGM音量の定数初期値
    /// </summary>
    public const float INIT_VOLUME_BGM = 2.5f;
    
    /// <summary>
    /// SE音量の定数初期値
    /// </summary>
    public const float INIT_VOLUME_SE = 2.5f;
    
    /// <summary>
    /// MOVIE音量の定数初期値
    /// </summary>
    public const float INIT_VOLUME_MOVIE = 2.5f;
    
    /// <summary>
    /// 選択したマウスの保持変数
    /// </summary>
    private static EMouseType selectedMouse;
    
    /// <summary>
    /// マウスが選択状態か管理する
    /// </summary>
    private static bool isSelected = false;

    /// <summary>
    /// 現在のBGM音量の取得・設定
    /// </summary>
    /// <value>フィールド変数 volumeBGM</value>
    public static float VolumeBGM
    {
        get { return volumeBGM; }
        set { volumeBGM = value; }
    }
    
    /// <summary>
    /// 現在のSE音量の取得・設定
    /// </summary>
    /// <value>フィールド変数 volumeSE</value>
    public static float VolumeSE
    {
        get { return volumeSE; }
        set { volumeSE = value; }
    }
    
    /// <summary>
    /// 現在の動画音量の取得・設定
    /// </summary>
    /// <value>フィールド変数 volumeMovie</value>
    public static float VolumeMovie
    {
        get { return volumeMovie; }
        set { volumeMovie = value; }
    }
    
    /// <summary>
    /// 選択したマウスの取得・設定
    /// </summary>
    /// <value>フィールド変数 selectedMouse</value>
    public static EMouseType SelectedMouse
    {
        get { return selectedMouse; }
        set { selectedMouse = value; }
    }

    /// <summary>
    /// 選択したマウスの保持変数(DuoGame用)
    /// [0]:1P
    /// [1]:2P
    /// </summary>
    public static EMouseType[] duoSelectedMouse
    { get; set; } = { EMouseType.None, EMouseType.None};

    /// <summary>
    /// 音楽のループ状態
    /// </summary>
    /// <value>初回falseが返る</value>
    public static bool isLoop
    { get; set; } = false;

    /// <summary>
    /// マウスの選択状態の取得・設定
    /// </summary>
    /// <value>フィールド変数 isSelected</value>
    public static bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    /// <summary>
    /// MusicManagerのDontDestroyであるかどうかの取得・設定
    /// </summary>
    /// <value>初回falseが返る</value>
    public static bool isMusicManagerDontDestroy
    { get; set; } = false;

    /// <summary>
    /// ConfigPanelのDontDestroyであるかどうかの取得・設定
    /// </summary>
    /// <value>初回falseが返る</value>
    public static bool isConfigPanelDontDestroy
    { get; set; } = false;

    /// <summary>
    /// 最初のBGMを再生したかどうかの取得・設定
    /// </summary>
    /// <value>初回falseが返る</value>
    public static bool isPlayFirstBGM
    { get; set; } = false;

    /// <summary>
    /// クリック数の取得・設定
    /// </summary>
    /// <value>フィールド変数 clickNum</value>
    public static int clickNum
    { get; set; }

    /// <summary>
    /// マウス番号の取得・設定
    /// </summary>
    /// <value>フィールド変数 mouseNum</value>
    public static int mouseNum
    { get; set; }

    /// <summary>
    /// 敵番号の取得・設定
    /// </summary>
    /// <value>フィールド変数 enemyNum</value>
    public static int enemyNum
    { get; set; }

    /// <summary>
    /// DuoGameでのマウス番号の取得・設定
    /// [0]:1P
    /// [1]:2P
    /// </summary>
    /// <value>フィールド変数 duoMouseNum</value>
    public static int[] duoMouseNum
    { get; set; } = { -1, -1 };
    
}