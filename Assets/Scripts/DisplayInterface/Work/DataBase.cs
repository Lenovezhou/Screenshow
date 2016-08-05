using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DataBase
{

    public class fileData
    {
        public string file;
        public string fileName;
    }

    public struct DataInt2
    {
        public int x;
        public int y;
        public DataInt2(int a, int b)
        {
            x = a;
            y = b;
        }

    }

    public struct DataInt3
    {
        public float x;
        public float y;
        public float z;
        public DataInt3(float a, float b, float c)
        {
            x = a;
            y = b;
            z = c;
        }
    }

    public struct Furniture
    {
        public Vector2 PicturePos;
        public Vector2 ScaleData;
    }

    public class GoodList
    {

    }

    public class Curtain : GoodList
    {
        public int MatID;
        public int Id;
        public int ComponentType;

        public Material Material;

        public bool IsModel;

        public string ModelUrl;
        public string TextureUrl;
        public string IconUrl;
        public DataInt2 UVrepeatParameters;
        public DataInt3 UVParameters;
        public float ScaleParameters;

        public string GoodUrl;

        public string Description;

        public string AudioUrl;

        public string MovieUrl;

        public Curtain() { }

        public Curtain(int matID, int id, bool isModel, string modelUrl, string textureUrl, Material material) 
        {
            MatID = matID;
            Id = id;
            IsModel = isModel;
            ModelUrl = modelUrl;
            TextureUrl = textureUrl;
            Material = material;
        }
    }

    public class SingleCurtain : GoodList
    {
        public string TextureUrl;
        public string Stylor_code;
        public string Curtain_kind;
        public string Name;
    }

    //public class Window : GoodList
    //{
    //    public DataInt3 WindowPosition;
    //    public DataInt3 WindowRotation;
    //    public DataInt3 ScaleParameters;
    //    public DataInt2 SizeParameters;

    //    public string WindowPictureUrl;
    //}
    public class Window : GoodList
    {
        public int Id;
        public DataInt3 WindowPosition;
        public DataInt3 WindowRotation;
        public DataInt3 ScaleParameters;
        public DataInt2 SizeParameters;

        public List<Curtain> curtain=new List<Curtain> ();

        public string WindowPictureUrl;
    }

    public class Scene : GoodList
    {
        public string CeilingPicture;

        public string WallPicture;

        public string FloorPicture;

        public Furniture Picture;
    }

    public class SceneLayout
    {
        public ushort layout_id;
        public ushort scene_id;
        public ushort sequ;
        public string layout_kind;
        public string model_kind;
        public string layout_code;
        public string layout_name;
        

    }

    public class AssetInfo
    {
        public string Icon;
        public string ProdId;
        public bool isModle;
        public string DefaultTexture;
        public int ID;
        public string Name;
        public List<string> ModelPath;
        public List<Material> material;
        public List<string> Texture;
        public AssetInfo()
        {
            ModelPath = new List<string>();
            material = new List<Material>();
            Texture = new List<string>();
        }
    }

    public class StyleInfo
    {
        public int Id;
        public int HouseID;
        public int SceneID;
        public string URL;
        public string IconUrl;
        public string description;
    }

}
