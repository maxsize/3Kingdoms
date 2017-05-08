using UnityEngine;
using System.Collections;

public class InjectableBehaviour : MonoBehaviour
{

    // Use this for initialization
    protected virtual void Start()
    {
        this.Inject();
    }
}
