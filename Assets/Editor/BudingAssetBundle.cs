using UnityEngine;
using System.Collections;
using UnityEditor;//添加命名空间

public class TestAssetBundle {

	[MenuItem("Assetbundle/Mybundle")]//在菜单栏添加一个选项
	static void MybundleFunction()
	{
		//选取要打包的资源，参数一：选取的类型，参数二：选取的模式（DeepAssets)）
		Object[] selectObjs = Selection.GetFiltered (typeof(Object),
		                                 SelectionMode.DeepAssets);
		//弹出保存面板(窗口左上角名字，路径（不指定，为空），名字，扩展名)
		string path = EditorUtility.SaveFilePanel ("Saveresource",
		                          "","mybundle","assetbundle");
		//打包函数,参数一：(null(通常设为null),
		//参数二：Object[]类型，打包的对象  参数三：路径 
		//参数四：打包的选择   参数五：打包平台的选择
		BuildPipeline.BuildAssetBundle (null,selectObjs,path,
		                  BuildAssetBundleOptions.CollectDependencies|
		                  BuildAssetBundleOptions.CompleteAssets,
		                           BuildTarget.StandaloneWindows);
		AssetDatabase.Refresh ();//刷新资源
	}

    [MenuItem("Assetbundle/Webbundle")]//在菜单栏添加一个选项
    static void MybundleFunction2()
    {
        //选取要打包的资源，参数一：选取的类型，参数二：选取的模式（DeepAssets)）
        Object[] selectObjs = Selection.GetFiltered(typeof(Object),
                                         SelectionMode.DeepAssets);
        //弹出保存面板(窗口左上角名字，路径（不指定，为空），名字，扩展名)
        string path = EditorUtility.SaveFilePanel("Saveresource",
                                  "", "mybundle", "assetbundle");
        //打包函数,参数一：(null(通常设为null),
        //参数二：Object[]类型，打包的对象  参数三：路径 
        //参数四：打包的选择   参数五：打包平台的选择
        BuildPipeline.BuildAssetBundle(null, selectObjs, path,
                          BuildAssetBundleOptions.CollectDependencies |
                          BuildAssetBundleOptions.CompleteAssets,
                                   BuildTarget.WebPlayer);
        AssetDatabase.Refresh();//刷新资源
    }
}
