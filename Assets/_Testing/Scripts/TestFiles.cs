using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TESTING
{
    public class TestFiles : MonoBehaviour
    {
        //private string fileName = "textFile.txt";
        //private string fileAssetName = "textFile";
        [SerializeField] private TextAsset fileAssetName;

        private void Start()
        {
            StartCoroutine(Run());
        }

        private IEnumerator Run()
        {
            //List<string> lines = FileManagers.ReadTextFile(fileName, false);
            List<string> lines = FileManagers.ReadTextAsset(fileAssetName, false);
            foreach (string line in lines)
                Debug.Log(line);
            yield return null;
        }
    }
}
