using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject water;
    [SerializeField] private Material skyBoxOriginal;
    [SerializeField] private Material skyBoxCheap;
    [SerializeField] private GameObject mirror;
    

    private bool original = true;
                    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
            water.SetActive(!water.activeInHierarchy);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            RenderSettings.fog = !RenderSettings.fog;

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            original = !original;
            RenderSettings.skybox = original ? skyBoxOriginal : skyBoxCheap;
        }
        
        if(Input.GetKeyDown(KeyCode.Alpha4))
            mirror.SetActive(!mirror.activeInHierarchy);
    }
}
