using Adic;
using Adic.Container;
using System;
using System.Collections;
using System.Collections.Generic;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;

namespace ThreeK.Game.UI
{
    public class MainUI : MonoStateMachine
    {
        [Inject] public IInjectionContainer Container;

	    // Use this for initialization
	    void Start () {
            this.Inject();
            Container.Bind<MainUI>().To(this);

            AddState(new HeroSelectionState());
            OnStateChange.AddListener(HandleStateChange);
	    }

        private void HandleStateChange(IState state)
        {
            DisableAll();
            var child = transform.Find(state.Data.ToString());
            child.gameObject.SetActive(true);
        }

        private void DisableAll()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
