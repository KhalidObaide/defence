using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;

public class BuildController : MonoBehaviour
{
    public List<Tower> towers;
    #if UNITY_EDITOR
        [Tag]
    #endif
    public List<string> buildableSurfaces;

    private Tower currentTower;
    private GameObject previewObject;
    private int previewLayer;

    private PhotonView view;

    void Start()
    {
        view = GetComponent<PhotonView>();
        if ( !view.IsMine )
        {
            return;
        }
        previewLayer = LayerMask.NameToLayer("Preview");
        ChangeTower(0);
    }

    void Update()
    {
        if ( !view.IsMine )
        {
            return;
        }

        for (int i = 0; i < towers.Count; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ChangeTower(i);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            previewObject.transform.Rotate(0, 90, 0);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << previewLayer)))
        {
            if (buildableSurfaces.Contains(hit.transform.tag))
            {
                MoveToTopSurface(previewObject, hit);
                previewObject.SetActive(true);
                if (IsCollidingWithObject())
                {
                    previewObject.SetActive(false);
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        SpawnPrefab();
                    }
                }
            }
            else
            {
                previewObject.SetActive(false);
            }
        }
        else
        {
            previewObject.SetActive(false);
        }
    }

    void ChangeTower(int index)
    {
        if (previewObject != null) Destroy(previewObject);
        currentTower = towers[index];
        SetupPrefabPreview();
    }

    void SetupPrefabPreview()
    {
        previewObject = Instantiate(currentTower.towerPrefab);
        SetMaterialTransparency(previewObject, 0.5f);
        SetLayerRecursively(previewObject, previewLayer);
    }

    void SetMaterialTransparency(GameObject gameObject, float alpha)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material material = renderer.material;
            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }
    }

    void SetLayerRecursively(GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform child in gameObject.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    void MoveToTopSurface(GameObject objectToMove, RaycastHit hit)
    {
        Transform groundPoint = objectToMove.transform.Find("GroundPoint");
        if (groundPoint == null) return;
        float surfaceY = hit.point.y + hit.collider.bounds.extents.y;
        float yOffset = objectToMove.transform.position.y - groundPoint.position.y;
        Vector3 position = hit.point;
        position.y = surfaceY + yOffset;
        objectToMove.transform.position = position;
    }

    void SpawnPrefab()
    {
        PhotonNetwork.Instantiate(currentTower.towerPrefab.name, previewObject.transform.position, previewObject.transform.rotation);
    }

    bool IsCollidingWithObject()
    {
        Collider[] intersecting = Physics.OverlapBox(previewObject.transform.position, previewObject.GetComponent<Collider>().bounds.extents, previewObject.transform.rotation, ~(1 << previewLayer));
        if (intersecting.Length > 0)
        {
            foreach (Collider collider in intersecting)
            {
                if (collider.gameObject != previewObject)
                {
                    if (!buildableSurfaces.Contains(collider.gameObject.tag))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
