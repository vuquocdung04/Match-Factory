
using EventDispatcher;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private EItemName itemName;
    public EItemName ItemName => itemName;

    private ItemSpot spot;
    public ItemSpot Spot => spot;

    [SerializeField] private Rigidbody rig;
    [SerializeField] private Collider objCollider;
    [SerializeField] private Renderer objRenderer;
    [SerializeField] private Material baseMaterial;

    [SerializeField] private bool isGoal;
    public bool IsGoal => isGoal;

    public void SetGoal(bool isGoal)
    {
        this.isGoal = isGoal;
    }

    public void SetMaterialDefault(Material material)
    {
        objRenderer.material = material;
        baseMaterial = material;
    }

    public void AssignSpot(ItemSpot spot)
    {
        this.spot = spot;
    }

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

    public void DestroyItem()
    {
        this.PostEvent(EventID.ITEM_DESTROYED,this);
        Destroy(this.gameObject);
    }


    [Button("Căn chỉnh vị trí theo anchor dưới", ButtonSizes.Large)]
    private void AlignToBottomAnchor()
    {
        objRenderer.transform.localPosition = Vector3.zero;
        float height = objRenderer.bounds.size.y / 2;
        objRenderer.transform.localPosition += Vector3.up * height;
    }

    // Fan Powerup
    public void ApplyRandomForce(float fanMagnitude)
    {
        rig.AddForce(Random.onUnitSphere * fanMagnitude, ForceMode.VelocityChange);
    }
}