using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 各種ゲームの設定とパラメータを管理するコア機能です。
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("# Game Control")]
    public bool isLive; // ゲームをPAUSE(ポーズ)
    public float gameTime; // ゲーム一回の時間
    public float maxGameTime = 2 * 10f; // 最大時間

    [Header("# Player Info")] // プレイヤーのステータス
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 3, 10, 25, 30, 45, 60, 80, 90, 100, 110 }; // レベルアップに必要な経験値

    [Header("# Game Object")]
    public PoolManager pool; // オブジェクトプール
    public Player player;
    public LevelUp uiLevelUp; // レベルアップするとスキルアップの画面を表示
    public Result uiResult; // ゲーム結果を表示
    public Transform uiJoy; // タッチコントローラ
    public GameObject enemyCleaner; //　ゲームが終わったら全てのモンスターを処理

    void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60; // 60フレームで設定
    }

    public void GameStart(int id)
    {
        playerId = id;
        health = maxHealth;

        player.gameObject.SetActive(true);
        uiLevelUp.Select(playerId % 2);
        Resume();
        
        AudioManager.instance.PlayBgm(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);

    }

    // ゲームオーバー
    public void GameOver()
    {
     StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        Stop();

    AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
    }

    // 生存
    public void GameVictory()
    {
     StartCoroutine(GameVictoryRoutine());
    }

    IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        Stop();

        AudioManager.instance.PlayBgm(false);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
    }

    // ゲームを再開する
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    // ゲームを終了
    public void GameQuit()
    {
        Application.Quit();
    }

    void Update()
    {
        if (!isLive)
            return;
        
        gameTime += Time.deltaTime;　// deltaTimeに設定すると、フレームの変化にも一定の時間が流れるようになります。

        if (gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameVictory(); // 生存した場合
        }
    }

    // 経験値の設定
    public void GetExp()
    {
        if(!isLive)
            return;

        exp++;

        if(exp == nextExp[Mathf.Min(level, nextExp.Length-1)]) // 経験値が一定値以上になった場合
        {
            level++;
            exp = 0;
            uiLevelUp.Show();
        }
    }

    // Pauseする
    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        uiJoy.localScale = Vector3.zero;
    }
    
    // 再生する
    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        uiJoy.localScale = Vector3.one;
    }
}
