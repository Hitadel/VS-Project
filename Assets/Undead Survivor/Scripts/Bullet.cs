using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 弾幕のダメージや貫通率を設定する機能です。
public class Bullet : MonoBehaviour
{
    public float damage;  // ダメージ
    public int per;  // 貫通率

    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    // 初期化
    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per = per;

        if (per >= 0)
            rigid.velocity = dir * 15f;  // 貫通率が無縁(マイナス)でないと速度を適用
    }

    // モンスターの貫通程度
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || per == -100) // エラー処理
            return;
        
        per--;

        // 貫通率が低下してゼロになると弾膜が消える
        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    // 弾倉が画面外に出たら消去
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }
}
