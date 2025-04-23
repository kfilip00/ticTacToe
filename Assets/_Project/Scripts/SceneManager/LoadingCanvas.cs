using UnityEngine;
using UnityEngine.UI;

public class LoadingCanvas : SceneManagement.LoadingCanvas
{
    [SerializeField] private Image progressBar;
    
    public override void UpdateProgress(float _progressPercentage)
    {
        if (progressBar==null)
        {
            return;
        }
        
        progressBar.fillAmount = _progressPercentage;
    }
}
