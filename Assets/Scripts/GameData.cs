using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public const float DeltaDrag = 0.1f;

    public const float _mapWidth = 425;
    public const float _mapHeight = 425;
    public const int MinNum = 2;
    public const int SecondMinNum = 4;
    public const int _line = 4;
    public const float _perSize = 100;
    public const float MoveTime = 0.17f;
    public const int MapSize = 4;
    public const int StartShowNumberCount = 3;
    public const int MaxNum = 2048;

    public static Vector2[,] MapPos;
    private int mCurScore = 0;
    public int CurScore
    {
        get
        {
            return mCurScore;
        }
        set
        {
            mCurScore = value;
            if (mCurScore > mBestScore)
                BestScore = value;
        }
    }

    private int mBestScore;
    public int BestScore
    {
        get
        {
            return mBestScore;
        }
        set
        {
            mBestScore = value;
            PlayerPrefs.SetInt("BestScore", value);
            PlayerPrefs.Save();
        }
    }
    public static float ScreenDpi
    {
        get
        {
            return Screen.dpi > 0 ? Screen.dpi : 169;
        }
    }
    public void Init()
    {
        MapPos = new Vector2[MapSize, MapSize];
        float spanSize = (_mapWidth - 4 * _perSize) * .2f;

        for (int x = 0; x < MapSize; x++)
        {
            for (int y = 0; y < MapSize; y++)
            {
                MapPos[x, y] = new Vector2(
                    (x + 1) * spanSize + x * _perSize + _perSize * .5f,
                    (MapSize - y) * spanSize + (MapSize - y - 1) * _perSize + _perSize * .5f) -
                    new Vector2(_mapWidth * .5f, _mapHeight * .5f);
            }
        }
        mCurScore = 0;
        if (PlayerPrefs.HasKey("BestScore"))
            mBestScore = PlayerPrefs.GetInt("BestScore");
        else
            mBestScore = 0;
    }

    public static bool CanMerge(int num1, int num2)
    {
        return num1 == num2;
    }
    public static int GetMerge(int num1, int num2)
    {
        if (CanMerge(num1, num2))
            return num1 + num2;
        return 0;
    }
}



public class Pos2
{
    public int X;
    public int Y;

    public Pos2() { }
    public Pos2(int x, int y) { X = x; Y = y; }
    public Pos2(Pos2 pos)
    {
        X = pos.X;
        Y = pos.Y;
    }

    public bool Equals(Pos2 pos)
    {
        if (X == pos.X)
            if (Y == pos.Y)
                return true;
        return false;
    }

    public bool Xequals(Pos2 pos)
    {
        if (X == pos.X)
            return true;
        return false;
    }
    public bool Yequals(Pos2 pos)
    {
        if (Y == pos.Y)
            return true;
        return false;
    }
    public override string ToString()
    {
        return "(" + X + "," + Y + ")";
    }
}
public enum MoveNormal
{
    Up,
    Down,
    Left,
    Right,
}
public class NumberComparer : IComparer<UINumber>
{
    public static MoveNormal CurNor;

    public NumberComparer() { }
    public NumberComparer(MoveNormal nor)
    {
        CurNor = nor;
    }
    public int Compare(UINumber x, UINumber y)
    {
        switch (CurNor)
        {
            case MoveNormal.Up:
                if (x.CurPos.X == y.CurPos.X)
                {
                    if (x.CurPos.Y < y.CurPos.Y)
                        return -1;
                }
                if (x.CurPos.X < y.CurPos.X)
                    return -1;
                break;
            case MoveNormal.Down:
                if (x.CurPos.X == y.CurPos.X)
                {
                    if (x.CurPos.Y > y.CurPos.Y)
                        return -1;
                }
                if (x.CurPos.X < y.CurPos.X)
                    return -1;
                break;
            case MoveNormal.Left:
                if (x.CurPos.Y == y.CurPos.Y)
                {
                    if (x.CurPos.X < y.CurPos.X)
                        return -1;
                }
                if (x.CurPos.Y < y.CurPos.Y)
                    return -1;
                break;
            case MoveNormal.Right:
                if (x.CurPos.Y == y.CurPos.Y)
                {
                    if (x.CurPos.X > y.CurPos.X)
                        return -1;
                }
                if (x.CurPos.Y < y.CurPos.Y)
                    return -1;
                break;
        }
        return 1;
    }
}