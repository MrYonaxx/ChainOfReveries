using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoiceActing;

public class EasterEggMeneras : MonoBehaviour
{

    [SerializeField]
    CharacterBase character = null;
    [SerializeField]
    CharacterData nelys;
    [SerializeField]
    AttackManager introNelys;

    bool intro = false;

    // Start is called before the first frame update
    void Start()
    {
        character.LockController.OnTargetLock += CheckNelys;
    }


    void OnDestroy()
    {
        if(intro == false)
            character.LockController.OnTargetLock -= CheckNelys;
    }


    void CheckNelys(CharacterBase target)
    {
        if (target == null)
            return;
        if(target.CharacterData == nelys)
        {
            target.CharacterAction.Action(introNelys);
            target.LockController.OnTargetLock -= CheckNelys;
            intro = true;
            character.SetCharacterMotionSpeed(0, 3f);
        }
    }
}
