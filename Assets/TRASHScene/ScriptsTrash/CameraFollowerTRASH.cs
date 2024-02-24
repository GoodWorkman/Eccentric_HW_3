using UnityEngine;

namespace Trash
{
    public class CameraFollowerTRASH : MonoBehaviour
    {
        [SerializeField] private Transform _target;

        [SerializeField] private float _lerpRate = 10f;

        private void LateUpdate()
        {
            transform.position =
                Vector3.Lerp(transform.position, _target.transform.position, Time.deltaTime * _lerpRate);
        }
    }
}


