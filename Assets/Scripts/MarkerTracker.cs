using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageTracker : MonoBehaviour
{
    private ARTrackedImageManager trackedImages;

    public GameObject[] ArPrefabs;
    public TextMeshProUGUI debugText;  // Assign this in the inspector

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    private Dictionary<TrackableId, string> trackedImageNames = new();

    private Dictionary<string, float> lastSeenTimes = new Dictionary<string, float>();
    private float hideDelay = 5.0f; // seconds to wait before hiding object

    void Awake()
    {
        trackedImages = GetComponent<ARTrackedImageManager>();
        if (trackedImages == null)
        {
            Debug.LogError("ARTrackedImageManager not found on GameObject!");
        }

        if (trackedImages.referenceLibrary == null)
        {
            Debug.LogError("ReferenceImageLibrary is missing at runtime!");
        }
        else
        {
            Debug.Log($"Library loaded with {trackedImages.referenceLibrary.count} images");
            for (int i = 0; i < trackedImages.referenceLibrary.count; i++)
            {
                var image = trackedImages.referenceLibrary[i];
                Debug.Log($"Reference image: {image.name}, GUID: {image.guid}");
            }

        }

        // Preload objects into dictionary (disabled initially)
        foreach (var prefab in ArPrefabs)
        {
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            obj.name = prefab.name;  // Ensure matching name
            obj.SetActive(false);
            arObjects.Add(prefab.name, obj);
        }
    }

    private void Update()
    {
        foreach (var kvp in arObjects)
        {
            string name = kvp.Key;
            GameObject obj = kvp.Value;

            if (lastSeenTimes.TryGetValue(name, out float lastSeen))
            {
                bool shouldHide = (Time.time - lastSeen > hideDelay);
                if (shouldHide && obj.activeSelf)
                {
                    obj.SetActive(false);
                    PrintDebug($"Hiding {name} after timeout.");
                }
            }
        }
    }

    void OnEnable()
    {
        if (trackedImages != null)
            trackedImages.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    void OnDisable()
    {
        if (trackedImages != null)
            trackedImages.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // PrintDebug($"Found: {trackedImage.referenceImage.name}");
            string name = trackedImage.referenceImage.name;
            trackedImageNames[trackedImage.trackableId] = name;
            UpdateImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            if (!trackedImageNames.TryGetValue(trackedImage.trackableId, out var name))
            {
                Debug.LogWarning("Missing tracked image name on update.");
                continue;
            }
            UpdateImage(trackedImage);
        }

        //foreach (var trackedImage in eventArgs.removed)
        //{
        //    if (trackedImageNames.TryGetValue(trackedImage.trackableId, out var name))
        //    {
        //        if (arObjects.TryGetValue(name, out var obj))
        //        {
        //            obj.SetActive(false);
        //            PrintDebug($"Removed: {name}");
        //        }
        //        trackedImageNames.Remove(trackedImage.trackableId);
        //    }
        //    else
        //    {
        //        PrintDebug("Tracked image name not found during removal.");
        //    }
        //}
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        Debug.Log($"Library loaded with {trackedImages.referenceLibrary.count} images");
        for (int i = 0; i < trackedImages.referenceLibrary.count; i++)
        {
            var image = trackedImages.referenceLibrary[i];
            //Debug.Log($"Reference image: {image.name}, GUID: {image.guid}");
        }

        if (trackedImage.referenceImage == null)
        {
            PrintDebug("Tracked image has no reference image.");
            return;
        }

        string name = trackedImage.referenceImage.name;

        if (string.IsNullOrEmpty(name))
        {
            PrintDebug("Reference image name is null or empty.");
            return;
        }

        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            lastSeenTimes[name] = Time.time;
            if (arObjects.TryGetValue(name, out GameObject obj))
            {
                //obj.SetActive(true);
                obj.SetActive(trackedImage.trackingState == TrackingState.Tracking);
                // ADDED FOR FADING
                //var fader = obj.GetComponent<ARObjectFader>();
                //if (fader != null)
                //{
                //    fader.SetVisible(trackedImage.trackingState == TrackingState.Tracking);
                //}
                

                obj.transform.position = trackedImage.transform.position;
                obj.transform.rotation = trackedImage.transform.rotation;
                //PrintDebug($"Tracking {name}: {trackedImage.trackingState}");
            }
            else
            {
                PrintDebug($"No prefab found for {name}");
            }
        }
    }

    private void PrintDebug(string message)
    {
        if (debugText != null)
        {
            //debugText.text = message;
        }
        Debug.Log(message);
    }
}
