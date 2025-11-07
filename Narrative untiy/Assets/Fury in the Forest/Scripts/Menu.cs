using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Animator foxMenu;
    public GameObject zzz;
    
    public void GameStart()
    { 
        foxMenu.SetBool("Wakeup", true);
        zzz.SetActive(false);
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene(1);
    }
}
