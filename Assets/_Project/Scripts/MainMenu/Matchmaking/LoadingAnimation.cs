using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Matchmaking
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private Image display;

        private void OnEnable()
        {
            AnimationRoutine();
        }

        private void OnDisable()
        {
            DOTween.KillAll();
        }

        private void AnimationRoutine()
        {
            float _duration = 1f;
            display.fillClockwise = true;
            display.fillAmount = 0;
            display.DOFillAmount(1, _duration).OnComplete(() =>
            {
                display.fillClockwise = !display.fillClockwise;
                display.DOFillAmount(0, _duration).OnComplete(AnimationRoutine);
            });
        }
    }
}