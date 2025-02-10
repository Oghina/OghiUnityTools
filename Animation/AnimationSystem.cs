using System;
using System.Collections.Generic;
using MEC;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace OghiUnityTools.Animation
{
    public class AnimationSystem
    {
        PlayableGraph playableGraph;

        readonly AnimationMixerPlayable topLevelMixer;

        AnimationClipPlayable oneShotPlayable;

        CoroutineHandle blendInHandle;
        CoroutineHandle blendOutHandle;

        public event Action OnOneShotFinished;

        public AnimationSystem(Animator animator, RuntimeAnimatorController runtimeController)
        {
            playableGraph = PlayableGraph.Create("AnimationSystem");

            AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", animator);

            topLevelMixer = AnimationMixerPlayable.Create(playableGraph, 2);
            playableOutput.SetSourcePlayable(topLevelMixer);

            //    locomotionMixer = AnimationMixerPlayable.Create(playableGraph, 3); // Update to 3 for idle, walk, and run
            var locomotionMixer1 = AnimatorControllerPlayable.Create(playableGraph, runtimeController);

            topLevelMixer.ConnectInput(0, locomotionMixer1, 0);
            playableGraph.GetRootPlayable(0).SetInputWeight(0, 1f);

            playableGraph.Play();
        }

        public void PlayOneShot(AnimationClip oneShotClip)
        {
            // If the current animation clip is playing return si it doesn't get played again
            if (oneShotPlayable.IsValid() && oneShotPlayable.GetAnimationClip() == oneShotClip) return;

            InterruptOneShot();
            oneShotPlayable = AnimationClipPlayable.Create(playableGraph, oneShotClip);
            topLevelMixer.ConnectInput(1, oneShotPlayable, 0);

            // Calculate blendDuration as 10% of clip length
            // but ensure that is's not less than 0.1f or more than half the clip length
            float blendDuration = Mathf.Max(0.1f, Mathf.Min(oneShotClip.length * 0.1f, oneShotClip.length / 2));

            BlendIn(blendDuration);
            BlendOut(blendDuration, oneShotClip.length - blendDuration);
        }

        private void BlendIn(float blendDuration)
        {
            blendInHandle = Timing.RunCoroutine(Blend(blendDuration, blendTime =>
            {
                float weight = Mathf.Lerp(1f, 0f, blendTime);
                topLevelMixer.SetInputWeight(0, weight);
                topLevelMixer.SetInputWeight(1, 1f - weight);
            }));
        }

        private void BlendOut(float blendDuration, float delay)
        {
            blendOutHandle = Timing.RunCoroutine(Blend(blendDuration, blendTime =>
            {
                float weight = Mathf.Lerp(0f, 1f, blendTime);
                topLevelMixer.SetInputWeight(0, weight);
                topLevelMixer.SetInputWeight(1, 1f - weight);
            }, delay, DisconnectOneShot));
        }

        IEnumerator<float> Blend(float duration, Action<float> blendCallback, float delay = 0f, Action finishedCallback = null)
        {
            if (delay > 0f)
            {
                yield return Timing.WaitForSeconds(delay);
            }

            float blendTime = 0f;
            while (blendTime < 1f)
            {
                blendTime += Time.deltaTime / duration;
                blendCallback(blendTime);
                yield return blendTime;
            }

            blendCallback(1f);

            finishedCallback?.Invoke();
        }

        private void InterruptOneShot()
        {
            Timing.KillCoroutines(blendInHandle);
            Timing.KillCoroutines(blendOutHandle);

            topLevelMixer.SetInputWeight(0, 1f);
            topLevelMixer.SetInputWeight(1, 0f);

            if (oneShotPlayable.IsValid())
            {
                DisconnectOneShot();
            }
        }

        private void DisconnectOneShot()
        {
            topLevelMixer.DisconnectInput(1);
            playableGraph.DestroyPlayable(oneShotPlayable);
            OnOneShotFinished?.Invoke();
        }

        public void Destroy()
        {
            if (playableGraph.IsValid())
            {
                playableGraph.Destroy();
            }
        }
    }
}
