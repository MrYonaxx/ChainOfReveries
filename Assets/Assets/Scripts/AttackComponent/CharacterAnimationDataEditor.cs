#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using UnityEditor.Experimental.SceneManagement;



namespace VoiceActing
{
    [CustomEditor(typeof(CharacterAnimationData))]
    public class CharacterAnimationDataEditor : OdinEditor
    {
        private float lastEditorTime = 0f;




        private bool isPlaying = false;
        private bool isLooping = false;

        private int sliderTime = 0;

        protected override void OnHeaderGUI()
        {
            //Debug.Log("Wesh");
            base.OnHeaderGUI();
        }
        protected override void OnEnable()
        {
            EditorApplication.playModeStateChanged += _OnPlayModeStateChange;
        }

        private void _OnPlayModeStateChange(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                StopAnimSimulation();
            }
        }


        protected override void OnDisable()
        {
            EditorApplication.playModeStateChanged -= _OnPlayModeStateChange;
            StopAnimSimulation();
        }





        // Inspector buttons


        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            GUILayout.BeginHorizontal();
            GUI.enabled = !isPlaying;
            if (GUILayout.Button("Play"))
            {
                StartAnimSimulation();
            }
            GUI.enabled = isPlaying;
            if (GUILayout.Button("Stop"))
            {
                StopAnimSimulation();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();


            CharacterAnimationData sampler = target as CharacterAnimationData;
            if (null != sampler)
            {
                if (null == sampler.AnimatorDebug) return;
                if (null == sampler.AnimationClipDebug) return;

                int maxTime = (int)(sampler.AnimationClipDebug.length * 60)-1;
                int time = sliderTime;
                sliderTime = EditorGUILayout.IntSlider(sliderTime, 0, maxTime);
                if (sliderTime != time) // Le slider a bougé
                {
                    StopAnimSimulation();
                    sampler.AnimationClipDebug.SampleAnimation(sampler.AnimatorDebug.gameObject, (sliderTime / 60f));
                }
            }
            GUILayout.Space(20);
            base.OnInspectorGUI();

        }





        // Animation Sampling


        private void OnEditorUpdate()
        {
            CharacterAnimationData sampler = target as CharacterAnimationData;
            if (null == sampler) return;
            if (null == sampler.AnimatorDebug) return;
            if (null == sampler.AnimationClipDebug) return;

            AnimationClip animClip = sampler.AnimationClipDebug;

            float animTime = Time.realtimeSinceStartup - lastEditorTime;
            if (animTime >= animClip.length)
            {
                StopAnimSimulation();
                if (isLooping)
                    StartAnimSimulation();
            }
            else
            {
                if (AnimationMode.InAnimationMode())
                {
                    AnimationMode.SampleAnimationClip(sampler.AnimatorDebug.gameObject, animClip, animTime);
                }
            }
        }



        public void StartAnimSimulation()
        {
            AnimationMode.StartAnimationMode();
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
            lastEditorTime = Time.realtimeSinceStartup;
            isPlaying = true;
        }

        public void StopAnimSimulation()
        {
            AnimationMode.StopAnimationMode();
            EditorApplication.update -= OnEditorUpdate;
            isPlaying = false;
        }
    }
}
#endif
