using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSystem : MonoBehaviour
{
    Animator animator;

    private void Start() 
    {
        animator = GameObject.FindWithTag("LevelLoader").GetComponent<Animator>();
    }

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadLevelCo(levelName));
    }

    IEnumerator LoadLevelCo(string levelName)
    {
        if(levelName == "Restart")
        {
            levelName = SceneManager.GetActiveScene().name;
            animator.SetTrigger("End");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(levelName);
        }
        else
        {
            animator.SetTrigger("End");
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(levelName);
        }
    }
}
