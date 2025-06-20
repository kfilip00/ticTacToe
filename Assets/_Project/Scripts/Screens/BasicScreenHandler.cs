using System;
using System.Collections;
using UnityEngine;

public class BasicScreenHandler : MonoBehaviour,IScreenHandler
{
    private IScreen currentScreen;
    
    public void ShowScreen(IScreen _screen, Action _callBack=null)
    {
        if (_screen == currentScreen)
        {
            return;
        }

        StartCoroutine(ShowScreenRoutine(_screen,_callBack));
    }
    
    IEnumerator ShowScreenRoutine(IScreen _screen, Action _callBack)
    {
        bool _continue;
        if (currentScreen != null)
        {
            _continue = false;
            currentScreen.Close(AllowContinue);
            yield return new WaitUntil(() => _continue);
        }

        _continue = false;
        _screen.Show(AllowContinue);
        yield return new WaitUntil(() => _continue);
        currentScreen = _screen;
        _callBack?.Invoke();
        yield break;

        void AllowContinue()
        {
            _continue = true;
        }
    }
}
