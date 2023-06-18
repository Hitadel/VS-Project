using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 地形とモンスターの位置を計算する機能です。
public class Reposition : MonoBehaviour
{
    Collider2D coll;

    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = GameManager.instance.player.transform.position; // プレイヤーの位置
        Vector3 myPos = transform.position; // オブジェクトの位置
        

        switch (transform.tag)
        {
            case "Ground": //// // 地形の位置を計算
                float diffX = playerPos.x - myPos.x;
                float diffY = playerPos.y - myPos.y;
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX);
                diffY = Mathf.Abs(diffY);

                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;

            case "Enemy": // プレイヤーの位置とモンスターの位置の距離を計算
                if (coll.enabled)
                {
                    Vector3 dist = playerPos - myPos;
                    Vector3 ran = new Vector3(Random.Range(-3,3), Random.Range(-3,3), 0);
                    transform.Translate(ran + dist * 2);
                }
                break;
        }
    }
}
