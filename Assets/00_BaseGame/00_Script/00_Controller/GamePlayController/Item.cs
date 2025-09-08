
using DG.Tweening;
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
    
    private bool isBusy;
    public bool IsBusy => isBusy;

    private bool isBusySpring;
    
    public void SetBusy(bool isBusyParam)
    {
        this.isBusy = isBusyParam;
    }

    public void SetGoal(bool isGoalParam)
    {
        this.isGoal = isGoalParam;
    }

    public void SetMaterialDefault(Material material)
    {
        objRenderer.material = material;
        baseMaterial = material;
    }

    public void ApplyTextureProperty(Material sharedMaterial, Texture2D texture)
    {
        // Gán material chia sẻ làm baseMaterial
        baseMaterial = sharedMaterial;
    
        // Set material mặc định
        objRenderer.material = sharedMaterial;
    
        // Áp dụng texture riêng qua PropertyBlock (cho rendering)
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetTexture("_MainTex", texture);
        objRenderer.SetPropertyBlock(propertyBlock);
    }

    public void AssignSpot(ItemSpot spotParam)
    {
        this.spot = spotParam;
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

    public void ResetAll()
    {
        transform.SetParent(null);
        isBusySpring = true;
        transform.DOScale(Vector3.one, 1f).OnComplete(delegate
        {
            objCollider.enabled = true;
            isBusySpring = false;
        });
        // Kích hoạt physics ngay lập tức để AddForce hoạt động
        rig.isKinematic = false;
        rig.AddTorque(Random.onUnitSphere * 15f, ForceMode.Impulse);
    }
    
    public void Select(Material outLineMaterial)
    {
        objRenderer.materials = new[] { baseMaterial, outLineMaterial };
    }

    public void Deselect()
    {
        objRenderer.materials = new[] { baseMaterial };
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
        if(isBusySpring) return;
        rig.AddForce(Random.onUnitSphere * fanMagnitude, ForceMode.VelocityChange);
    }
}