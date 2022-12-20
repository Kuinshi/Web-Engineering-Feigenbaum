using UnityEngine;

public class TestIKWebGl : MonoBehaviour
{
    [SerializeField] private Transform leftHandTarget;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform headTarget;
    [SerializeField] private Transform hipTarget;
    
    

    public void RepositionStuff()
    {
        hipTarget.localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        
        leftHandTarget.localPosition += RandomVector3();
        rightHandTarget.localPosition += RandomVector3();
        headTarget.localPosition += RandomVector3();

    }

    private Vector3 RandomVector3()
    {
        Vector3 returnVector = Vector3.zero;

        returnVector.x = Random.Range(-5f, 5f);  
        returnVector.y = Random.Range(-5f, 5f);    
        returnVector.z = Random.Range(-5f, 5f);

        return returnVector;
    }
    
}
