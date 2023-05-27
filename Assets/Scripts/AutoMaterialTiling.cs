using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
public class AutoMaterialTiling : MonoBehaviour
{
    private Renderer rend;
    // Start is called before the first frame update
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rend != null)
        {
            Vector3 scale = transform.lossyScale;
            rend.sharedMaterial.mainTextureScale = new Vector2(scale.x, scale.z);
        }
    }
}
