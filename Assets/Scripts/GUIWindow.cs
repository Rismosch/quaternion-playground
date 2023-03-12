using UnityEngine;
using UnityEditor;
using System.Linq;

public class GUIWindow : EditorWindow {
    [MenuItem("Visualizing-Quaternions/GUIWindow")]
    private static void ShowWindow() {
        var window = GetWindow<GUIWindow>();
        window.titleContent = new GUIContent("GUIWindow");
        window.Show();
    }

    private enum Example{
        OneSphere,
        TwoSphere,
        ThreeSphere,
    }

    private Example SelectedExample;
    private GUIWindowOneSphere GUIWindow1Sphere = new();
    private GUIWindowTwoSphere GUIWindow2Sphere = new();
    private GUIWindowThreeSphere GUIWindow3Sphere = new();

    private OneSphere OneSphere;
    private TwoSphere TwoSphere;
    private ThreeSphere ThreeSphere;
    private string[] SphereNames = {"OneSphere", "TwoSphere", "ThreeSphere"};


    private void OnGUI() {
        SelectedExample = (Example)EditorGUILayout.EnumPopup(SelectedExample);

        switch (SelectedExample)
        {
            case Example.OneSphere:
                GetSphereGameObject(ref OneSphere, x => {
                    x.gameObject.SetActive(true);
                    GUIWindow1Sphere.Draw(x);
                });
                GetSphereGameObject(ref TwoSphere, x => x.gameObject.SetActive(false));
                GetSphereGameObject(ref ThreeSphere, x => x.gameObject.SetActive(false));
                break;
            case Example.TwoSphere:
                GetSphereGameObject(ref OneSphere, x => x.gameObject.SetActive(false));
                GetSphereGameObject(ref TwoSphere, x =>{
                    x.gameObject.SetActive(true);
                    GUIWindow2Sphere.Draw();
                });
                GetSphereGameObject(ref ThreeSphere, x => x.gameObject.SetActive(false));
                break;
            case Example.ThreeSphere:
                GetSphereGameObject(ref OneSphere, x => x.gameObject.SetActive(false));
                GetSphereGameObject(ref TwoSphere, x => x.gameObject.SetActive(false));
                GetSphereGameObject(ref ThreeSphere, x => {
                    x.gameObject.SetActive(true);
                    GUIWindow3Sphere.Draw();
                });
                break;
        }
    }

    private void GetSphereGameObject<T>(ref T sphere, System.Action<T> callback) where T: MonoBehaviour
    {
        if (sphere == null || sphere.gameObject == null)
        {
            var rootGameObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            var gameObject = rootGameObjects.First(x => x.name == typeof(T).Name);
            sphere = gameObject.GetComponent<T>();
        }

        callback(sphere);
    }
}