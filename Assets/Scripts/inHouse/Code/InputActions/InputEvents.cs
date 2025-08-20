using System;
using GameManager;
using UnityEngine;
using Utility;
namespace InputActons
{
    public class InputEvents : SingletonMonoBehaviour<InputEvents>
    {

        public event EventHandler OnQPress;

        private void Update()
        {
            if (GameInput.Instance.IsQKeyPerformedThisFrame())
            {
                
                OnQPress?.Invoke(this , EventArgs.Empty);
            }
        }
    }
}