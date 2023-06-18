using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//　負け、生存の結果を出力する機能です。
public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Lose()
    {
        titles[0].SetActive(true);
    }

    public void Win()
    {
        titles[1].SetActive(true);
    }
}
