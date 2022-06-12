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
        TMPro.TextMeshProUGUI textHealthValue = null;
        [SerializeField]
        RectTransform healthGaugeOutline = null;
        [SerializeField]
        RectTransform healthGauge = null;

        [SerializeField]
        RectTransform transformBarNumber = null;
        [SerializeField]
        Image transformBar = null;

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

        [Title("Armors")]
        [SerializeField]
        RectTransform armorsList = null;
        [SerializeField]
        Image armorIcon = null;

        [Title("Parameter")]
        [SerializeField]
        int healthBarAmount = 1000;
        [SerializeField]
        float rVAmount = 30;

        [SerializeField]
        float sizeHealthBar = 1000;
        [SerializeField]
        Vector2 sizeRVGauge = new Vector2(100, 400);

        [Title("Health Feedback")]
        [SerializeField]
        Animator animatorHealthLost = null;
        [SerializeField]
        Animator animatorRevengeValue = null;

        bool showRV = false;
        int previousHealth = 0;
        int previousHealthMax = 0;
        List<Image> healthBarList = new List<Image>();
        List<Image> armorIconsList = new List<Image>();


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
            this.gameObject.SetActive(true);

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

            // décale le nom si il y a trop de barre de vie
            //if (healthBarList.Count > 8)
            textCharacterName.gameObject.SetActive(!(healthBarList.Count > 8));//.rectTransform.anchoredPosition += new Vector2(0, 20);

            // Affiche la première barre de vie un peu différemment
            healthBarHidden.gameObject.SetActive(maxHp >= healthBarAmount);

            previousHealth = (int)hp;
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

            // Dessine le texte hp
            if (textHealthValue != null)
                textHealthValue.text = hp.ToString();

            // Affiche le bon nombre de barre de vie
            int currentHealth = 0;
            for (int i = healthBarList.Count - 1; i >= 0; i--)
            {
                currentHealth += healthBarAmount;
                if (hp >= currentHealth)
                    healthBarList[i].enabled = true;
                else
                    healthBarList[i].enabled = false;
            }


            // encore un cas particulier ; quand on meurt en Reverie 3 et plus, le buff de vie disparait et nique l'affichage
            if (hp == 0)
            {
                animatorHealthLost.transform.localScale = new Vector3(healthGauge.transform.localScale.x, 1, 1);
                healthGauge.transform.localScale = new Vector3(0, 1, 1);
                animatorHealthLost.SetTrigger("Feedback");
                previousHealth = hp;
                return;
            }

            if (maxHp < healthBarAmount)
            {
                // Il n'y a qu'une seule barre de vie
                healthGaugeOutline.sizeDelta = new Vector2((maxHp / (float)healthBarAmount) * sizeHealthBar, healthGaugeOutline.sizeDelta.y);
                healthGauge.transform.localScale = new Vector3(hp / (float)Mathf.Max(1, maxHp), 1, 1);
                animatorHealthLost.transform.localScale = new Vector3(previousHealth / (float)Mathf.Max(1, maxHp), 1, 1);
            }
            else if (hp >= currentHealth)
            {
                // C'est la première barre de vie
                healthGaugeOutline.sizeDelta = new Vector2(((maxHp - currentHealth) / (float)healthBarAmount) * sizeHealthBar, healthGaugeOutline.sizeDelta.y);
                healthGauge.transform.localScale = new Vector3((hp % healthBarAmount) / (float)Mathf.Max(maxHp - currentHealth, 1), 1, 1);
                animatorHealthLost.transform.localScale = new Vector3((previousHealth % healthBarAmount) / (float)Mathf.Max(maxHp - currentHealth, 1), 1, 1);
            }
            else
            {
                // Barre de vie du milieu
                healthGaugeOutline.sizeDelta = new Vector2(sizeHealthBar, healthGaugeOutline.sizeDelta.y);
                healthGauge.transform.localScale = new Vector3((hp % healthBarAmount) / (float)healthBarAmount, 1, 1);

                if ((hp % healthBarAmount) > (previousHealth % healthBarAmount))
                    animatorHealthLost.transform.localScale = new Vector3(1, 1, 1);
                else
                    animatorHealthLost.transform.localScale = new Vector3((previousHealth % healthBarAmount) / (float)healthBarAmount, 1, 1);
            }

            animatorHealthLost.SetTrigger("Feedback");
            previousHealth = hp;
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
                    if (rVMaxGauge.gameObject.activeInHierarchy)
                        lerpRV.StartLerp(canvasRV.alpha, 1f, (startValue, t) => { canvasRV.alpha = Mathf.Lerp(startValue, 0, t); });
                }
            }
            else
            {
                // Show Revenge value gauge
                if (!showRV)
                {
                    showRV = true;
                    rVMaxGauge.gameObject.SetActive(true);
                    lerpRV.StartLerp(canvasRV.alpha, 1f, (startValue, t) => { canvasRV.alpha = Mathf.Lerp(startValue, 1, t); });
                }
                animatorRevengeValue.SetBool("Appear", rV == maxRV); 
            }

        }

        public void DrawArmors(List<CardEquipment> equipments)
        {
            // Draw Armors
            int index = 0;
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].GetCardType() == 1)
                {
                    if (index <= armorIconsList.Count)
                        armorIconsList.Add(Instantiate(armorIcon, armorsList));
                    armorIconsList[index].sprite = equipments[i].GetCardIcon();
                    armorIconsList[index].gameObject.SetActive(true);
                    index++;
                }
            }
            for (int i = index; i < armorIconsList.Count; i++)
            {
                armorIconsList[i].gameObject.SetActive(false);
            }
        }


        #endregion

    } 

} // #PROJECTNAME# namespace