using Adic;
using Adic.Container;
using Adic.Injection;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    public class InputFactory : IFactory
    {
        [Inject] public IInjectionContainer Container;

        private Plane _ground = new Plane(Vector3.up, Vector3.zero);

        public object Create(InjectionContext context)
        {
            if (context.identifier.Equals(typeof(MoveInput)))
                return CreateMoveInput(context);
            return null;
        }

        private IInput CreateMoveInput(InjectionContext context)
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            float ent;
            if (!_ground.Raycast(ray, out ent))
                return null;
            var hitPoint = ray.GetPoint(ent);
            //Debug.DrawRay(ray.origin, ray.direction * ent, Color.green, 2);
            //Debug.Log(hitPoint);
            return new MoveInput(hitPoint);
        }
    }
}
