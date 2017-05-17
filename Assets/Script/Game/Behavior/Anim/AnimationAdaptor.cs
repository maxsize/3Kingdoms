using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Game.Event;
using ThreeK.Game.Behavior.Core;
using ThreeK.Game.Behavior.Movement;
using ThreeK.Game.Helper;
using UnityEngine;

namespace Game.Behavior.Anim
{
    [RequireComponent(typeof(Animator))]
    [InjectFromContainer(BindingHelper.Identifiers.PlayerContainer)]
    public class AnimationAdaptor : MonoBehaviour
    {
        private const string ATTACK1_TRIGGER = "Attack1Trigger";

        [Inject] public EventDispatcher Dispatcher;

        void Start()
        {
            this.Inject();
            Dispatcher.AddListener<TargetChangeEvent>(OnMovementChange);
        }

        void OnEnable()
        {
            if (Dispatcher != null)
                Dispatcher.AddListener<TargetChangeEvent>(OnMovementChange);
        }

        void OnDisable()
        {
            Dispatcher.RemoveListener<TargetChangeEvent>(OnMovementChange);
        }

        private void OnMovementChange(TargetChangeEvent e)
        {
            var animator = GetComponent<Animator>();
            switch (e.Data.ToString())
            {
                case "Spinner":
                case "Stander":
                    animator.SetBool("Moving", false);
                    animator.SetBool("Running", false);
                    break;
                case "Mover":
                case "Mover2":
                    animator.SetBool("Moving", true);
                    animator.SetBool("Running", true);
                    break;
                case "Attacker":
                    animator.SetTrigger(ATTACK1_TRIGGER);
                    break;
            }
        }
    }
}
