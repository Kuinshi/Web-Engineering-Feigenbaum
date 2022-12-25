using Fusion;
using UnityEngine;

public class JaegerController : NetworkBehaviour
{
    [SerializeField] private GameObject fpsCamera;

    public override void Spawned()
    {
        base.Spawned();
        fpsCamera.SetActive(HasInputAuthority);
    }
}
