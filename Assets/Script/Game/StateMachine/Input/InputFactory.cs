using System.Linq;
using Adic;
using Adic.Container;
using Adic.Injection;
using Assets.Script.Game.Data;
using ThreeK.Game.Data;
using ThreeK.Game.Helper;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    public class InputFactory : IFactory
    {
        [Inject] public PlayerVO Player;
        [Inject] public Metadata Meta;

        private Plane _ground = new Plane(Vector3.up, Vector3.zero);

        public object Create(InjectionContext context)
        {
            IInput input = null;
            if (context.memberType == typeof(MoveInput))
                input = CreateMoveInput(context);
            else if (context.memberType == typeof(AttackInput))
                input = CreateAttackInput(context);
            else if (context.memberType == typeof(CastInput))
                input = CreateCastInput(context);

            if (input != null)
            {
                InputHelper.CurrentInput = input;
            }
            return input;
        }

        private IInput CreateCastInput(InjectionContext context)
        {
            // Assuming using BladeFury
            var type = Meta.Abilities[0].AbilityTypes[0];
            var ability = Meta.Abilities.ToList().Find(a => a.Name == Player.Abilities[0]);
            CastInput input = null;
            if (type == (int)AbilityTypes.NoTarget)
                input = new CastInput(ability);
            if (type == (int)AbilityTypes.PointTarget)
                input = new CastInput(GetHitPointOnGround(), ability);
            if (type == (int)AbilityTypes.UnitTarget)
            {
                var hit = GetHitEnemy();
                if (hit == null)
                    return null;
                input = new CastInput(hit.transform, ability);
            }

            return input;
        }

        private IInput CreateAttackInput(InjectionContext context)
        {
            var hit = GetHitEnemy();
            if (hit == null)
                return null;
            return new AttackInput(hit.transform);
        }

        private IInput CreateMoveInput(InjectionContext context)
        {
            var hitPoint = GetHitPointOnGround();
            if (hitPoint == Vector3.zero)
                return null;
            return new MoveInput(hitPoint);
        }

        private Vector3 GetHitPointOnGround()
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            float ent;
            if (!_ground.Raycast(ray, out ent))
                return Vector3.zero;
            var hitPoint = ray.GetPoint(ent);
            Debug.DrawRay(ray.origin, ray.direction * ent, Color.green, 6);
            return hitPoint;
        }

        private Transform GetHitEnemy()
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            //var dist = 50f;
            var index = LayerMask.NameToLayer("Enemy");
            LayerMask mask = 1 << index;
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
                return null;
            return hit.transform;
        }
    }
}
