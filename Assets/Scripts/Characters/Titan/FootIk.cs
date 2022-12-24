using UnityEngine;

namespace Characters.Titan
{
    
    /// <summary>
    /// Source: https://www.youtube.com/watch?v=1Xr3jB8ik1g - But modified to work in all direction - still a bit wonky though, but thats why we hide the feet with water :P
    /// </summary>
    public class FootIk : MonoBehaviour
    {
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Transform body;
        [SerializeField] private FootIk oppositeFoot;
        [SerializeField] private float speed = 5, stepDistance = 30, stepLength = 30, stepHeight = 30;
        [SerializeField] private Vector3 footPosOffset;
        

        private float footSpacing, lerp;
        private Vector3 oldPos, currentPos, newPos, baseLocalRot;
        private bool isFirstStep;

        private void Start()
        {
            footSpacing = transform.localPosition.x;
            baseLocalRot = transform.localRotation.eulerAngles;
            currentPos = newPos = oldPos = transform.position;
            lerp = 1;
        }

        private void Update()
        {
            transform.position = currentPos + footPosOffset;
            Vector3 newLocalRot = baseLocalRot;
            newLocalRot.y += body.localRotation.eulerAngles.y;
            transform.localRotation = Quaternion.Euler(newLocalRot);

            Ray ray = new Ray(body.position + body.right * footSpacing + Vector3.up * 20, Vector3.down);
            Debug.DrawRay(body.position + body.right * footSpacing + Vector3.up * 20, Vector3.down * 20, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer))
            {
                if (isFirstStep || (Vector3.Distance(newPos, hit.point) > stepDistance && !oppositeFoot.IsMoving() && !IsMoving()))
                {
                    lerp = 0;
                    int direction = body.InverseTransformPoint(hit.point).z > body.InverseTransformPoint(newPos).z ? 1 : -1;
                    newPos = hit.point + body.forward * direction * stepLength;
                }
            }

            if (lerp < 1)
            {
                Vector3 tempPos = Vector3.Lerp(oldPos, newPos, lerp);
                tempPos.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

                currentPos = tempPos;
                lerp += Time.deltaTime * speed;
            }
            else
            {
                oldPos = newPos;
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
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        }
    }
}