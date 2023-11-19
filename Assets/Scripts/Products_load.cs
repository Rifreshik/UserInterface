using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Firebase.Database;
using Firebase.Storage;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;

public class Products_load : MonoBehaviour
{
    public GameObject objectTemplate; // Template object for instantiation
    public Transform parentObject; // Parent object for positioning

    private DatabaseReference dbRef;
    private FirebaseStorage storage;
    private StorageReference storageReference;
    private float objectOffset = 0f; // Offset value for positioning objects vertically

    private Dictionary<string, Texture2D> textureCache = new Dictionary<string, Texture2D>(); // Cache for downloaded textures

    private void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://museumr-72dd0.appspot.com");

        StartCoroutine(LoadData());
    }

    private IEnumerator LoadData()
    {
        var productTask = dbRef.Child("products").GetValueAsync();
        yield return new WaitUntil(() => productTask.IsCompleted);

        if (productTask.Exception != null)
        {
            Debug.Log("Error loading products: " + productTask.Exception.Message);
            yield break;
        }

        var productSnapshot = productTask.Result;

        if (productSnapshot == null || productSnapshot.ChildrenCount == 0)
        {
            Debug.Log("No products available.");
            yield break;
        }

        foreach (var childSnapshot in productSnapshot.Children)
        {
            Product product = GetProductFromSnapshot(childSnapshot);

            yield return CreateObjectWithTexture(product);

            objectOffset -= 300f; // Decrease the offset value for the next object
        }
    }

    private Product GetProductFromSnapshot(DataSnapshot snapshot)
    {
        string name = snapshot.Child("name").Value?.ToString() ?? "";
        int count = int.TryParse(snapshot.Child("kol").Value?.ToString(), out int result) ? result : 0;
        string description = snapshot.Child("description").Value?.ToString() ?? "";
        int cost = int.TryParse(snapshot.Child("price").Value?.ToString(), out int result2) ? result2 : 0;
        string url = snapshot.Child("url").Value?.ToString() ?? "";

        return new Product(name, count, description, cost, url);
    }

    private async Task<string> CreateObjectWithTexture(Product product)    
    {    
        GameObject newObj = Instantiate(objectTemplate, parentObject);

        // Set the position offset
        Vector3 offset = new Vector3(0f, objectOffset, 0f);
        newObj.transform.position += offset;

        // Get the Text components of the new object
        Text[] texts = newObj.GetComponentsInChildren<Text>();
        Text nameText = texts[0];
        Text countText = texts[1];
        Text descriptionText = texts[2];
        Text costText = texts[3];

        // Set the Text components accordingly
        nameText.text = product.Name;
        countText.text = product.Count.ToString();
        descriptionText.text = product.Description;
        costText.text = product.Cost.ToString();

        // Get the Image component of the new object
        Image image = newObj.GetComponentInChildren<Image>();

        // Check if the texture is already in the cache
        if (textureCache.TryGetValue(product.Url_photo, out Texture2D texture))
        {
            // Use the cached texture
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            image.sprite = sprite;
        }
        else
        {
            // Load the texture asynchronously
            texture = await LoadTexture(product.Url_photo);
            if (texture != null)
            {
                // Add the downloaded texture to the cache
                textureCache.Add(product.Url_photo, texture);

                // Create the sprite from the downloaded texture
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);

                // Set the sprite of the image component
                image.sprite = sprite;
            }
        }

        // Activate the new object and deactivate the template object
        newObj.SetActive(true);
        objectTemplate.SetActive(false);

        return product.Url_photo;
    }

    private async Task<Texture2D> LoadTexture(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(url))
        {
            // Load the texture asynchronously
            var asyncOp = webRequest.SendWebRequest();
            while (!asyncOp.isDone)
            {
                await Task.Yield();
            }

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(webRequest.error);
                return null;
            }
            else
            {
                // Load the image data asynchronously
                var texture = new Texture2D(2, 2);
                texture.wrapMode = TextureWrapMode.Clamp; // Set wrap mode to clamp to avoid texture artifacts
                texture.filterMode = FilterMode.Bilinear; // Set filter mode to bilinear for better quality
                texture.anisoLevel = 2; // Set anisotropic filtering level to 2 for better quality
                ImageConversion.LoadImage(texture, webRequest.downloadHandler.data, false);
                await Task.Yield(); // Wait until the texture is readable

                // Return the loaded texture
                return texture;
            }
        }
    }

    private class Product
    {
        public string Name { get; }
        public int Count { get; }
        public string Description { get; }
        public int Cost { get; }
        public string Url_photo { get; }

        public Product(string name, int count, string description, int cost, string url_photo)
        {
            Name = name;
            Count = count;
            Description = description;
            Cost = cost;
            Url_photo = url_photo;
        }
    }
}