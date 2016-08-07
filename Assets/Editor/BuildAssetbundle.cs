using UnityEngine;
using System.Collections;
using UnityEditor;
public class BuildAssetbundle : MonoBehaviour {

    //[MenuItem("BuildAssetbundle/BuildAll")]
    static void Build()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/../Assetbundle", BuildAssetBundleOptions.UncompressedAssetBundle);
    }

}
