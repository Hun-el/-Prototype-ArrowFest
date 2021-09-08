using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowsObject : StackObject
{
    float speed;
    public const float DISTANCEOFORBITS = 0.0175f; //0.017f
    const float PLANESIZE = 1f; //0.4f
    float leftMove = 0f;
    
    [SerializeField] int maxVisible;
    int orbit = 1;

    bool finishMode = false,fail = false;

    [SerializeField] PoolObject arrowPrefab;
    [SerializeField] TextMesh counterText;
    GameManager gameManager;
    GoldSystem goldSystem;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        goldSystem = FindObjectOfType<GoldSystem>();

        speed = gameManager.minSpeed;
        speed = speed + PlayerPrefs.GetInt("level") * gameManager.speedIncrease;
        if(speed >= gameManager.maxSpeed)
        {
            speed = gameManager.maxSpeed;
        }

        PoolSystem.ResetPools();
        if(PlayerPrefs.HasKey("arrowLevel")){ AddNewStack(PlayerPrefs.GetInt("arrowLevel")); }else{ AddNewStack(); }
        TouchSystem.Touching += TouchSystem_Touching;
    }

    void FixedUpdate()
    {
        if(gameManager.readyforMove && !fail)
        {
            Move();
        }
    }

    private void TouchSystem_Touching()
    {
        leftMove = 0;
        if (Mathf.Abs(TouchSystem.DistanceBetweenTwoFrame.x) >= 5f)
            leftMove = TouchSystem.DistanceBetweenTwoFrame.x;
        SetAllArrowsLocation(Mathf.Clamp((PLANESIZE - Mathf.Abs(transform.position.x)), 0.3f, 1f) / PLANESIZE, 1f);
    }

    public void Move()
    {
        float NewX = 0;
        float touchXDelta = 0;

        #if UNITY_ANDROID && !UNITY_EDITOR
        if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && !finishMode)
        {
            touchXDelta = 20*Input.GetTouch(0).deltaPosition.x / Screen.width;
        }
        #endif

        #if UNITY_STANDALONE || UNITY_EDITOR
        if(Input.GetMouseButton(0) && !finishMode)
        {
            touchXDelta = 10*Input.GetAxis("Mouse X");
        }
        #endif

        NewX = transform.position.x + touchXDelta * 1 * Time.deltaTime;
        NewX = Mathf.Clamp(NewX ,-0.4f,0.4f);

        Vector3 newPos = new Vector3(NewX,transform.position.y,transform.position.z + speed * Time.deltaTime);
        transform.position = newPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        Window window;
        if (other.TryGetComponent<Window>(out window))
        {
            if (other.transform.position.x / transform.position.x < 0)
                return;
            other.enabled = false;
            if (window.TypeWindow == WindowType.Positive)
            {
                AddNewStack(window.TypeStack == StackType.Plus ? window.Value : StackCount * (window.Value - 1));
            }
            else
            {
                RemoveArrows(window.TypeStack == StackType.Plus ? window.Value : StackCount - (StackCount / window.Value));
            }
            SetAllArrowsLocation(Mathf.Clamp((PLANESIZE - Mathf.Abs(transform.position.x)),0.3f,1f) / PLANESIZE, 1f);
            SetArrowCounterText();
        }
        else if (other.tag == "FinishRoad")
        {
            finishMode = true; speed = gameManager.maxSpeed; transform.DOMoveX(0f, 0.5f); SetAllArrowsLocation(1f, 1f);
        }
        else if (other.tag == "FinishLevel")
        {
            gameManager.LevelCompleted();
        }

        if(other.tag == "Human")
        {
            if(other.GetComponent<Human>().health < StackCount)
            {
                RemoveArrows(other.GetComponent<Human>().health);
                goldSystem.UpdateGold(10 * PlayerPrefs.GetInt("incomeLevel"));
                other.gameObject.GetComponent<Animator>().SetBool("dead",true);
                if(other.transform.parent.GetComponent<Renderer>() != null)
                {
                    Color c = other.transform.parent.GetComponent<Renderer>().material.color;
                    other.transform.parent.GetComponent<Renderer>().material.DOColor(Color.white,0.25f).OnComplete( ()=>{ other.transform.parent.GetComponent<Renderer>().material.DOColor(c,0.5f); } );
                }
            }
            else
            {
                RemoveArrows(StackCount);
            }
        }
    }

    public void AddNewStack(int count=1)
    {
        for (int i = 0; i < count; i++)
        {
            if(StackCount >= maxVisible)
            {
                StackCount++;
            }
            else
            {
                GameObject newArrow = arrowPrefab.InstantiniatePool();
                newArrow.transform.parent = transform;
                AddStack(newArrow);
                newArrow.transform.localPosition = GetNewLocationForNewArrow(StackCount);
                newArrow.GetComponent<Arrow>().SetValues(orbit);

                StackCount++;

                newArrow.transform.DOScale(new Vector3(1,1,1),Random.Range(0f,1f)).SetEase(Ease.OutBounce);

                if ((orbit + 2) * (orbit + 3) < StackCount)
                    orbit++;
            }
        }
        SetArrowCounterText();
    }

    public void RemoveArrows(int count,bool lostControl=true)
    {
        for (int i = 0; i < count; i++)
        {
            if(StackCount >= maxVisible)
            {
                StackCount--;
                SetArrowCounterText();
            }
            else
            {
                StackCount--;
                if (ControlLost(lostControl))
                {
                    return;
                }

                GetAnArrow().DestroyPool();

                if ((orbit + 2) * (orbit + 1) > StackCount)
                {
                    orbit--;
                }
            }
        }
    }

    public bool ControlLost(bool lostControl = true)
    {
        if (StackCount <= 0)
        {
            if (lostControl)
            {
                fail = true;
                if(!finishMode)
                {
                    gameManager.LevelFailed();
                }
                else
                {
                    gameManager.LevelCompleted();
                }
            }
            return true;
        }
        return false;
    }

    public Vector3 GetNewLocationForNewArrow(int index)
    {
        float _index = ((index ) - ((orbit + 1) * (orbit + 2)));
        float radian;
        if (_index != 0)
            radian = 2 * Mathf.PI / ((orbit + 3) * (orbit + 2) - (orbit + 1) * (orbit + 2)) * _index;
        else
            radian = 0;
        Vector3 spawnDir = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian),0);       
        return spawnDir * orbit * DISTANCEOFORBITS;
    }

    public void SetAllArrowsLocation(float x, float y)
    {
        foreach (Arrow item in stackObjects)
        {
            item.SetLocation(x, y);
        }
    }

    public Arrow GetAnArrow()
    {
        Arrow arrow = GetLastStack() as Arrow;
        arrow.transform.parent = null;
        RemoveStack();
        SetArrowCounterText();
        return arrow;
    }

    public void SetArrowCounterText()
    {
        counterText.text = StackCount.ToString();
    }

    private void OnDestroy()
    {
        TouchSystem.Touching -= TouchSystem_Touching;
    }
}
