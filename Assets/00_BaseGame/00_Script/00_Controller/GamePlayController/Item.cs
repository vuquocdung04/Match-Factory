using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Rigidbody rig;
    [SerializeField] private Collider objCollider;
    [SerializeField] private Renderer objRenderer;
    [SerializeField] private Material baseMaterial;

    public void DisableShadow()
    {
        objRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    public void DisablePhysic()
    {
        rig.isKinematic = true;
        objCollider.enabled = false;
    }

    public void Select(Material outLineMaterial)
    {
        objRenderer.materials = new Material[] { baseMaterial, outLineMaterial };
    }

    public void Deselect()
    {
        objRenderer.materials = new Material[] { baseMaterial };
    }
}