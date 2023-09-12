using UnityEngine;

namespace Utility
{
    public class EarthRotator : MonoBehaviour
    {
        public float rotationSpeed = 2.0f;

        private void Update()
        {
            // Rotate the GameObject around its forward axis (Z-axis in Unity) over time
            transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
        }
    }
}