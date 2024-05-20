using Firebase.Auth;
using Firebase.Storage;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.Database;
using System.Net.Http;
using UnityEngine.Networking;


public class SelectImage : MonoBehaviour
{
    private string pdfFileType;
    private string finalPath;
    [SerializeField] private RawImage rawImage;
    FirebaseStorage storage;
    StorageReference storageReference;
    private DatabaseReference reference;
    Texture2D texture1;

    void Start()
    {
        storage = FirebaseStorage.DefaultInstance;
        storageReference = storage.GetReferenceFromUrl("gs://mybuisness-7445d.appspot.com");
        StorageReference image = storageReference.Child("1.png");
    }

    public void LoadImage()
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                finalPath = path;
                Debug.Log("Picked file: " + path);
                texture1 = NativeGallery.LoadImageAtPath( path, 512 );
                StartCoroutine(SetImage());
            }
        });

        Debug.Log("Permission result: " + permission);
    }

    private IEnumerator SetImage()
    {
        byte[] bytes = null;
        UnityWebRequest www = UnityWebRequest.Get("file:///" + finalPath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            bytes = www.downloadHandler.data;
        }
        rawImage.texture = texture1;
        string mail = "";
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        string uid = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        var user = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(uid).GetValueAsync();
        yield return new WaitUntil(predicate: () => user.IsCompleted);
        if (user.Exception != null)
        {
            Debug.Log(user.Exception);
        }
        else if (user.Result == null)
        {
            Debug.Log("User is empty");
        }
        else
        {
            DataSnapshot dataSnapshot = user.Result;
            mail = dataSnapshot.Child("email").Value.ToString();
            if (dataSnapshot.Child("HaveAvatars").Value.ToString() == "1")
            {
                storage = FirebaseStorage.DefaultInstance;
                storageReference = storage.GetReferenceFromUrl("gs://mybuisness-7445d.appspot.com");
                storageReference.Child("avatasr/avatar:" + mail).DeleteAsync();
            }
            else
            {
                reference.Child("Users").Child(uid).Child("HaveAvatars").SetValueAsync("1");
                if (dataSnapshot.Child("rank").Value.ToString() != "0")
                {
                    string bossuid = dataSnapshot.Child("boss").Value.ToString();
                    reference.Child("Users").Child(bossuid).Child("MyWorker").Child(mail).Child("HaveAvatars").SetValueAsync("1");
                }
            }    
        }
        StorageReference saveRef = storageReference.Child("avatasr/avatar:" + mail);

        saveRef.PutBytesAsync(bytes).ContinueWithOnMainThread((task) => {
            if(task.IsFaulted || task.IsCanceled)
            {
                Debug.Log(task.Exception);
            }
            else
            {
                Debug.Log("Is uploaded");
            }
        });
    }
}
