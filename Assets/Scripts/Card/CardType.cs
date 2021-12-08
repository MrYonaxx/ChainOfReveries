/*****************************************************************
 * Product:    #PROJECTNAME#
 * Developer:  #DEVELOPERNAME#
 * Company:    #COMPANY#
 * Date:       #CREATIONDATE#
******************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace VoiceActing
{
    [CreateAssetMenu(fileName = "CardTypesData", menuName = "Data/CardTypes", order = 1)]
    public class CardType : ScriptableObject
    {
        [HorizontalGroup]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        private List<string> cardTypeName;
        public List<string> CardTypeName
        {
            get { return cardTypeName; }
        }

        [HorizontalGroup]
        [ListDrawerSettings(Expanded = true)]
        [SerializeField]
        private List<Color> cardTypeColor;
        public List<Color> CardTypeColor
        {
            get { return cardTypeColor; }
        }


        public int GetTypeIndex(string name)
        {
            for (int i = 0; i < cardTypeName.Count; i++)
            {
                if (cardTypeName[i].Equals(name))
                {
                    return i;
                }
            }
            return -1;
        }


        public Color GetColorType(string name)
        {
            for(int i = 0; i < cardTypeName.Count; i++)
            {
                if(cardTypeName[i].Equals(name))
                {
                    return cardTypeColor[i];
                }
            }
            return Color.black;
        }

        public Color GetColorType(int i)
        {
            return cardTypeColor[i];
        }

        public IEnumerable GetAllTypeName()
        {
            return cardTypeName;//.Select(x => new ValueDropdownItem(x.VariableName, x.VariableName));
        }
        public IEnumerable GetAllTypeID()
        {
            List<ValueDropdownItem<int>> res = new List<ValueDropdownItem<int>>();
            for (int i = 0; i < cardTypeName.Count; i++)
            {
                res.Add(new ValueDropdownItem<int>(cardTypeName[i], i));
            }
 
            return res;
        }
       
    } 

} // #PROJECTNAME# namespace