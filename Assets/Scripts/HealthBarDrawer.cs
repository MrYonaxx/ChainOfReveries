/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    public class HealthBarDrawer: MonoBehaviour
    {
        #region Attributes 

        /* ======================================== *\
         *               ATTRIBUTES                 *
        \* ======================================== */
        [Title("Data")]
        [SerializeField]
        Image characterFace = null;
        [SerializeField]
        Image characterFaceOutline = null;
        [SerializeField]
        TextMeshProUGUI textCharacterName = null;

        [Title("Health")]
        [SerializeField]
        GaugeDrawer healthGauge = null;
        [SerializeField]
        RectTransform transformBarNumber = null;
        [SerializeField]
        Image transformBar = null;
        [SerializeField]
        RectTransform firstHealthBar = null;
        [SerializeField]
        RectTransform healthBarHidden = null;

        [Title("RevengeValue")]
        [SerializeField]
        RectTransform rVMaxGauge = null;
        [SerializeField]
        GaugeDrawer rVGauge = null;
        [SerializeField]
        Feedbacks.GenericLerp lerpRV = null;
        [SerializeField]
        CanvasGroup canvasRV = null;

        [Title("Parameter")]
        [SerializeField]
        int healthBarAmount = 1000;
        [SerializeField]
        float rVAmount = 30;
        [SerializeField]
        Vector2 sizeRVGauge = new Vector2(100, 400);

        [Title("Health Feedback")]
        [SerializeField]
        Animator animatorHealthLost = null;

        bool showRV = false;
        int previousHealth = 0;
        int previousHealthMax = 0;
        List<Image> healthBarList = new List<Image>();


        #endregion

        #region GettersSetters 

        /* ======================================== *\
         *           GETTERS AND SETTERS            *
        \* ======================================== */

        #endregion

        #region Functions 

        /* ======================================== *\
         *                FUNCTIONS                 *
        \* ======================================== */
        public void DrawCharacter(CharacterData characterData, CharacterStat characterStat)
        {
            // Draw data
            textCharacterName.text = characterData.CharacterName;
            if (characterData.CharacterFace == null)
            {
                characterFace.enabled = false;
                characterFaceOutline.enabled = false;
            }
            else
            {
                characterFace.enabled = true;
                characterFaceOutline.enabled = true;
            }
            characterFace.sprite = characterData.CharacterFace;
            characterFaceOutline.sprite = characterFace.sprite;

            // Draw Health
            previousHealthMax = (int)characterStat.HPMax.Value;
            CreateHealth((int)characterStat.HP, (int)characterStat.HPMax.Value);
            DrawHealth((int)characterStat.HP, (int)characterStat.HPMax.Value);

            // Revenge value
            if (characterStat.RevengeValue.Value <= 0)
            {
                rVMaxGauge.gameObject.SetActive(false);
                return;
            }
            rVMaxGauge.gameObject.SetActive(true);
            DrawRevengeValue(0, characterStat.RevengeValue.Value);
        }


        public void CreateHealth(float hp, float maxHp)
        {
            // Calcule le nombre de barre de vie
            int healthBarNumber = (int)(maxHp / (float)healthBarAmount);
            healthBarNumber = Mathf.Max(0, healthBarNumber);
            for (int i = 0; i < healthBarNumber; i++)
            {
                if (healthBarList.Count <= i)
                    healthBarList.Add(Instantiate(transformBar, transformBarNumber));
                healthBarList[i].gameObject.SetActive(true);
            }
            for (int i = healthBarNumber; i < healthBarList.Count; i++)
            {
                healthBarList[i].gameObject.SetActive(false);
            }
            if (healthBarList.Count == 0)
                transformBarNumber.gameObject.SetActive(false);
            previousHealth = (int)hp;

            // Affiche la première barre de vie un peu différemment
            healthBarHidden.gameObject.SetActive(false);
            firstHealthBar.gameObject.SetActive(false);

            int firstBarAmount = (int)maxHp % healthBarAmount;
            if (previousHealth >= maxHp - firstBarAmount && previousHealth >= firstBarAmount)
            {
                healthBarHidden.gameObject.SetActive(true);
                firstHealthBar.gameObject.SetActive(true);
                firstHealthBar.transform.localScale = new Vector3(firstBarAmount / (float)healthBarAmount, 1, 1);

            }        
        }


        // Vu que les stats sont des float pour être le plus "générique" possible je dois faire cette fonction un peu ghetto
        public void DrawHealth(float hp, float maxHp)
        {
            if (previousHealthMax != (int)maxHp)
                CreateHealth(hp, maxHp);

            DrawHealth((int)hp, (int)maxHp);
        }

        public void DrawHealth(int hp, int maxHp)
        {
            maxHp = Mathf.Max(1, maxHp);
            // Pour que ça s'affiche bien quand on a un multiple de healthBar PV
            if(hp > 0 && hp % healthBarAmount == 0)
                healthGauge.DrawGauge(healthBarAmount, healthBarAmount);
            else
                healthGauge.DrawGauge(hp % healthBarAmount, Mathf.Min(maxHp, healthBarAmount));
            healthGauge.DrawGaugeText(hp.ToString());

            int currentHealth = 0;
            for (int i = healthBarList.Count-1; i >= 0; i--)
            {
                currentHealth += healthBarAmount;
                if (hp >= currentHealth)
                    healthBarList[i].enabled = true;
                else
                    healthBarList[i].enabled = false;
            }

            animatorHealthLost.transform.localScale = new Vector3((float)(previousHealth % healthBarAmount) / (float)healthBarAmount, 1, 1);
            animatorHealthLost.SetTrigger("Feedback");
            previousHealth = hp;

            // Affiche la première barre de vie un peu différemment
            int firstBarAmount = maxHp % healthBarAmount;
            if (previousHealth >= maxHp - firstBarAmount && previousHealth >= firstBarAmount)
            {
                healthBarHidden.gameObject.SetActive(true);
                firstHealthBar.gameObject.SetActive(true);
            }
            else
            {
                firstHealthBar.gameObject.SetActive(false);
                healthBarHidden.gameObject.SetActive(false);
            }
        }

        public void DrawRevengeValue(float rV, float maxRV)
        {
            float sizeRatio = maxRV / rVAmount;
            rVMaxGauge.sizeDelta = new Vector2(sizeRVGauge.x + (sizeRVGauge.y - sizeRVGauge.x) * sizeRatio, rVMaxGauge.sizeDelta.y);
            rVGauge.DrawGauge(rV, maxRV);

            if(rV <= 0)
            {
                // Hide Revenge value gauge
                if (showRV)
                {
                    showRV = false;
                    lerpRV.StartLerp(canvasRV.alpha, 1f, (startValue, t) => { canvasRV.alpha = Mathf.Lerp(startValue, 0, t); });
                }
            }
            else
            {
                // Show Revenge value gauge
                if (!showRV)
                {
                    showRV = true;
                    lerpRV.StartLerp(canvasRV.alpha, 1f, (startValue, t) => { canvasRV.alpha = Mathf.Lerp(startValue, 1, t); });
                }
            }
        }


        #endregion

    } 

} // #PROJECTNAME# namespace