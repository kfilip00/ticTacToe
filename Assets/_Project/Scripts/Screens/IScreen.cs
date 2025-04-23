using System;

public interface IScreen
{
    void Show(Action _callBack);
    void Close(Action _callBack);
}
