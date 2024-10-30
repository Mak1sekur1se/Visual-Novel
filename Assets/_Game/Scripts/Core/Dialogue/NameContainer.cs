using TMPro;
using UnityEngine;

namespace DIALOGUE
{
    [System.Serializable]
    /// <summary>
    /// 控制屏幕上的名字文本，DialogueContainer的一部分
    /// </summary>
    public class NameContainer
    {
        [SerializeField] private GameObject root;
        [SerializeField] private TextMeshProUGUI nameText;

        public void Show(string nameToShow = "")
        {
            root.SetActive(true);

            if (nameToShow != string.Empty)
                nameText.text = nameToShow;

        }

        public void Hide()
        {
            root.SetActive(false);
        }

        public void SetNameColor(Color color)  => nameText.color = color;
        public void SetNameFont(TMP_FontAsset font) => nameText.font = font;
    }
}
