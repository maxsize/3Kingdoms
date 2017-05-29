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
    public class BladeFury : MovementBehaviour
    {
        [Inject]
        public Metadata Meta;

        private static readonly Vector3 RotateSpeed = Vector3.up * 15.1f;

        protected override void SetTarget(float latency)
        {
            var ability = Meta.Abilities.ToList().Find(a => a.Name == GetType().Name);
            if (ability.Name == null)
            {
                Debug.LogError(string.Format("Ability {0} is not defined.", GetType().Name));
                return;
            }
            var duration = ability.Levels[0].Duration - (latency / 1000);
            StartCoroutine(Wait(duration));
            StartCoroutine(LateEnd());
            gameObject.AddComponent<ParticleSystem>();
        }

        public override void End()
        {
        }

        private IEnumerator LateEnd()
        {
            yield return null;
            OnEnd.Invoke();
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(duration);
            enabled = false;
            var p = gameObject.GetComponent<ParticleSystem>();
            Destroy(p);
        }
    }
}