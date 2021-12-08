#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;

[InitializeOnLoad]
public class PrefabUpdater
{
    static PrefabUpdater()
    {
        Debug.Log("Prefab Updater armé et paré");
        //PrefabStage.prefabStageClosing += UpdateComponents; // fuite mémoire ? si y'en a cest la faute de la doc d'unity
        PrefabStage.prefabSaving += UpdateComponents2;
    }

    /*static void UpdateComponents(PrefabStage p)
    {
        VoiceActing.CharacterAnimationData sampler = p.prefabContentsRoot.GetComponentInChildren<VoiceActing.CharacterAnimationData>();
        if (sampler != null)
        {
            sampler.GetFrameData();
            Debug.Log(sampler.FrameData.Count);
            Debug.Log("J'update Character Animation Data");
            EditorUtility.SetDirty(sampler);
            PrefabUtility.RecordPrefabInstancePropertyModifications(sampler);
        }
    }*/

    static void UpdateComponents2(GameObject p)
    {
        VoiceActing.AttackManager attackManager = p.GetComponent<VoiceActing.AttackManager>();
        if (p.GetComponent<VoiceActing.AttackManager>() == null)
            return;
        attackManager.CharacterAnimationData.GetFrameData();
        attackManager.SetAttackControllers();
        //Debug.Log(p);
    }
}
#endif