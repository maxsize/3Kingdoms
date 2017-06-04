using UnityEngine;
using System.Linq;
using ThreeK.Game.Behavior.Core;
using Adic;
using Assets.Script.Game.Data;
using UnityEngine.Assertions;
using System.Collections;
using ThreeK.Game.Helper;

namespace ThreeK.Game.Behavior.Movement.Cast
{
    [InjectFromContainer(BindingHelper.Identifiers.MainContainer)]
    public class Skewer : MovementBehaviour
    {
        [Inject]
        public Metadata Meta;

        private float _speed = 5f;
        private Vector3 _destination;

        protected override void SetTarget(Vector3 point, float latency)
        {
            var ability = Meta.Abilities.ToList().Find(a => a.Name == GetType().Name);
            if (ability.Name == null)
            {
                Debug.LogError(string.Format("Ability {0} is not defined.", GetType().Name));
                return;
            }
            _destination = point;
            //var duration = ability.Levels[0].Duration - (latency / 1000);
            transform.Translate(Vector3.forward * _speed * latency);
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * _speed * Time.fixedDeltaTime);
            if (IsReached())
            {
                //Debug.Log(transform.position + " - " + _target + " - " + Vector3.Distance(transform.position, _target));
                transform.position = _destination;   // Correct the final position
                OnEnd.Invoke();
                enabled = false;
            }
        }

        private bool IsReached()
        {
            return Vector3.Distance(_destination, transform.position) < 0.2f;
        }

        public override void End()
        {
        }
    }
}