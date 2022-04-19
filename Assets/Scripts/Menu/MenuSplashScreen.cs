using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VoiceActing 
{
    public class MenuSplashScreen : MonoBehaviour
    {
        [SerializeField]
        string sceneName;

        [SerializeField]
        AudioClip music;


        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SplashScreenCoroutine());
            //StartCoroutine(LoadSceneCoroutine());
        }

        IEnumerator SplashScreenCoroutine()
        {
            /*yield return new WaitForSeconds(0.2f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;*/

            yield return new WaitForSeconds(3f);
           // AudioManager.Instance.PlayMusic(music);
            yield return new WaitForSeconds(2f);

            /*asyncLoad.allowSceneActivation = true; 
            
            while (!asyncLoad.isDone)
            {
                yield return null;
            }*/
            AudioManager.Instance.PlayMusic(music);
            SceneManager.LoadScene(sceneName);
        }

        /*private IEnumerator LoadSceneCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            asyncLoad.allowSceneActivation = false;

            yield return new WaitForSeconds(5f); 

            asyncLoad.allowSceneActivation = true;

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }*/
    }
}
