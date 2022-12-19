using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(VIDE_Assign))]
public class SceneSwitcher : MonoBehaviour, ISerializationCallbackReceiver
{
    private VIDE_Assign _videAssign;
    public VIDE_Assign videAssign => _videAssign ? _videAssign : (_videAssign = GetComponent<VIDE_Assign>());

#if UNITY_EDITOR
    public UnityEditor.SceneAsset sceneAsset;
#endif

    [SerializeField][HideInInspector] private string sceneName;

    private bool deserialized = false;
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (deserialized)
            {
                sceneAsset = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.SceneAsset>(sceneName);
            }
        };
#endif
    }

    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
#if UNITY_EDITOR
        deserialized = false;
        UnityEditor.EditorApplication.delayCall += () =>
        {
            sceneName = UnityEditor.AssetDatabase.GetAssetPath(sceneAsset);
            deserialized = true;
        };
#endif
    }

    private void Update()
    {
        if (videAssign.interactionCount > 0)
        {
            SceneManager.LoadScene(sceneName);
        }
    }

}
