using UnityEngine;

namespace Characters.Titan
{
    
    /// <summary>
    /// Source: https://www.youtube.com/watch?v=1Xr3jB8ik1g
    /// </summary>
    public class FootIk : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform body;
        [SerializeField] private FootIk oppositeFoot;
        [SerializeField] private float speed = 5, stepDistance = 30, stepLength = 30, stepHeight = 30;
        [SerializeField] private Vector3 footPosOffset, footRotOffset;
        [SerializeField] private Transform rootJoint;
        

        private float footSpacing, lerp;
        private Vector3 oldPos, currentPos, newPos;
        private Vector3 oldNormal, currentNormal, newNormal;
        private bool isFirstStep;

        private void Start()
        {
            footSpacing = transform.localPosition.x;
            currentPos = newPos = oldPos = transform.position;
            currentNormal = newNormal = oldNormal = transform.up;
            lerp = 1;
        }

        private void Update()
        {
            transform.position = currentPos + footPosOffset;
            Vector3 targetRot = transform.rotation.eulerAngles;
            targetRot.y = rootJoint.rotation.y;
            transform.rotation = Quaternion.Euler(targetRot);
           // transform.localRotation = Quaternion.LookRotation(currentNormal) * Quaternion.Euler(footRotOffset);

            Ray ray = new Ray(body.position + body.right * footSpacing + Vector3.up * 20, Vector3.down);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                if (isFirstStep || (Vector3.Distance(newPos, hit.point) > stepDistance && !oppositeFoot.IsMoving() && !IsMoving()))
                {
                    lerp = 0;
                    int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;
                    newPos = hit.point + body.forward * direction * stepLength;
                    newNormal = hit.normal;
                }

            }

            if (lerp < 1)
            {
                Vector3 tempPos = Vector3.Lerp(oldPos, newPos, lerp);
                tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

                currentPos = tempPos;
                currentNormal = Vector3.Lerp(oldNormal, newNormal, lerp);
                lerp += Time.deltaTime * speed;
            }
            else
            {
                oldPos = newPos;
                oldNormal = newNormal;
            }
        }

        public bool IsMoving()
        {
            return lerp < 1;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(newPos, 0.5f);
        }
    }
}