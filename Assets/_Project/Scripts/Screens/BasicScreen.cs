using System;
using UnityEngine;

public class BasicScreen : MonoBehaviour,IScreen
{
    [SerializeField] private GameObject holder;
    
    public void Show(Action _callBack)
    {
        holder.SetActive(true);    
        Setup();
        _callBack?.Invoke();
    }

    protected virtual void Setup()
    {
        
    }

    public void Close(Action _callBack)
    {
        Hide();
        holder.SetActive(false);    
        _callBack?.Invoke();
    }

    protected virtual void Hide()
    {
        
    }
}
