using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace GFrame
{
    public class DynamicImage : Image
    {
        public DynamicAtlasGroup atlasGroup = DynamicAtlasGroup.Size_1024;

        private DynamicAtlasGroup m_Group;
        /// <summary>
        /// 动态图集
        /// </summary>
        private DynamicAtlas m_Atlas;
        /// <summary>
        /// 默认Sprite
        /// </summary>
        private Sprite m_DefaultSprite;
        /// <summary>
        /// 图片名字
        /// </summary>
        private string m_SpriteName;

        protected override void Awake()
        {
            base.Awake();
#if UNITY_EDITOR
            //在编辑器下 退出playmode会再走一次start
            if (Application.isPlaying)
            {
                OnPreDoImage();
            }
#else
       OnPreDoImage();
#endif
        }

        private void OnPreDoImage()
        {
            if (sprite != null)//事先挂载了一张图片
            {
                //可以先放入到图集中去，在使用这一张图集里面的图片
                SetGroup(atlasGroup);
                SetImage();
            }
        }

        private void SetGroup(DynamicAtlasGroup group)
        {
            if (m_Atlas != null)
            {
                return;
            }

            m_Group = group;
            m_Atlas = DynamicAtlasMgr.Instance.GetDynamicAtlas(group);
        }

        private void SetImage()
        {
            m_DefaultSprite = sprite;
            m_SpriteName = mainTexture.name;
            m_Atlas.SetTexture(mainTexture, OnGetImageCallBack);
            #region 输出动态图集更直观
            Debug.LogError("设置图片：" + m_Atlas.PageList.Count);
            for (int i = 0; i < m_Atlas.PageList.Count; i++)
            {
                Texture2D texture2D = m_Atlas.PageList[i].texture;
                byte[] texture2DData = texture2D.EncodeToPNG();
                string path = Application.dataPath + "/动态图集.png";
                File.WriteAllBytes(path, texture2DData);
                Debug.LogError("写入完成");
            }
            #endregion
        }

        private void OnGetImageCallBack(Texture tex, Rect rect)
        {
            int length = (int)m_Group;
            Rect spriteRect = rect;

            if (m_DefaultSprite != null)
                sprite = Sprite.Create((Texture2D)tex, spriteRect, m_DefaultSprite.pivot, m_DefaultSprite.pixelsPerUnit, 1, SpriteMeshType.Tight, m_DefaultSprite.border);
            else
            {
                sprite = Sprite.Create((Texture2D)tex, spriteRect, new Vector2(spriteRect.width * .5f, spriteRect.height * .5f), 100, 1, SpriteMeshType.Tight, new Vector4(0, 0, 0, 0));
                m_DefaultSprite = sprite;
            }
        }



        #region Public Func
        public void SetImage(string name)
        {
            if (m_Atlas == null)
            {
                SetGroup(atlasGroup);
            }
            if (!string.IsNullOrEmpty(m_SpriteName) && m_SpriteName.Equals(name))
            {
                return;
            }

            m_SpriteName = name;
            m_Atlas.GetTeture(name, OnGetImageCallBack);
        }

        public void RemoveImage(bool clearRange = false)
        {
            if (m_Atlas == null)//并没有使用图集
                return;

            if (!string.IsNullOrEmpty(m_SpriteName))
            {
                m_Atlas.RemoveTexture(m_SpriteName, clearRange);
            }
        }
        #endregion
    }
}