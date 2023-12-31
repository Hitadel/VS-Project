using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲームのUIを表示する機能です。
public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Health }
    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
        myText = GetComponent<Text>(); // レベルと残り時間、倒したモンスターの数に使う
        mySlider = GetComponent<Slider>(); //HPと経験値に使う
    }

    void LateUpdate()
    {
        switch(type)
        {
            case InfoType.Exp:
                float curExp = GameManager.instance.exp; // 現在の経験値
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level, GameManager.instance.nextExp.Length-1)]; // 次のレベルまでの最大経験値
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.level);
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainTime = GameManager.instance.maxGameTime - GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60); // 分数部分を取得
                int sec = Mathf.FloorToInt(remainTime % 60); // 秒数部分を取得
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec); // 残り時間を「分:秒」の形式で表示
                break;
            case InfoType.Health:
                float curHealth = GameManager.instance.health; // 現在のHP
                float maxHealth = GameManager.instance.maxHealth; // // 最大HP
                mySlider.value = curHealth / maxHealth;

                break;
        }
    }
}
