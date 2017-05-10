using Adic;
using Adic.Container;
using Adic.Injection;
using ThreeK.Game.Helper;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    public class InputFactory : IFactory
    {
        [Inject] public IInjectionContainer Container;

        private Plane _ground = new Plane(Vector3.up, Vector3.zero);

        public object Create(InjectionContext context)
        {
            IInput input = null;
            if (context.identifier.Equals(typeof(MoveInput)))
                input = CreateMoveInput(context);
            if (context.identifier.Equals(typeof(AttackInput)))
                input = CreateAttackInput(context);

            if (input != null)
            {
                //var subContainer = Container.Resolve<IInjectionContainer>("SubContainer");
                //// Bind current input
                //subContainer.Unbind(BindingHelper.Identifiers.CurrentInput);
                //subContainer.Bind<IInput>().To(input).As(BindingHelper.Identifiers.CurrentInput);
                InputHelper.CurrentInput = input;
            }
            return input;
        }

        private IInput CreateAttackInput(InjectionContext context)
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            //var dist = 50f;
            var index = LayerMask.NameToLayer("Enemy");
            LayerMask mask = 1 << index; 
            RaycastHit hit;
            if (!Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
                return null;
            return new AttackInput(hit.transform);
        }

        private IInput CreateMoveInput(InjectionContext context)
        {
            var ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            float ent;
            if (!_ground.Raycast(ray, out ent))
                return null;
            var hitPoint = ray.GetPoint(ent);
            Debug.DrawRay(ray.origin, ray.direction * ent, Color.green, 6);
            //Debug.Log(hitPoint);
            return new MoveInput(hitPoint);
        }
    }
}
