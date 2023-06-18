using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// モンスターのステータスや衝突判定を担当する機能です。
public class Enemy : MonoBehaviour
{
    public float speed;
    public float health;
    public float maxHealth;
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;
    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    // モンスターの速度と方向の処理
    void FixedUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))　// モンスターが死亡またはノックバックされると停止
            return;

        Vector2 dirVec = target.position - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
        rigid.velocity = Vector2.zero;
    }

    // スプライトの反転
    void LateUpdate()
    {
        if (!GameManager.instance.isLive)
            return;

        if (!isLive)
            return;
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // スポーン
    void OnEnable()
    {
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }

    // 各種モンスターの初期化
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    // モンスターがダメージを受けるとノックバック
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.CompareTag("Bullet") || !isLive)
            return;
        
        health -= collision.GetComponent<Bullet>().damage; 
        StartCoroutine(KnockBack());
        
        if (health > 0)
        { // 被害を受けても死亡していない場合
            anim.SetTrigger("Hit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit); // 被弾音
        }
        else
        { // 死亡した場合
            isLive= false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.instance.kill++;
            GameManager.instance.GetExp(); //　プレイヤーの経験値が上がる

            if (GameManager.instance.isLive)
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
        }
    }
    
    // ノックバック
    IEnumerator KnockBack()
    {
        yield return wait; // 次の一つの物理Frameをディレイ
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

        // yield return null; // 1Frame休む　
        // yield return new WaitForSeconds(2f); // 2秒休む
        
    }

    // モンスター死亡
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
