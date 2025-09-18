using UnityEngine;

namespace AztechGames
{
    public class GliderEngine_Controller : MonoBehaviour
    {
        [Tooltip("Acceleration rate of the glider engine.")]
        public float acceleration = 10f;

        private float thrust = 0;

        /// <summary>
        /// Gets or sets the thrust value, clamped between 0 and 200.
        /// </summary>
        public float Thrust
        {
            get => Mathf.Clamp(thrust, 0f, 30f);
            set => thrust = value;
        }

        /// <summary>
        /// Handles engine inputs, adjusting thrust based on user input and slat amount.
        /// </summary>
        void EngineInputs()
        {
            if (Input.GetButton("Fire1"))
            {
                thrust += Time.deltaTime * acceleration;
            }
            else if (Input.GetButton("Fire2"))
            {
                thrust -= Time.deltaTime * acceleration;
            }
            thrust -= GliderSurface_Controller.Instance.SlatAmount * Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (GliderSurface_Controller.Instance != null)
            {
                GliderSurface_Controller.Instance.GetInputs();
                GliderSurface_Controller.Instance.PlaneRotations();
                EngineInputs();
            }
        }
    }
}