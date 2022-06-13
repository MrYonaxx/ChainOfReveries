using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;
using Sirenix.OdinInspector;
using TMPro;

public class EventBattleTimer : MonoBehaviour
{

    [SerializeField]
    GameRunData runData;
    [SerializeField]
    BattleManager battleManager;
    [SerializeField]
    TextMeshProUGUI textTimer;

    [SerializeField]
    [SuffixLabel("en s")]
    int[] timer;

    float t = 0;
    bool start = false;
    int count = 0;

    // Start is called before the first frame update
    public void StartTimer()
    {
        textTimer.gameObject.SetActive(true);
        textTimer.text = ((int)t).ToString();
        t = timer[runData.Floor];
        start = true;

        count = BattleUtils.Instance.Characters.Count;
        for (int i = BattleUtils.Instance.Characters.Count - 1; i >= 0; i--)
        {
            if (BattleUtils.Instance.Characters[i].tag == "Enemy")
            {
                BattleUtils.Instance.Characters[i].CharacterKnockback.OnDeath += StopTimer;
            }
        }
    }


    public void StopTimer(CharacterBase character, DamageMessage dmg)
    {
        character.CharacterKnockback.OnDeath -= StopTimer;

        count -= 1;
        if (count <= 1)
        {
            start = false;
            textTimer.gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            t -= Time.deltaTime;
            textTimer.text = ((int)t).ToString();
            if (t<=0)
            {
                BattleFeedbackManager.Instance.BackgroundFlash();
                BattleFeedbackManager.Instance.ShakeScreen();
                start = false;
                for (int i = BattleUtils.Instance.Characters.Count-1; i >= 0; i--)
                {
                    BattleFeedbackManager.Instance.RippleScreen(BattleUtils.Instance.Characters[i].transform.position.x, BattleUtils.Instance.Characters[i].transform.position.y);
                    if (BattleUtils.Instance.Characters[i].tag == "Enemy")
                    {
                        BattleUtils.Instance.Characters[i].CharacterKnockback.Death();
                    }
                    else
                    {
                        BattleUtils.Instance.Characters[i].CharacterKnockback.IsInvulnerable = false;
                        DamageMessage dmg = new DamageMessage();
                        dmg.damage = 2000;
                        BattleUtils.Instance.Characters[i].CharacterKnockback.Hit(dmg);
                    }
                }
                textTimer.gameObject.SetActive(false);
            }
        }
    }
}
