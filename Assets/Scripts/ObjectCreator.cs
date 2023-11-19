using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    public GameObject objectTemplate;
    public Transform parentTransform;

    private void Start()
    {
        InstantiateFirstObject();
        for (int i = 1; i < 5; i++)
        {
            CreateObject(i);
        }
    }

    private void InstantiateFirstObject()
    {
        GameObject firstObject = Instantiate(objectTemplate);
        SetObjectTransform(firstObject, -600);
    }

    private void CreateObject(int index)
    {
        GameObject newObject = Instantiate(objectTemplate, parentTransform);
        SetObjectTransform(newObject, index);
    }

    private void SetObjectTransform(GameObject obj, int index)
    {
        obj.transform.SetParent(parentTransform);
        obj.transform.localPosition = GetSpawnPosition(index);
        obj.transform.localScale = objectTemplate.transform.localScale;
    }

    private Vector3 GetSpawnPosition(int index)
    {
        return new Vector3(0f, -600f * (index + 1), 0f);
    }
}