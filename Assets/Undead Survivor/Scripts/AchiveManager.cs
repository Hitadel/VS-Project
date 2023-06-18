using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 隠しキャラを解放する機能です。
public class AchiveManager : MonoBehaviour
{
    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;
    public GameObject uiNotice;  // 通知を表示するゲームオブジェクト

    // 達成条件の列挙型
    enum Achive { UnlockPotato, UnlockBean }  
    Achive[] achives;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achives = (Achive[])Enum.GetValues(typeof(Achive));  // 達成条件の配列を列挙型から取得
        wait = new WaitForSecondsRealtime(5); // リアルタイムで５秒間、通知を表示

        if (!PlayerPrefs.HasKey("MyData"))  // PlayerPrefsに「MyData」というキーがない場合
        {
            Init();  // 初期化
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);  // "MyData"というキーで整数値1を保存

        foreach (Achive achive in achives)
        {
            PlayerPrefs.SetInt(achive.ToString(), 0);  // 各達成条件を未達成状態(0)で保存
        }

    }

    void Start()
    {
        UnlockCharacter();  // 解放する
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacter.Length; i++)
        {
            string achiveName = achives[i].ToString();  // 達成条件の名前を取得
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;  // 達成条件が達成されたかどうかを確認

            // 隠しキャラの表示を設定
            lockCharacter[i].SetActive(!isUnlock);
            unlockCharacter[i].SetActive(isUnlock);  
        }   
    }

    void LateUpdate()
    {
        foreach (Achive achive in achives)
        {
            CheckAchive(achive);  // 達成条件をチェック
        }
    }

    void CheckAchive(Achive achive)
    {
        bool isAchive = false;  // 達成条件が達成されたかどうかを示す

        switch (achive)
        {
            case Achive.UnlockPotato:
                isAchive = GameManager.instance.kill >= 350;  // キル数が350以上になると解放
                break;
            case Achive.UnlockBean:
                isAchive = GameManager.instance.gameTime == GameManager.instance.maxGameTime;  // 最後までクリアすると解放
                break;
        }

        if(isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)  
        {
            PlayerPrefs.SetInt(achive.ToString(),1);  // 達成状態(1)を保存
            
            for (int i = 0; i < uiNotice.transform.childCount; i++)
            {
                // 達成条件に応じた通知の表示を設定
                bool isActive = i == (int)achive;  
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }

            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);  // 通知を表示
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);  // 効果音を再生

        yield return wait;

        uiNotice.SetActive(false);  // 通知を非表示
    }
}