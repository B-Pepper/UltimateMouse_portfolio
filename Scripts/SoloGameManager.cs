using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// ヒトリデモードのゲームを管理するクラス
/// </summary>
public class SoloGameManager : MonoBehaviour
{
    /// <summary>
    /// インスタンス格納フィールド変数
    /// </summary>
    private static SoloGameManager soloGameManager;

    /// <summary>
    /// ゲーム本編開始フラグ
    /// </summary>
    private bool isStartGame = false;

    /// <summary>
    /// ゲーム本編終了フラグ
    /// </summary>
    private bool isFinishGame = false;

    /// <summary>
    /// クリック中フラグ
    /// </summary>
    private bool isClicking = false;

    /// <summary>
    /// 集中線素材
    /// </summary>
    [SerializeField] GameObject syutyu;

    /// <summary>
    /// 現在のゲーム状態を管理する列挙型 
    /// </summary>
    [SerializeField] GameStatus gameStatus;

    /// <summary>
    /// 時間制限
    /// </summary>
    [SerializeField] float timeLimit;

    /// <summary>
    /// カウントダウン定数
    /// </summary>
    [SerializeField] float countdownTime;

    /// <summary>
    /// 開始カウントダウン画像
    /// </summary>
    [SerializeField] Sprite[] StartCountdownImages;

    /// <summary>
    /// 終了カウントダウン画像
    /// </summary>
    [SerializeField] Sprite[] FinishCountDownImages;

    /// <summary>
    /// タイマ用変数
    /// </summary>
    [SerializeField] float timer;

    /// <summary>
    /// ムービーを再生するコンポーネント
    /// </summary>
    [SerializeField] VideoPlayer videoPlayer;


    /// <summary>
    /// 勝敗画像
    /// </summary>
    [SerializeField] Sprite[] ResultImages;
    
    /// <summary>
    /// マウス画像素材
    /// </summary>
    [SerializeField] Image gameMouseImage;

    /// <summary>
    /// 敵画像素材
    /// </summary>
    [SerializeField] Image gameEnemyImage;

    /// <summary>
    /// 結果画像素材
    /// </summary>
    [SerializeField] Image resultMouseImage;
    
    /// <summary>
    /// 敵オブジェクト
    /// </summary>
    [SerializeField] GameObject[] gameEnemyObjects;

    /// <summary>
    /// Movie関連の処理を一度行ったかどうか
    /// </summary>
    private bool doneMovie = false;

    /// <summary>
    /// ゲーム本編BGM1回実行フラグ
    /// </summary>
    private bool doneGameBGM = false;
    
    /// <summary>
    /// リザルトBGM1回実行フラグ
    /// </summary>
    private bool doneResBGM = false;

    /// <summary>
    /// マウスセレクトBGM1回実行フラグ
    /// </summary>
    private bool doneSelectBGM = false;


    /// <summary>
    /// 敵番号決定・インスタンス初期化
    /// </summary>
    void Awake()
    {
        // ランダムに整数を生成
        Config.enemyNum = Random.Range(0, 3);
        if (soloGameManager == null)
        {
            Debug.Log("soloGameManager生成");
            soloGameManager = this;
        }
    }

    /// <summary>
    /// 最初は開始のカウントダウン秒数をタイマに入れておく
    /// </summary>
    void Start()
    {
        timer = countdownTime;
    }

    /// <summary>
    /// gameStatusによる処理の振り分け
    /// </summary>
    void Update()
    {
        switch (gameStatus)
        {
            case GameStatus.CharacterSelect:
                if(!doneSelectBGM)
                {
                    // 一度だけBGMを再生
                    MusicManager.GetInstance().IsStopBGMLoop(0, Config.isLoop);
                    MusicManager.GetInstance().IsBGMLoop(1, Config.isLoop);
                    doneSelectBGM = true;
                }
                break;
            case GameStatus.Movie:
                Debug.Log("GameStatus.Movie => Start");
                PanelManager.GetInstance().ChangePanel(GameStatus.Movie, GameStatus.CharacterSelect);
                if (!doneMovie)
                {
                    doneMovie = true;
                    MusicManager.GetInstance().StopBGM(1);
                    // カメラの前にレンダリングするようにする
                    // videoPlayer.renderMode = VideoRenderMode.CameraNearPlane;
                    // 再生ムービー決定
                    videoPlayer.clip = GetMovie(GetRandomMovieNum());
                    // ムービーが終了したときの処理
                    videoPlayer.loopPointReached += EndReached;
                    // ムービーの音量設定
                    SetMovieVol();
                    // ムービーの再生
                    videoPlayer.Play();
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    EndReached(videoPlayer);
                }
                break;
            case GameStatus.Game:
                if (!doneGameBGM)
                {
                    doneGameBGM = true;
                    MusicManager.GetInstance().IsStopBGMLoop(1, Config.isLoop);
                    MusicManager.GetInstance().PlayBGM(2);
                }
                // 敵画像設定
                GameObject enemyAnimObj = gameEnemyObjects[Config.enemyNum];
                SpriteRenderer sR = enemyAnimObj.GetComponent<SpriteRenderer>();
                Color enemyColor = sR.color;
                enemyColor.a = 1.0f;
                sR.color = enemyColor;
                // settingCharacterImage(gameEnemyImage, "image/enemy/IS0" + (Config.enemyNum + 6) + "01");

                // マウス画像設定
                settingCharacterImage(gameMouseImage, "image/mice/IC0" + (Config.mouseNum + 1) + "ab");
                GameObject CountDownImage = GameObject.FindGameObjectWithTag("GamePanel").transform.Find("CountDownImage").gameObject;
                // 始めにカウントダウン表示
                if (!isStartGame && !isFinishGame && timer > 0f)
                {
                    // カウントダウンを表示
                    ShowCount(1, countdownTime, ref CountDownImage, StartCountdownImages);
                    // タイマをデルタ時間減算
                    timer -= Time.deltaTime;
                }
                // カウントダウンが終わったとき、ゲーム開始
                else if (!isStartGame && !isFinishGame)
                {
                    CountDownImage.SetActive(false);
                    GameStart();
                }
                // ゲーム中
                else if (isStartGame && !isFinishGame)
                {
                    if (timer > 0f)
                    {
                        // カウントダウンを表示
                        ShowCount(11, timeLimit, ref CountDownImage, FinishCountDownImages);
                        // プレイ中にマウスクリックしたときの挙動
                        CheckMouseClick();

                        // タイマをデルタ時間減算
                        timer -= Time.deltaTime;
                    }
                    else
                    {
                        Config.clickNum = BasicEnemy.GetInstance().Count;
                        // ゲームFinishi時のBGM
                        //MusicManager.GetInstance().PlayBGM(0);
                        GameFinish();
                    }

                }
                // フィニッシュ時の動作
                else if (isFinishGame)
                {
                    if (timer > 0f)
                    {
                        // 2秒なんにもしない余韻
                        timer -= Time.deltaTime;
                    }
                    else
                    {
                        // 巨大化演出に遷移
                        gameStatus = GameStatus.GiantVideo;
                    }
                }
                break;
            case GameStatus.GiantVideo:
                PanelManager.GetInstance().ChangePanel(GameStatus.GiantVideo, GameStatus.Game);
                break;
            case GameStatus.Result:
                if (!doneResBGM)
                {
                    doneResBGM = true;
                    MusicManager.GetInstance().StopBGM(2);
                    MusicManager.GetInstance().IsBGMLoop(3, Config.isLoop);
                }
                PanelManager.GetInstance().ChangePanel(GameStatus.Result, GameStatus.GiantVideo);
                int clickCount = BasicEnemy.GetInstance().Count;
                settingCharacterImage(resultMouseImage, "image/mice/IC0" + (Config.mouseNum + 1) + "ab");

                int enemyHP = BasicEnemy.GetInstance().HP;
                if (enemyHP - clickCount > 0)
                {
                    // ここで負けBGM・SEを鳴らす
                    GameObject.FindGameObjectWithTag("IsWinImage").gameObject.GetComponent<Image>().sprite = ResultImages[1];
                    GameObject.FindGameObjectWithTag("ResultText").gameObject.GetComponent<Text>().text = "つぎは まけない...！";
                }
                else
                {
                    // ここで勝ちBGM・SEを鳴らす
                    GameObject.FindGameObjectWithTag("IsWinImage").gameObject.GetComponent<Image>().sprite = ResultImages[0];
                    GameObject.FindGameObjectWithTag("ResultText").gameObject.GetComponent<Text>().text = "キミの おかげで まちは すくわれた！";
                }
                GameObject.FindGameObjectWithTag("SizeText").GetComponent<Text>().text = "おおきさ " + (float)(clickCount * clickCount / 100) + "メートル";

                break;
            default:
                break;
        }
    }

    /// <summary>
    /// インスタンスの取得(シングルトンパターン適用)
    /// </summary>
    /// <returns>SoloGameManagerインスタンス</returns>
    public static SoloGameManager GetInstance()
    {
        return soloGameManager;
    }

    /// <summary>
    /// カウント系の表示をする
    /// 要素以上の秒数にならないように注意
    /// </summary>
    /// <param name="num">空白秒数指定(空白秒数＋カウント表示となる)</param>
    /// <param name="countTime">カウント定数</param>
    /// <param name="obj">表示させたい参照オブジェクト</param>
    /// <param name="sprites">表示するスプライト配列</param>
    private void ShowCount(int num, float countTime, ref GameObject obj, Sprite[] sprites)
    {
        int count = (int)(countTime - timer) - num;

        if (count >= 0)
        {
            obj.SetActive(true);
            // Debug.Log(count);
            obj.GetComponent<Image>().sprite = sprites[count];
            // 表示スクリプト
        }
    }


    /// <summary>
    /// マウスがクリックされているかどうかの判別及びその場合の挙動
    /// </summary>
    public void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject mouse = GameObject.FindGameObjectWithTag("MouseImage");
            //clickCount++;
            BasicMouse.GetInstance().Attack();
            //Debug.Log("現在のカウント数: " + clickCount);
            EfResizeImage.GetInstance().AnimateImage(mouse, 1.2f, 0.2f);
        }
    }

    /// <summary>
    /// ゲーム開始時の挙動
    /// 1. ゲーム開始フラグを立てる
    /// 2. タイマの初期化設定
    /// </summary>
    public void GameStart()
    {
        // ゲーム開始フラグを立てる
        isStartGame = true;
        timer = timeLimit;
        Debug.Log("ゲーム開始");
    }

    /// <summary>
    /// ゲーム終了時の挙動
    /// 1. ゲーム開始フラグを折る
    /// 2. ゲーム終了フラグを立てる
    /// 3. タイマの初期化設定
    /// </summary>
    public void GameFinish()
    {
        isStartGame = false;
        isFinishGame = true;
        timer = 2f;
    }

    /// <summary>
    /// 再生するムービのゲッター
    /// </summary>
    /// <param name="i">ムービの種類(番号)</param>
    /// <returns></returns>
    private VideoClip GetMovie(int i)
    {
        return Resources.Load<VideoClip>("movie/MOVIE_" + i);
    }

    /// <summary>
    /// ムービーの番号をランダムに返却する
    /// </summary>
    /// <returns>0~2の整数</returns>
    private int GetRandomMovieNum()
    {
        return Config.enemyNum;
    }

    /// <summary>
    /// 動画再生終了時のコールバック関数
    /// </summary>
    /// <param name="vp">動画を再生するコンポーネント</param>
    private void EndReached(VideoPlayer vp)
    {
        // Gameステートに遷移
        gameStatus = GameStatus.Game;
        // GamePanelをActive化
        PanelManager.GetInstance().ChangePanel(GameStatus.Game, GameStatus.Movie);
        // カメラのムービーレンダリングテクスチャを透明化
        videoPlayer.targetCameraAlpha = 0F;
        // ムービー関連処理終了
        doneMovie = false;
        // ボリュームを0にする
        videoPlayer.Stop();
    }

    /// <summary>
    /// 動画音量の設定
    /// </summary>
    private void SetMovieVol()
    {
        videoPlayer.SetDirectAudioVolume (0 , Config.VolumeMovie/5);
    }

    /// <summary>
    /// 画像コンポーネントにパスに対応する画像を割り当てる
    /// </summary>
    /// <param name="imageComponent">任意の画像コンポーネント</param>
    /// <param name="path">画像パス(Resources以下)</param>
    private void settingCharacterImage(Image imageComponent, string path)
    {
        Sprite newSprite = Resources.Load<Sprite>(path);
        imageComponent.sprite = newSprite;
    }

    /// <summary>
    /// 集中線の透明度を設定する
    /// </summary>
    /// <param name="alpha">透明度</param>
    public void settingSyutyu(float alpha)
    {
        Color color = syutyu.GetComponent<Image>().color;
        color.a = alpha;
        syutyu.GetComponent<Image>().color = color;
    }

    /// <summary>
    /// ステータス列挙型のプロパティ
    /// </summary>
    /// <value>フィールド変数 gameStatus</value>
    public GameStatus NowStatus
    {
        get { return gameStatus; }
        set { gameStatus = value; }
    }

}
