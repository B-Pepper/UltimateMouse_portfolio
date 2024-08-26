using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// マウス拡大縮小エフェクトクラス
/// </summary>
public class EfResizeImage : MonoBehaviour
{
    /// <summary>
    /// インスタンス格納フィールド変数
    /// </summary>
    private static EfResizeImage instance;
    
    /// <summary>
    /// 拡大縮小中フラグ
    /// </summary>
    private static bool isResizing = false;

    /// <summary>
    /// MusicManagerを格納する変数
    /// </summary>
    MusicManager mm = MusicManager.GetInstance();

    void Start()
    {
    }

    void Update()
    {
    }

    /// <summary>
    /// インスタンスの取得(シングルトンパターン適用)
    /// </summary>
    /// <returns>EfResizeImageインスタンス</returns>
    public static EfResizeImage GetInstance()
    {
        if(!instance)
        {
            instance = new EfResizeImage();
        }
        return instance;
    }

    /// <summary>
    /// エフェクト実行
    /// </summary>
    /// <param name="obj">対象オブジェクト</param>
    /// <param name="scale">倍率</param>
    /// <param name="duration">オフセット秒数</param>
    public void AnimateImage(GameObject obj, float scale, float duration)
    {
        // リサイズしている途中
        if (isResizing)
        {
            return;
        }
        isResizing = true;

        // リサイズするオブジェクトがnull
        if (obj == null)
        {
            Debug.LogError("GameObject is null.");
            return;
        }

        // オブジェクトの画像コンポネントを取得
        Image image = obj.GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Image component not found on GameObject.");
            return;
        }

        // 等倍の大きさ、拡大したときの大きさ
        float originalScale = image.transform.localScale.x;
        float targetScale = originalScale * scale;
        SoloGameManager.GetInstance().settingSyutyu(0.7f);
        LeanTween.scale(obj, new Vector3(targetScale, targetScale, 0.05f), duration)
            // イージング(拡大終盤の拡大の速度)
            .setEaseInOutQuad()
            // Scaleを等倍に(LeanTweenのScaleメソッド終了時に呼び出し)
            .setOnComplete(() =>
            {
                LeanTween.scale(obj, new Vector3(originalScale, originalScale, 0.05f), duration)
                    .setEaseInOutQuad()
                    .setOnComplete(() => {
                        isResizing = false;
                        // ここで音を鳴らすメソッド呼び出し
                        mm.PlaySE(6);
                        SoloGameManager.GetInstance().settingSyutyu(0.2f);
                    });
            });
    }

    /// <summary>
    /// リサイズフラグの取得・設定
    /// </summary>
    /// <value>フィールド変数 isResizing</value>
    public bool IsResizing
    {
        get { return isResizing; }
        set { isResizing = value; }
    }
}
