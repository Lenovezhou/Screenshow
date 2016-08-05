using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Data
{
    public class FurnitureData
    {
        public string SelfName;
        public string FilePath;
        public List<string> PictureList;
        public FurnitureData()
        {
            
        }

        public FurnitureData(string Name, string filepath, List<string> pictureList)
        {
            SelfName = Name;
            FilePath = filepath;
            PictureList = pictureList;
        }

    }
}
