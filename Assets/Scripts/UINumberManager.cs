using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UINumberManager : MonoBehaviour
{
    public UINumber[] Numbers;
    public int CurMergeScore = 0;
    public int CurMergeMaxNumber = 0;
    public List<UINumber> InHandNumbers;
    public List<UINumber> OnMapNumbers;
    private List<Pos2> EmptyPos;
    public void InitNumbers()
    {
        Numbers = GetComponentsInChildren<UINumber>();
        EmptyPos = new List<Pos2>();
        for (int x = 0; x < GameData.MapSize; x++)
            for (int y = 0; y < GameData.MapSize; y++)
            {
                Numbers[GameData.MapSize * x + y].Init(this);
                EmptyPos.Add(new Pos2(x, y));
            }
        InHandNumbers = new List<UINumber>();
        InHandNumbers.AddRange(Numbers);
        OnMapNumbers = new List<UINumber>();
    }

    public bool ShowNextNumber()
    {
        if (OnMapNumbers.Count < GameData.MapSize * GameData.MapSize)
        {
            UINumber nextNumber = InHandNumbers[0];
            InHandNumbers.Remove(nextNumber);
            OnMapNumbers.Add(nextNumber);
            if (Random.Range(0f, 100f) < 90f)
            {
                nextNumber.SetNumber(GameData.MinNum, true);
            }
            else
            {
                nextNumber.SetNumber(GameData.SecondMinNum, true);
            }
            Pos2 pos = EmptyPos[Random.Range(0, EmptyPos.Count)];
            nextNumber.SetPos(pos);
            RemoveEmptyPos(pos);
            return true;
        }
        return false;
    }

    public bool CheckCanMove()
    {
        int count = OnMapNumbers.Count;
        OnMapNumbers.Sort(new NumberComparer(MoveNormal.Up));
        for (int i = 0; i < count - 1; i++)
        {
            if (OnMapNumbers[i].CurPos.X == OnMapNumbers[i + 1].CurPos.X)
            {
                if (GameData.CanMerge(OnMapNumbers[i].CurNumber, OnMapNumbers[i + 1].CurNumber))
                    return true;
            }
        }
        OnMapNumbers.Sort(new NumberComparer(MoveNormal.Left));
        for (int i = 0; i < count - 1; i++)
        {
            if (OnMapNumbers[i].CurPos.Y == OnMapNumbers[i + 1].CurPos.Y)
            {
                if (GameData.CanMerge(OnMapNumbers[i].CurNumber, OnMapNumbers[i + 1].CurNumber))
                    return true;
            }
        }
        return false;
    }

    public bool Move(MoveNormal nor)
    {
        bool ismove = false;
        CurMergeScore = 0;
        CurMergeMaxNumber = 0;
        OnMapNumbers.Sort(new NumberComparer(nor));
        //foreach(UINumber num in OnMapNumbers)
        //{
        //    Debug.Log(num.CurPos.ToString());
        //}
        int count = OnMapNumbers.Count;
        for (int i = 0; i < count; i++)
        {
            if (i != 0)
                ismove = ismove | OnMapNumbers[i].MoveTo(nor, OnMapNumbers[i - 1]);
            else
                ismove = ismove | OnMapNumbers[i].MoveTo(nor, null);
        }
        return ismove;
    }

    public void RemoveEmptyPos(Pos2 pos)
    {
        int count = EmptyPos.Count;
        for (int i = 0; i < count; i++)
        {
            if (EmptyPos[i].Equals(pos))
            {
                EmptyPos.RemoveAt(i);
                return;
            }
        }
    }
    public void AddEmptyPos(Pos2 pos)
    {
        bool ishave = false;
        int count = EmptyPos.Count;
        for (int i = 0; i < count; i++)
        {
            if (EmptyPos[i].Equals(pos))
            {
                ishave = true;
            }
        }
        if (!ishave)
            EmptyPos.Add(new Pos2(pos));
    }

}
