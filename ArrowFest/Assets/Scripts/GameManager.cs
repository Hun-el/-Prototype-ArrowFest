using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject platformPrefab;
    public GameObject windowPrefab;
    public GameObject humanPrefab;
    public GameObject finishplatformPrefab;
    public GameObject finishRoadPrefab;
    public GameObject finishLevelPrefab;
    public GameObject completedCanvasPrefab;
    public GameObject failedCanvasPrefab;

    [Header("Spawn Locations")]
    public Transform platformLoc;
    public Transform windowLoc;
    public Transform humanLoc;

    [Header("Platform Settings")]
    [Range(2,15)][SerializeField] int finishPlatform;
    [Range(6,20)][SerializeField] int maxPlatform;
    [Range(2,10)][SerializeField] int minPlatform;

    [Header("Window Settings")]
    [Range(10,90)] public int windowPlusRate;
    [Range(10,100)] public int maxPositiveRate;
    [Range(10,90)] public int minPositiveRate;
    [Range(1,5)]public int positiveRateIncrease;

    [Header("Upgrade Settings")]
    [Range(0,1000)] public int startingFee;
    [Range(25,1000)] public int Increase;

    [Header("Player Settings")]
    [Range(1,10)] public float minSpeed;
    [Range(1,10)] public float maxSpeed;
    [Range(0.1f,1f)] public float speedIncrease;

    [Header("Others")]
    public GameObject mainPlatform;
    public bool readyforMove;

    GameObject platformClone;
    LevelSystem levelSystem;    
    int humanCount;

    private void Start() {
        levelSystem = FindObjectOfType<LevelSystem>();
        platformClone = mainPlatform;

        if(PlayerPrefs.GetInt("level") <= 18)
        {
            SetupPlatform(minPlatform + (PlayerPrefs.GetInt("level") / 3));
        }
        else
        {
            SetupPlatform(maxPlatform);
        }
    }

    private void Update() {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * 3);
    }

    void SetupPlatform(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Collider cloneColl = platformClone.GetComponent<Collider>();
            Vector3 v = new Vector3(cloneColl.bounds.center.x,cloneColl.bounds.center.y,cloneColl.bounds.max.z + (cloneColl.bounds.size.z / 2));
            platformClone = Instantiate(platformPrefab,v,Quaternion.identity);
            platformClone.transform.SetParent(platformLoc);

            if(i != count - 1)
            {
                SpawnWindow(platformClone);
                if(PlayerPrefs.GetInt("level") >= 5)
                {
                    SpawnHuman(platformClone,false);
                }
            }
            else
            {
                SpawnFinish(platformClone,finishRoadPrefab);
            }
        }
        SpawnFinishPlatform(platformClone);
    }

    void SpawnFinish(GameObject platformClone,GameObject finishType)
    {
        Collider col = platformClone.GetComponent<Collider>();
        GameObject finishClone = Instantiate(finishType,new Vector3(col.bounds.center.x,col.bounds.max.y,col.bounds.center.z),Quaternion.identity);
        finishClone.transform.SetParent(platformLoc);
    }

    void SpawnFinishPlatform(GameObject platformClone)
    {
        float a = 255f / finishPlatform;
        for(int i = 1; i <= finishPlatform; i++)
        {
            Collider col = platformClone.GetComponent<Collider>();
            GameObject finishplatformClone = Instantiate(finishplatformPrefab,new Vector3(col.bounds.center.x,col.bounds.center.y,col.bounds.max.z + 1 * i),Quaternion.identity);
            finishplatformClone.transform.SetParent(platformLoc);

            finishplatformClone.GetComponent<Renderer>().material.color = new Color(0f/255f, 0f/255f, (a)/255f);
            a += 255f / finishPlatform;

            SpawnHuman(finishplatformClone,true);

            if(i == finishPlatform)
            {
                SpawnFinish(finishplatformClone,finishLevelPrefab);
            }
        }
    }

    void SpawnWindow(GameObject platformClone)
    {
        Collider col = platformClone.GetComponent<Collider>();
        GameObject windowClone = Instantiate(windowPrefab,new Vector3(col.bounds.center.x,col.bounds.max.y,col.bounds.center.z),Quaternion.identity);
        windowClone.transform.SetParent(windowLoc);
    }

    void SpawnHuman(GameObject platformClone,bool center)
    {
        if(!center)
        {
            Collider col = platformClone.GetComponent<Collider>();
            float randomX = Random.Range(col.bounds.min.x,col.bounds.max.x);
            float randomZ = Random.Range(col.bounds.min.z,col.bounds.max.z);

            GameObject humanClone = Instantiate(humanPrefab,new Vector3(randomX,col.bounds.max.y,randomZ),new Quaternion(0,180,0,0));
            humanClone.GetComponent<Human>().health = 1;
            humanClone.transform.SetParent(humanLoc);
        }
        else
        {
            Collider col = platformClone.GetComponent<Collider>();

            GameObject humanClone = Instantiate(humanPrefab,new Vector3(col.bounds.center.x,col.bounds.max.y,col.bounds.center.z),new Quaternion(0,180,0,0));
            //humanClone.transform.localScale = new Vector3(1f + humanCount / 20f,1f + humanCount / 20f,1f + humanCount / 20f);
            humanClone.GetComponent<Human>().health = (1+humanCount) * (1 + (PlayerPrefs.GetInt("level") / 5));
            humanClone.transform.SetParent(platformClone.transform);
            humanCount++;
        }
    }

    public void LevelFailed()
    {
        Instantiate(failedCanvasPrefab);
        StartCoroutine(LevelUp(false));
    }
    public void LevelCompleted()
    {
        Instantiate(completedCanvasPrefab);
        StartCoroutine(LevelUp(true));
    }
    IEnumerator LevelUp(bool win)
    {
        if(win)
        {
            levelSystem.LevelUp();
        }
        yield return new WaitForSeconds(1.5f);
        LoadingSystem loadingSystem = GetComponent<LoadingSystem>();
        loadingSystem.LoadLevel("Restart");
    }

    public void Ready()
    {
        readyforMove = true;
    }

    private void OnApplicationQuit() {
        PlayerPrefs.DeleteAll();
    }
}
