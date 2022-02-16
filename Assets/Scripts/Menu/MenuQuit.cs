using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

namespace Menu
{
    public class MenuQuit : MonoBehaviour
    {
        [SerializeField]
        InputController[] inputs = null;

        [SerializeField]
        MenuList menu = null;
        [SerializeField]
        GameObject fade = null;
        [SerializeField]
        string scene = "";

        private void Awake()
        {
            menu.OnEnd += Quit;
        }

        private void OnDestroy()
        {
            menu.OnEnd -= Quit;
        }


        private void Quit()
        {
            StartCoroutine(QuitCoroutine());
        }

        private IEnumerator QuitCoroutine()
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                inputs[i].enabled = false;
            }
            fade.SetActive(true);
            yield return new WaitForSeconds(1f);
            UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
        }
    }
}
