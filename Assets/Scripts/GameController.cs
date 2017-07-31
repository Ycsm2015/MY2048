using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject LoseObj;
    public GameObject WinObj;

    public UINumberManager mUINumberManager;
    public UIScore mUIScore;
    public GameData mGameData;
    //生成背景
    public GameObject backParent;
    public GameObject backPrefabs;
    //生成数字并添加索引
    public GameObject UINumberParent;
    public GameObject UINumberPrefabs;

    private bool isGameOver = false;
    private bool canControll = false;


    public Sprite[] Texures = new Sprite[11];
    public Sprite GetSpriteByNum(int num)
    {
        int i = num == 2 ? 0 :
            num == 4 ? 1 :
            num == 8 ? 2 :
            num == 16 ? 3 :
            num == 32 ? 4 :
            num == 64 ? 5 :
            num == 128 ? 6 :
            num == 256 ? 7 :
            num == 512 ? 8 :
            num == 1024 ? 9 :
            10;
        return Texures[i];
    }

    private static GameController _instance;

    public static GameController GetInstance()
    {
        if (_instance == null)
        {
            _instance = GameObject.Find("Game").GetComponent<GameController>();
        }
        return _instance;
    }
    public IEnumerator Start()
    {
        InitBackGroundAndNumbers();
        yield return new WaitForEndOfFrame();
        NewGame();
    }
    public void NewGame()
    {
        isGameOver = false;
        LoseObj.SetActive(false);
        WinObj.SetActive(false);
        mGameData = new GameData();
        mGameData.Init();
        mUIScore.SetScore(mGameData.CurScore, mGameData.BestScore);
        mUINumberManager.InitNumbers();
        for (int i = 0; i < GameData.StartShowNumberCount; i++)
        {
            mUINumberManager.ShowNextNumber();
        }
        canControll = true;
    }
    private void Awake()
    {
        mUINumberManager = UINumberParent.GetComponent<UINumberManager>();
    }

    private void InitBackGroundAndNumbers()
    {
        float width = GameData._mapWidth;
        float height = GameData._mapHeight;

        float spanSize = (GameData._mapWidth - 4 * GameData._perSize) * .2f;

        for (int i = 0; i < GameData.MapSize; i++)
        {
            for (int j = 0; j < GameData.MapSize; j++)
            {
                Vector2 pos = new Vector2(
                    (i + 1) * spanSize + i * GameData._perSize + GameData._perSize * .5f,
                    (j + 1) * spanSize + j * GameData._perSize + GameData._perSize * .5f) -
                    new Vector2(width * .5f, height * .5f);

                GameObject backGO = Instantiate(backPrefabs);
                GameObject numGO = Instantiate(UINumberPrefabs);
                backGO.transform.SetParent(backParent.transform, false);
                backGO.transform.position = pos;

                numGO.transform.SetParent(UINumberParent.transform, false);
                numGO.transform.position = pos;
            }
        }
    }

    #region 移动
    void Update()
    {
        if (!isGameOver && canControll)
        {
            KeyBoardUpdate();
            TouchUpdate();
        }
    }
    void KeyBoardUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            UpMove();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            LeftMove();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            RightMove();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            DownMove();
        }
    }
    void TouchUpdate()
    {
        if (Input.touchCount > 0 && Input.touchCount < 2)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 deltatouch = touch.deltaPosition;
            if (deltatouch.magnitude / GameData.ScreenDpi > GameData.DeltaDrag)
            {
                if (Mathf.Abs(deltatouch.x) > Mathf.Abs(deltatouch.y))
                {
                    if (deltatouch.x > 0)
                    {
                        RightMove();
                    }
                    else
                    {
                        LeftMove();
                    }
                }
                else
                    if (Mathf.Abs(deltatouch.x) < Mathf.Abs(deltatouch.y))
                {
                    if (deltatouch.y > 0)
                    {
                        UpMove();
                    }
                    else
                    {
                        DownMove();
                    }
                }
            }


        }
    }

    public void UpMove()
    {
        StartMove(MoveNormal.Up);
    }
    public void DownMove()
    {
        StartMove(MoveNormal.Down);
    }
    public void RightMove()
    {
        StartMove(MoveNormal.Right);
    }
    public void LeftMove()
    {
        StartMove(MoveNormal.Left);
    }

    private void StartMove(MoveNormal nor)
    {
        canControll = false;
        if (mUINumberManager.Move(nor))
            StartCoroutine(Moving());
        else
            canControll = true;
    }
    private IEnumerator Moving()
    {
        canControll = false;
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(GameData.MoveTime);
        yield return new WaitForEndOfFrame();

        mGameData.CurScore += mUINumberManager.CurMergeScore;
        mUIScore.ShowAddScore(mUINumberManager.CurMergeScore);
        mUIScore.SetScore(mGameData.CurScore, mGameData.BestScore);
        if (mUINumberManager.CurMergeMaxNumber >= GameData.MaxNum)
        {
            yield return new WaitForSeconds(1f);
            WinObj.SetActive(true);
            isGameOver = true;
            yield break;
        }
        mUINumberManager.ShowNextNumber();
        if (mUINumberManager.OnMapNumbers.Count == GameData.MapSize * GameData.MapSize && !mUINumberManager.CheckCanMove())
        {
            yield return new WaitForSeconds(1f);
            LoseObj.SetActive(true);
            isGameOver = true;
            yield break;
        }
        canControll = true;
    }



    #endregion
}

