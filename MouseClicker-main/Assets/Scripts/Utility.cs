using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 計算処理、ファイル入出力系を管理するクラス
/// </summary>
public static class Utility
{
    /// <summary>
    /// クリック回数からマウスの大きさを算出
    /// 大きさ(m) = クリック回数(回)^2 / 100
    /// </summary>
    /// <param name="clickCount">マウスクリック回数</param>
    /// <returns>マウスの大きさ(単位: メートル)</returns>
    public static float CalcMouseSize(int clickCount) {
        float size = (float)(clickCount * clickCount / 100);
        return size;
    }

    /// <summary>
    /// ランキングデータ(Json)から読み込み、
    /// string配列として返す
    /// </summary>
    /// <returns>ランキングデータ</returns>
    public static string[] LoadRankingList()
    {
        return null;
    }

    /// <summary>
    /// ランキングデータをJsonファイルへ保存
    /// </summary>
    /// <param name="data">ランキングデータ</param>
    /// <returns>dataを返す</returns>
    public static string[] SaveRankingList(string[] data)
    {
        return null;
    }

    /// <summary>
    /// 元ランキングデータと、追加されたスコアからランキングデータの構築
    /// dataにstringキャストしたscoreを追加して、SortRankingListにぶち込む
    /// </summary>
    /// <param name="data">元ランキングデータ</param>
    /// <param name="score">追加されたスコア</param>
    /// <returns>スコアを加味したランキングデータ</returns>
    public static string[] MakeRankingList(string[] data, float score)
    {
        string[] tmp = data;
        // 配列サイズ拡張
        Array.Resize(ref tmp, tmp.Length + 1);
        // 末尾にスコアデータ追加
        tmp[tmp.Length - 1] = score.ToString();
        // ランキングソート
        string[] result = SortRankingList(tmp);
        // ええやん、返したれ返したれ
        return result;
    }

    /// <summary>
    /// ランキングデータの整列
    /// 配列をfloatキャストして、QuickSortにぶち込む。たぶん
    /// </summary>
    /// <param name="data">ランキングデータ</param>
    /// <returns>ランキングデータ</returns>
    public static string[] SortRankingList(string[] data)
    {
        // 配列キャスト・例外はデフォルト値代入
        int[] numData = Array.ConvertAll(data, s =>
        {
            int tmp = 0;
            if (!int.TryParse(s, out tmp))
            {
                tmp = 50; // デフォルト値
            }
            return tmp;
        });
        // ↑しないとうまくできないからね、しゃーない
        QuickSort(numData, 0, numData.Length - 1);
        // string配列に変換して返す
        string[] result = Array.ConvertAll(numData, x => x.ToString());

        return result;
    }

    /// <summary>
    /// クイックソート
    /// </summary>
    /// <param name="array">対象の配列</param>
    /// <param name="left">ソート範囲の最初のインデックス</param>
    /// <param name="right">ソート範囲の最後のインデックス</param>
    public static void QuickSort<T>(T[] array, int left, int right) where T : IComparable<T>
    {
        // 範囲が一つだけなら処理を抜ける
        if (left >= right) return;

        // ピポットを選択(範囲の先頭・真ん中・末尾の中央値を使用)
        T pivot = Median(array[left], array[(left + right) / 2], array[right]);

        int i = left;
        int j = right;

        while (i <= j)
        {
            // array[i] < pivot まで左から探索
            while (i < right && array[i].CompareTo(pivot) > 0) i++;
            // array[i] >= pivot まで右から探索
            while (j > left && array[j].CompareTo(pivot) <= 0) j--;

            if (i > j) 
            {
                break;
            }
            Swap<T>(ref array[i], ref array[j]);

            // 交換を行った要素の次の要素にインデックスを進める
            i++;
            j--;
        }

        QuickSort(array, left, i - 1);
        QuickSort(array, i, right);
    }


    /// <summary>
    /// 中央値を求める
    /// </summary>
    /// <param name="x">右値</param>
    /// <param name="y">中央値</param>
    /// <param name="z">左値</param>
    /// <typeparam name="T">数値型</typeparam>
    /// <returns>中央値</returns>
    private static T Median<T>(T x, T y, T z) where T : IComparable<T>
    {
        // x > y なら1以上の整数値が返される
        if (x.CompareTo(y) > 0) Swap(ref x, ref y);
        if (x.CompareTo(z) > 0) Swap(ref x, ref z);
        if (y.CompareTo(z) > 0) Swap(ref y, ref z);
        return y;
    }

    /// <summary>
    /// 参照を入れ替える(値型だと変数のコピーになってしまうため)
    /// </summary>
    /// <param name="x">入れ替え対象1</param>
    /// <param name="y">入れ替え対象2</param>
    /// <typeparam name="T">数値型</typeparam>
    private static void Swap<T>(ref T x, ref T y) where T : IComparable<T>
    {
        var tmp = x;
        x = y;
        y = tmp;
    }

}
