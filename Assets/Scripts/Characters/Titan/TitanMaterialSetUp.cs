using Networking;
using UnityEngine;

namespace Characters.Titan
{
    public class TitanMaterialSetUp : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer bodySurfaces;
        [SerializeField] private SkinnedMeshRenderer bodyJoints;
        [SerializeField] private Material transparentSurfaces;
        [SerializeField] private Material opaqueSurfaces;
        [SerializeField] private Material transparentJoints;
        [SerializeField] private Material opaqueJoints;
        
        

        public void Start()
        {
            PlayerObject localPlayerObject = PlayerObject.Local;
            if(localPlayerObject.IsTitan)
                SetUpAsTitan();
            else
                SetUpAsJaeger();
        }

        private void SetUpAsTitan()
        {
            bodySurfaces.sharedMaterial = transparentSurfaces;
            bodyJoints.sharedMaterial = transparentJoints;
            
            Debug.Log("Titan Body should be HIDDEN");
        }

        private void SetUpAsJaeger()
        {
            bodySurfaces.sharedMaterial = opaqueSurfaces;
            bodyJoints.sharedMaterial = opaqueJoints;
            
            Debug.Log("Titan Body should be VISIBLE");

        }
    }
}
