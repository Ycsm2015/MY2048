using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScore : MonoBehaviour
{
    public GameObject CurScore;
    public GameObject BestScore;

    public Animation CurScoreAnim;
    public Animation BestScoreAnim;

    private int _cur;
    private int _best;

    public void SetScore(int cur, int best)
    {
        if (_cur != cur)
        {
            _cur = cur;
            CurScore.GetComponent<TextMesh>().text = "CurScore:\n" + cur.ToString();
            CurScoreAnim.Play();
        }
        if (_best != best)
        {
            _best = best;
            BestScore.GetComponent<TextMesh>().text = "BestScore:\n" + best.ToString();
            BestScoreAnim.Play();
        }
    }
    public void ShowAddScore(int add)
    {
        if (add > 0)
        {

        }
    }
}
