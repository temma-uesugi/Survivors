using System;

namespace App.Inputs
{
    public interface IActionInputs : IDisposable
    {
        void SetEnable(bool isEnable);
    }
}