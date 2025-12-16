using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IInputService
{
    public void ShowPointer(bool value = true);
    public void Enable();
    public void Disable();
}
