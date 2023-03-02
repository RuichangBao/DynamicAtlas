using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GFrame
{
    public class DynamicAtlasMgr : Singleton<DynamicAtlasMgr>
    {
        private Dictionary<DynamicAtlasGroup, DynamicAtlas> m_DynamicAtlasMap = new Dictionary<DynamicAtlasGroup, DynamicAtlas>();

        private Queue<GetTextureData> m_GetTextureDataQueue = new Queue<GetTextureData>();
        private Queue<SaveTextureData> m_SaveTextureDataQueue = new Queue<SaveTextureData>();
        private Queue<IntegerRectangle> m_IntegerRectangleQueue = new Queue<IntegerRectangle>();

        public DynamicAtlas GetDynamicAtlas(DynamicAtlasGroup group)
        {
            DynamicAtlas atlas;
            if (m_DynamicAtlasMap.ContainsKey(group))
            {
                atlas = m_DynamicAtlasMap[group];
            }
            else
            {
                atlas = new DynamicAtlas(group);
                m_DynamicAtlasMap[group] = atlas;
            }
            return atlas;
        }


        public SaveTextureData AllocateSaveTextureData()
        {
            if (m_SaveTextureDataQueue.Count > 0)
            {
                return m_SaveTextureDataQueue.Dequeue();
            }
            SaveTextureData data = new SaveTextureData();
            return data;
        }

        public void ReleaseSaveTextureData(SaveTextureData data)
        {
            m_SaveTextureDataQueue.Enqueue(data);
        }

        public GetTextureData AllocateGetTextureData()
        {
            if (m_GetTextureDataQueue.Count > 0)
            {
                return m_GetTextureDataQueue.Dequeue();
            }
            GetTextureData data = new GetTextureData();
            return data;
        }

        public void ReleaseGetTextureData(GetTextureData data)
        {
            m_GetTextureDataQueue.Enqueue(data);
        }

        public IntegerRectangle AllocateIntegerRectangle(int x, int y, int width, int height)
        {
            if (m_IntegerRectangleQueue.Count > 0)
            {
                IntegerRectangle rectangle = m_IntegerRectangleQueue.Dequeue();
                rectangle.x = x;
                rectangle.y = y;
                rectangle.width = width;
                rectangle.height = height;
                //这个地方看不太懂后期修改 包芮昌
                //return rectangle;
            }
            return new IntegerRectangle(x, y, width, height);
        }

        public void ReleaseIntegerRectangle(IntegerRectangle rectangle)
        {
            m_IntegerRectangleQueue.Enqueue(rectangle);
        }

        public void ClearAllCache()
        {
            m_GetTextureDataQueue.Clear();
            m_SaveTextureDataQueue.Clear();
            m_IntegerRectangleQueue.Clear();
        }
    }


}