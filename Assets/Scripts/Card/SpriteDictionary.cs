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
    [CreateAssetMenu(fileName = "SpriteDictionary", menuName = "Data/SpriteDictionary", order = 1)]
    public class SpriteDictionary : ScriptableObject
    {
        [HorizontalGroup]
        [SerializeField]
        [ListDrawerSettings(Expanded = true)]
        private List<Sprite> sprites;
        public List<Sprite> Sprites
        {
            get { return sprites; }
        }


       

        public Sprite GetSprite(int i)
        {
            return sprites[i];
        }

        /*public IEnumerable GetAllTypeName()
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
        }*/
       
    } 

} // #PROJECTNAME# namespace