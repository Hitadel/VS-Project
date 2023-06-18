using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 各キャラクターの特性を設定する機能です。
public class Character : MonoBehaviour
{
    public static float Speed　// 最初のキャラクターの特性
    {
        //
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f; }
    }

    public static float WeaponSpeed // 2番目のキャラクターの特性
    {
        
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f; }
    }

    public static float WeaponRate
    {
        
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f; }
    }

    public static float Damage // 3番目のキャラクターの特性
    {
        
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f; }
    }

    public static int Count // 最後のキャラクターの特性
    {
        
        get { return GameManager.instance.playerId == 3 ? 1 : 0; }
    }
}
