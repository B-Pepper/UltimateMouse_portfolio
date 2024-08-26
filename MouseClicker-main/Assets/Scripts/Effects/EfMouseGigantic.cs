using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// クリック終了時にマウスが巨大化する演出の制御管理クラス
/// </summary>
public class EfMouseGigantic : MonoBehaviour
{
    /// <summary>
    /// 設定している背景画像
    /// </summary>
    [SerializeField] Image backGroundImage;

    /// <summary>
    /// 全ての背景画像
    /// </summary>
    [SerializeField] Sprite[] backGroundImages;

    /// <summary>
    /// マウスの画像をアタッチしているオブジェクト
    /// </summary>
    [SerializeField] GameObject mouseObject;

    /// <summary>
    /// マウス画像(Image)
    /// </summary>
    [SerializeField] Image mouseImage;
    
    /// <summary>
    /// 敵の画像をアタッチしているオブジェクト
    /// </summary>
    [SerializeField] GameObject enemyObject;
    
    /// <summary>
    /// 敵画像(Image)
    /// </summary>
    [SerializeField] Image enemyImage;

    /// <summary>
    /// マウスクリック回数
    /// </summary>
    private int clickNum;

    /// <summary>
    /// クリック数レベル
    /// </summary>
    private int level;

    /// <summary>
    /// X軸におけるマウス画像のもとサイズ
    /// </summary>
    float originalScale_x;

    /// <summary>
    /// Y軸におけるマウス画像のもとサイズ
    /// </summary>
    float originalScale_y;

    /// <summary>
    /// X軸におけるマウス画像の最大化したサイズ
    /// </summary>
    float targetScale_x;

    /// <summary>
    /// Y軸におけるマウス画像の最大化したサイズ
    /// </summary>
    float targetScale_y;

    /// <summary>
    /// X軸における敵画像のもとサイズ
    /// </summary>
    float e_originalScale_x;

    /// <summary>
    /// Y軸における敵画像のもとサイズ
    /// </summary>
    float e_originalScale_y;

    /// <summary>
    /// X軸における敵画像の縮小化したサイズ
    /// </summary>
    float e_targetScale_x;

    /// <summary>
    /// Y軸における敵画像の縮小化したサイズ
    /// </summary>
    float e_targetScale_y;

    /// <summary>
    /// 最大化する演出時に掛けていく係数
    /// </summary>
    [SerializeField] float m_coeffScale;

    /// <summary>
    /// 最大化する際にかかる時間
    /// </summary>
    [SerializeField] float time;

    /// <summary>
    /// 縮小化する演出時に掛けていく係数
    /// </summary>
    [SerializeField] float e_coeffScale;

    /// <summary>
    /// MusicManagerを格納する変数
    /// </summary>
    private MusicManager mm;

    /// <summary>
    /// 巨大化指標定数(レベル1)
    /// </summary>
    const int CLICKLEVEL_1 = 30;

    /// <summary>
    /// 巨大化指標定数(レベル2)
    /// </summary>
    const int CLICKLEVEL_2 = 50;

    /// <summary>
    /// 巨大化指標定数(レベル3)
    /// </summary>
    const int CLICKLEVEL_3 = 70;

    /// <summary>
    /// 巨大化指標定数(レベル4)
    /// </summary>
    const int CLICKLEVEL_4 = 100;

    /// <summary>
    /// マウス画像を設定
    /// 敵画像を設定
    /// クリック回数レベルを取得
    /// 背景画像設定
    /// 巨大化開始
    /// </summary>
    void Start()
    {
        mm = MusicManager.GetInstance();
        // マウス画像を設定
        mouseImage.sprite = GetMouseSprite();
        // 敵画像を設定
        enemyImage.sprite = GetEnemySprite();
        // クリック回数レベルを取得
        level = SetLevel(Config.clickNum);
        // 背景画像設定
        ChangeMouseBackGroundImage(0);
        // 巨大化開始
        StartCoroutine(Main(time,Config.clickNum));

        Debug.Log("EfMouseGigantic.Start() End");
    }

    /// <summary>
    /// 取得したマウスの画像番号に応じて画像を設定
    /// </summary>
    /// <returns>選択状態のマウスに対応するスプライト</returns>
    private Sprite GetMouseSprite()
    {
        // マウス画像を取得
        return Resources.Load<Sprite>("image/mice/IC0"+ (Config.mouseNum + 1) +"ab");
    }

    /// <summary>
    /// 取得した敵の画像番号に応じて画像を設定
    /// </summary>
    /// <returns>選択状態のマウスに対応するスプライト</returns>
    private Sprite GetEnemySprite()
    {
        // 敵画像を取得
        return Resources.Load<Sprite>("image/enemy/IS0"+ (Config.enemyNum + 6) +"01");
    }

    /// <summary>
    /// クリック数に対応したレベル帯の取得
    /// </summary>
    /// <param name="num">クリック回数</param>
    /// <returns>レベル帯</returns>
    private int SetLevel(int num)
    {
        if(0 <= num && num <= CLICKLEVEL_1)
        {
            return 0;
        }
        else if((CLICKLEVEL_1 + 1) <= num && num <= CLICKLEVEL_2)
        {
            return 1;
        }
        else if((CLICKLEVEL_2 + 1) <= num && num <= CLICKLEVEL_3)
        {
            return 2;
        }
        else if((CLICKLEVEL_3 + 1) <= num && num <= (CLICKLEVEL_4 - 1))
        {
            return 3;
        }
        else 
        {
            return 4;
        }
    }
    
    /// <summary>
    /// 背景画像が変わる
    /// </summary>
    /// <param name="bgNum">背景番号</param>
    private void ChangeMouseBackGroundImage(int bgNum)
    {
        backGroundImage.sprite = backGroundImages[bgNum];
    }

    /// <summary>
    /// マウス画像の目的拡大率の算出
    /// </summary>
    private void CalcMouseScale()
    {
        // 等倍の大きさ、拡大したときの大きさ
        originalScale_x = mouseImage.transform.localScale.x;
        originalScale_y = mouseImage.transform.localScale.y;
        targetScale_x = originalScale_x * m_coeffScale;
        targetScale_y = originalScale_y * m_coeffScale;
    }

    /// <summary>
    /// 敵画像の目的拡大率の算出
    /// </summary>
    private void CalcEnemyScale()
    {
        e_originalScale_x = enemyImage.transform.localScale.x;
        e_originalScale_y = enemyImage.transform.localScale.y;
        e_targetScale_x = e_originalScale_x * e_coeffScale;
        e_targetScale_y = e_originalScale_y * e_coeffScale;
    }

    /// <summary>
    /// マウスを巨大化するアニメーション
    /// </summary>
    private void ChangeGigantMouse()
    {
        Debug.Log("ChengeGigantMouse => Start");
        LeanTween.scale(mouseObject,
            new Vector2(targetScale_x, targetScale_y), time)
            // イージング(拡大終盤の拡大の速度)
            .setEaseInOutQuad();
    }

    /// <summary>
    /// マウスを通常化するアニメーション
    /// </summary>
    private void ChangeOriginalMouse()
    {
        Debug.Log("ChengeOriginalMouse => Start");
        // スケール縮小化(オリジナル化)
        LeanTween.scale(mouseObject, 
            new Vector2(originalScale_x, originalScale_y), 0)
            // イージング(拡大終盤の拡大の速度)
            .setEaseInOutQuad();
    }

    /// <summary>
    /// 敵画像をレベルによって縮小する
    /// </summary>
    private void ChangeSmallEnemy()
    {
        // スケール縮小化
        LeanTween.scale(enemyObject, 
            new Vector2(e_targetScale_x, e_targetScale_y), 0)
            // イージング(拡大終盤の拡大の速度)
            .setEaseInOutQuad();
        e_targetScale_x *= e_coeffScale;
        e_targetScale_y *= e_coeffScale;
    }

    /// <summary>
    /// マウス画像を巨大化、縮小化する
    /// </summary>
    /// <param name="time">処理待機時間</param>
    /// <param name="c_level">クリック数</param>
    /// <returns>コルーチン(待機時間)</returns>
    IEnumerator Main(float time, int c_level) {
        Debug.Log("Main => Start");
        CalcMouseScale();
        CalcEnemyScale();
        // クリックレベル0
        if(c_level <= CLICKLEVEL_1)
        {
            ChangeGigantMouse();
        }
        // クリックレベル1
        else if(CLICKLEVEL_1 < c_level && c_level <= CLICKLEVEL_2)
        {
            ChangeMouseBackGroundImage(0);
            mm.PlaySE(2);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(1);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
        }
        // クリックレベル2
        else if(CLICKLEVEL_2 < c_level && c_level <= CLICKLEVEL_3)
        {
            ChangeMouseBackGroundImage(0);
            mm.PlaySE(2);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(1);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(3);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(2);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(4);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
        }
        // クリックレベル3
        else if(CLICKLEVEL_3 < c_level && c_level <= CLICKLEVEL_4)
        {
            Debug.Log("LEVEL3 => Start");
            ChangeMouseBackGroundImage(0);
            mm.PlaySE(2);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(1);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(3);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(2);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(4);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(3);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(4);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
        }
        else
        {
            ChangeMouseBackGroundImage(0);
            mm.PlaySE(2);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(1);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(3);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(2);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(4);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
            ChangeMouseBackGroundImage(3);
            ChangeOriginalMouse();
            ChangeSmallEnemy();
            mm.PlaySE(4);
            ChangeGigantMouse();
            yield return new WaitForSecondsRealtime(time);
        }
        yield return new WaitForSecondsRealtime(1.0f);
        EndMouseGigantic();
    }

    /// <summary>
    /// リザルト画面へ遷移させる設定を行う
    /// </summary>
    private void EndMouseGigantic()
    {
        SoloGameManager.GetInstance().NowStatus = GameStatus.Result;
    }
}
