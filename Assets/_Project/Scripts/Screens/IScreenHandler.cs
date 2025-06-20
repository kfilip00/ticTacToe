using System;

public interface IScreenHandler
{
    void ShowScreen(IScreen _screen, Action _callBack);
}
