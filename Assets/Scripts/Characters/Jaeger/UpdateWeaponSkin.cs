using UnityEngine;

namespace Characters.Jaeger
{
    public class UpdateWeaponSkin : MonoBehaviour
    {
        [SerializeField] private string[] skinIds;
        [SerializeField] private Material[] skinMaterials;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    

        public void UpdateSkin(string equippedSkin)
        {
            for(int i = 0; i < skinIds.Length; i++)
            {
                if (skinIds[i] == equippedSkin)
                {
                    var materials = skinnedMeshRenderer.materials;
                    materials[0] = skinMaterials[i];
                    skinnedMeshRenderer.materials = materials;
                    Debug.Log("Updated Skin to be: " + skinMaterials[i].name);
                }
            }
        }
    }
}
