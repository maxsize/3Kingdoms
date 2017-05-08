using Adic;
using Adic.Container;
using Adic.Injection;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    public class InputFactory : IFactory
    {
        [Inject] public IInjectionContainer Container;

        public object Create(InjectionContext context)
        {
            if (context.identifier.Equals(typeof(MoveInput)))
                return CreateMoveInput(context);
            return null;
        }

        private IInput CreateMoveInput(InjectionContext context)
        {
            var camera = Camera.main;
            var fwd = camera.transform.TransformDirection(Vector3.forward);
            float dist = 0.0f;
            RaycastHit hit;
            bool hitted = Physics.Raycast(camera.transform.position, fwd, out hit);
            if (!hitted)
                return null;
            dist = Vector3.Distance(camera.transform.position, hit.point);
            var mousePos = UnityEngine.Input.mousePosition;
            mousePos.z = dist;
            //_target = new Vector3(277.3f, 122.4f, 65.1f);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            return new MoveInput(mousePos);
        }
    }
}
