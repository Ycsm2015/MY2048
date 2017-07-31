using UnityEngine;
using System.Collections;

public class UINumber : MonoBehaviour
{
    public Transform SelfTF;
    public Animation Anima;
    public SpriteRenderer Sprite;

    public int CurNumber = 0;

    public Pos2 GotoPos;
    public Pos2 CurPos;
    public int MergeNumber = 0;
    public UINumberManager Father;
    public bool isMove
    {
        get
        {
            return !GotoPos.Equals(CurPos);
        }

    }
    public void Init(UINumberManager father)
    {
        SelfTF = transform;
        Anima = GetComponent<Animation>();
        Sprite = GetComponent<SpriteRenderer>();
        Sprite.sortingOrder = -2;
        Father = father;
    }
    public void SetNumber(int num, bool apear = false)
    {
        if (num > 0)
        {
            Sprite.sprite = GameController.GetInstance().GetSpriteByNum(num);
            Sprite.enabled = true;
            Sprite.sortingOrder = num;

            if (apear)
                Anima.Play("NumberApear");
            else
                Anima.Play("NumberShake");

        }
        else
        {
            Sprite.sortingOrder = -2;
            Sprite.enabled = false;
        }
        CurNumber = num;
    }

    public void SetPos(Pos2 pos)
    {
        CurPos = new Pos2(pos);
        GotoPos = new Pos2(pos);
        SelfTF.localPosition = GameData.MapPos[pos.X, pos.Y];
    }

    public bool MoveTo(MoveNormal nor, UINumber forwardNum)
    {
        MergeNumber = 0;
        switch (nor)
        {
            case MoveNormal.Up:
                moveToUP(forwardNum);
                break;
            case MoveNormal.Down:
                moveToDown(forwardNum);
                break;
            case MoveNormal.Left:
                moveToLeft(forwardNum);
                break;
            case MoveNormal.Right:
                moveToRight(forwardNum);
                break;
        }
        if (GotoPos.Equals(CurPos))
            return false;
        StartCoroutine(Moving(forwardNum));
        Father.RemoveEmptyPos(GotoPos);
        Father.AddEmptyPos(CurPos);
        return true;
    }

    private void moveToUP(UINumber forwardNum)
    {
        if (forwardNum == null || CurPos.X != forwardNum.CurPos.X)
        {
            GotoPos = new Pos2(CurPos.X, 0);
            return;
        }
        if (GameData.CanMerge(CurNumber, forwardNum.CurNumber) && forwardNum.MergeNumber == 0)
        {
            GotoPos = new Pos2(forwardNum.GotoPos);
            MergeNumber = GameData.GetMerge(CurNumber, forwardNum.CurNumber);
            return;
        }
        else
        {
            GotoPos = new Pos2(CurPos.X, forwardNum.GotoPos.Y + 1);
            return;
        }
    }
    private void moveToDown(UINumber forwardNum)
    {
        if (forwardNum == null || CurPos.X != forwardNum.CurPos.X)
        {
            GotoPos = new Pos2(CurPos.X, GameData.MapSize - 1);
            return;
        }

        if (GameData.CanMerge(CurNumber, forwardNum.CurNumber) && forwardNum.MergeNumber == 0)
        {
            GotoPos = new Pos2(forwardNum.GotoPos);
            MergeNumber = GameData.GetMerge(CurNumber, forwardNum.CurNumber);
            return;
        }
        else
        {
            GotoPos = new Pos2(CurPos.X, forwardNum.GotoPos.Y - 1);
            return;
        }


    }
    private void moveToLeft(UINumber forwardNum)
    {
        if (forwardNum == null || CurPos.Y != forwardNum.CurPos.Y)
        {
            GotoPos = new Pos2(0, CurPos.Y);
            return;
        }

        if (GameData.CanMerge(CurNumber, forwardNum.CurNumber) && forwardNum.MergeNumber == 0)
        {
            GotoPos = new Pos2(forwardNum.GotoPos);
            MergeNumber = GameData.GetMerge(CurNumber, forwardNum.CurNumber);
            return;
        }
        else
        {
            GotoPos = new Pos2(forwardNum.GotoPos.X + 1, GotoPos.Y);
            return;
        }


    }
    private void moveToRight(UINumber forwardNum)
    {
        if (forwardNum == null || CurPos.Y != forwardNum.CurPos.Y)
        {
            GotoPos = new Pos2(GameData.MapSize - 1, CurPos.Y);
            return;
        }

        if (GameData.CanMerge(CurNumber, forwardNum.CurNumber) && forwardNum.MergeNumber == 0)
        {
            GotoPos = new Pos2(forwardNum.GotoPos);
            MergeNumber = GameData.GetMerge(CurNumber, forwardNum.CurNumber);
            return;
        }
        else
        {
            GotoPos = new Pos2(forwardNum.GotoPos.X - 1, GotoPos.Y);
            return;
        }


    }

    private IEnumerator Moving(UINumber forwardNumber)
    {
        if (GotoPos.Equals(CurPos))
            yield break;
        //Debug.Log("GotoPos=" + GotoPos.ToString());
        Vector3 positon = GameData.MapPos[GotoPos.X, GotoPos.Y];
        float time = GameData.MoveTime;
        Vector3 normal = positon - SelfTF.localPosition;
        Vector3 Speed = normal / time;
        while (time > 0)
        {
            SelfTF.Translate(Speed * Time.deltaTime, Space.Self);
            yield return new WaitForEndOfFrame();
            time -= Time.deltaTime;
            if (time < Time.deltaTime)
                break;
        }
        SelfTF.localPosition = positon;

        if (MergeNumber > 0)
        {
            SetNumber(MergeNumber);
            Father.OnMapNumbers.Remove(forwardNumber);
            Father.InHandNumbers.Add(forwardNumber);
            forwardNumber.SetNumber(0);
            //Debug.Log("OnMapNumbers=" + Father.OnMapNumbers.Count + " InHandNumbers=" + Father.InHandNumbers.Count);
            Father.CurMergeScore += MergeNumber;
            if (Father.CurMergeMaxNumber < MergeNumber)
            {
                Father.CurMergeMaxNumber = MergeNumber;
            }

        }

        CurPos = new Pos2(GotoPos);
    }
}
