
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
namespace Habby.Material
{
    public static class MaterialHelper
    {
        static ConcurrentQueue <MaterialPropertyBlock> matPropertyQue = new ConcurrentQueue<MaterialPropertyBlock>();
        static MaterialPropertyBlock DequeueMaterialPropertyBlock()
        {
            MaterialPropertyBlock ret = null;
            if (matPropertyQue.Count > 0)
            {
                var have = matPropertyQue.TryDequeue(out ret);
                if(!have)
                {
                    ret = new MaterialPropertyBlock();
                }
                ret.Clear();
            }
            else
            {
                ret = new MaterialPropertyBlock();
            }
            return ret;
        }

        static void EnqueueMaterialPropertyBlock(MaterialPropertyBlock pBlock)
        {
            if (pBlock != null)
            {
                matPropertyQue.Enqueue(pBlock);
            }
        }

        static bool IsRight(Renderer renderer, int materialIndex = 0)
        {
            return renderer != null && renderer.sharedMaterials != null && renderer.sharedMaterials.Length > materialIndex && renderer.sharedMaterials[materialIndex] != null;
        }

        public static MaterialPropertyBlock GetMaterialPropertyBlock(this Renderer renderer, int materialIndex = 0)
        {
            if (!IsRight(renderer, materialIndex)) return null;

            var block = DequeueMaterialPropertyBlock();
            if (renderer != null && renderer.HasPropertyBlock())
            {
                renderer.GetPropertyBlock(block, materialIndex);
            }
            return block;
        }

        public static int GetNameId(string nameKey)
        {
            return Shader.PropertyToID(nameKey);
        }

        public static void SetFloat(this Renderer renderer,int nameId, float value, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer,materialId);

            block.SetFloat(nameId, value);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetFloatArray(this Renderer renderer,int nameID, List<float> values, int materialId = 0)
        {
             if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer,materialId);

            block.SetFloatArray(nameID, values);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetInt(this Renderer renderer,int nameId, int value, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer,materialId);

            block.SetInt(nameId, value);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetMatrix(this Renderer renderer,int nameId, Matrix4x4 value, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer,materialId);

            block.SetMatrix(nameId, value);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetMatrixArray(this Renderer renderer,int nameId, List<Matrix4x4> values, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer,materialId);

            block.SetMatrixArray(nameId, values);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetTexture(this Renderer renderer,int nameId, Texture value, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer,materialId);

            block.SetTexture(nameId, value);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetVector(this Renderer renderer, int nameId, Vector4 value, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer, materialId);

            block.SetVector(nameId, value);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetVectorArray(this Renderer renderer, int nameId, List<Vector4> values, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer, materialId);

            block.SetVectorArray(nameId, values);

            renderer.SetPropertyBlock(block, materialId);

            EnqueueMaterialPropertyBlock(block);
        }

        public static void SetCustomProperty(this Renderer renderer,System.Action<MaterialPropertyBlock> delgateSet, int materialId = 0)
        {
            if (!IsRight(renderer, materialId)) return;

            var block = GetMaterialPropertyBlock(renderer, materialId);

            try
            {
                delgateSet(block);
                renderer.SetPropertyBlock(block, materialId);
            }
            catch (System.Exception e)
            {
                Logger.Error("SetCustomProperty Exception:{0}",e);
            }

            EnqueueMaterialPropertyBlock(block);
        }

    }
}
