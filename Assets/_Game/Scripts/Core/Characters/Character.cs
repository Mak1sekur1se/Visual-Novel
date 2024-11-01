using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace CHARACTERS
{
    /// <summary>
    /// The base class from which all character types derive from
    /// </summary>

    public abstract class Character
    {
        public const bool ENABLE_ON_START = true;

        protected  CharacterManager characterManager => CharacterManager.Instance;

        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        public CharacterConfigData config;
        public Animator animator;
        public Color color { get; protected set; } = Color.white;

        public DialogueSystem dialogueSystem => DialogueSystem.Instance;

        //Coroutines
        protected Coroutine co_revealing, co_hiding;
        protected Coroutine co_moving;
        protected Coroutine co_changingColor;
        public bool isMoving => co_moving != null;
        /// <summary>
        /// ����ShowCharacter
        /// </summary>
        public bool isRevealing => co_revealing != null;
        public bool isHiding => co_hiding != null;
        public bool isChangingColor => co_changingColor != null;
        public virtual bool isVisible { get; set; }

        public Character(string name, CharacterConfigData config, GameObject prefab)
        {
            this.name = name;
            displayName = name;
            this.config = config;

            if (prefab != null)
            {
                GameObject ob = Object.Instantiate(prefab, characterManager.characterPanel);
                ob.name = characterManager.FormatCharacterPath(characterManager.characterPrefabNameFormat, name);
                ob.SetActive(true);
                root = ob.GetComponent<RectTransform>();
                animator = root.GetComponentInChildren<Animator>();
            }
        }


        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationsOnScreen();
            return dialogueSystem.Say(dialogue);
        }

        //��̬���Ľ�ɫ�������
        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void ResetConfigurationData() => config = CharacterManager.Instance.GetCharacterConfig(name);

        public void UpdateTextCustomizationsOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public virtual Coroutine Show()
        {
            if (isRevealing)
                return co_revealing;

            if (isHiding)
                characterManager.StopCoroutine(co_hiding);

            co_revealing = characterManager.StartCoroutine(ShowingOrHiding(true));

            return co_revealing;


        }

        public virtual Coroutine Hide()
        {
            if(isHiding)
                return co_hiding;

            if (isRevealing)
                characterManager.StopCoroutine(co_revealing);

            co_hiding = characterManager.StartCoroutine(ShowingOrHiding(false));

            return co_hiding;
        }

        //����ê��������λ�ã�
        public virtual void SetPosition(Vector2 position)
        {
            if (root == null)
                return;

            (Vector2 minAnchorTarget, Vector2 MaxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);

            root.anchorMin = minAnchorTarget;
            root.anchorMax = MaxAnchorTarget;
        }

        public virtual Coroutine MoveToPosition(Vector2 position, float speed = 2f, bool smooth = false)
        {
            if (root == null)
                return null;

            if (isMoving)
                characterManager.StopCoroutine(co_moving);
            co_moving = characterManager.StartCoroutine(MovingToPosition(position, speed, smooth));

            return co_moving;
        }

        private IEnumerator MovingToPosition(Vector2 position, float speed, bool smooth)
        {

            (Vector2 minAnchorTarget, Vector2 maxAnchorTarget) = ConvertUITargetPositionToRelativeCharacterAnchorTargets(position);
            Vector2 padding = root.anchorMax - root.anchorMin;

            Vector2 startAnchorMin = root.anchorMin;
            float i = 0.01f;
            while (root.anchorMin != minAnchorTarget || root.anchorMax != maxAnchorTarget)
            {
                if (speed * i >= 1)
                    i = 1/speed;
                root.anchorMin = smooth ?
                    Vector2.Lerp(startAnchorMin, minAnchorTarget, speed *  i)
                    : Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed * Time.deltaTime * 0.35f);// not linear
                root.anchorMax = root.anchorMin + padding;
                i += 0.01f;

                yield return null;
            }
            co_moving = null;
        }

        protected (Vector2, Vector2) ConvertUITargetPositionToRelativeCharacterAnchorTargets(Vector2 position) 
        {
            Vector2 padding = root.anchorMax - root.anchorMin;

            float maxX = 1f - padding.x;
            float maxY = 1f - padding.y;

            Vector2 minAnchorTarget = new Vector2(maxX * position.x, maxY * position.y);
            Vector2 maxAnchorTarget = minAnchorTarget + padding;

            return (minAnchorTarget, maxAnchorTarget);
        }

        public virtual void SetColor(Color color)
        {
            this.color = color;
        }

        public Coroutine TransitionColor(Color color, float speed = 1f)
        {
            this.color = color;

            if (isChangingColor)
                characterManager.StopCoroutine(co_changingColor);

            co_changingColor = characterManager.StartCoroutine(ChangingColor(color, speed));

            return co_changingColor;
        }

        public virtual IEnumerator ChangingColor(Color color, float speed)
        {
            Debug.Log("Color changing is not applicable on this character type!");
            yield return null;
        }

        public virtual IEnumerator ShowingOrHiding(bool show)
        {
            Debug.Log("Show/Hide cannot be called from a base character type.");
            yield return null;
        }

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }
    }
}
