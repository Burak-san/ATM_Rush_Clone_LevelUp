using UnityEngine;

namespace Controllers
{
    public class WalkingPlatformMeshController : MonoBehaviour
    {
        #region Self Variables

        #region Serialize Variables
        [SerializeField] private float scrollSpeed = 0.05f;
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

        #endregion

        #region Private Variables

        private new Renderer renderer = new Renderer();

        #endregion

        #endregion

        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            float offset = Time.time * scrollSpeed;
            renderer.material.SetTextureOffset(BaseMap, new Vector2(0, -offset));
        }
    }
}