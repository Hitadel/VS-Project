using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// レベルアップの場合、アイテムをUIに表示する機能です。
public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    Item[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    // UIを表示
    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        AudioManager.instance.EffectBgm(true);
    }

    // UIを未表示
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        AudioManager.instance.EffectBgm(false);
    }

    public void Select(int i) // UI呼び出し
    {
        items[i].OnClick();
    }

    void Next()
    {
        // 1.すべてのアイテムを無効化
        foreach(Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2.その中からランダムで3つのアイテムを活性化
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int i = 0; i < ran.Length; i++)
        {
            Item ranItem = items[ran[i]];
            
        

            // 3.最大レベルのアイテムの場合は消費アイテムで代替
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else{
                ranItem.gameObject.SetActive(true);
            }
        
        }
    }
}
