using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour // オブジェクトプール
{
    // プレハブ(Prefab)を格納する配列変数
    public GameObject[] prefabs;
    // プールの担当をするリスト
    List<GameObject>[] pools;

    void Awake()
    {
        // リスト初期化時、配列の長さを活用
        pools = new List<GameObject>[prefabs.Length];
        
        // すべてのオブジェクトプールリストを初期化
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }
    
    // ゲームオブジェクト(GameObject)を返す関数
    public GameObject Get(int i) 
    {
        GameObject select = null;

        // 選択したプールで使用していないGameObjectにアクセス
        foreach (GameObject item in pools[i])
        {
            if(!item.activeSelf) // 見つけたらselect変数に割当
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

            // 見つけなかったら？
            if(!select)
            {
                // 新しく生成してselect変数に割当
                select = Instantiate(prefabs[i], transform); // Instantiate():オリジナルオブジェクトをコピーしてシーンに生成する関数
                pools[i].Add(select);
            }
        
        return select;

    }
}
