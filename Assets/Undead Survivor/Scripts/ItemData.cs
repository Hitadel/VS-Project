using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// アイテムの種類を管理する機能です。
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, Glove, Shoe, Heal }

    [Header("# Main Info")] // アイテムの説明
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;


    [Header("# Level Data")] // アイテムレベルによる性能
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public int[] counts;


    [Header("# Weapon")] // 武器の種類
    public GameObject projectile;
    public Sprite hand;

}
