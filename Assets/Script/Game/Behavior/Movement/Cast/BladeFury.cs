using UnityEngine;
using System.Linq;
using ThreeK.Game.Behavior.Core;
using Adic;
using Assets.Script.Game.Data;
using UnityEngine.Assertions;
using System.Collections;

namespace ThreeK.Game.Behavior.Movement.Cast
{
    public class BladeFury : MovementBehaviour
    {
        [Inject] public Metadata Meta;

        private static Vector3 _rotateSpeed = Vector3.up * 1.1f;

        protected override void SetTarget()
        {
            var ability = Meta.Abilities.ToList().Find(a => a.Name == GetType().Name);
            if (ability.Name == null)
            {
                Debug.LogError(string.Format("Ability {0} is not defined.", GetType().Name));
                return;
            }
            var duration = ability.Levels[0].Duration;
            StartCoroutine(Wait(duration));
        }

        public override void End()
        {
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(duration);
            OnEnd.Invoke();
            enabled = false;
        }

        private void Update()
        {
            gameObject.GetComponent<Spinner>().enabled = false;
            transform.Rotate(_rotateSpeed);
        }
    }
}
