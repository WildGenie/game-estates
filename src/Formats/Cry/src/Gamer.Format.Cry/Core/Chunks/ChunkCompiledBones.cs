﻿using System.Collections.Generic;
using System.Linq;
using static Gamer.Core.Debug;

namespace Gamer.Format.Cry.Core
{
    public abstract class ChunkCompiledBones : Chunk     //  0xACDC0000:  Bones info
    {
        public string RootBoneName;         // Controller ID?  Name?  Not sure yet.
        public CompiledBone RootBone;       // First bone in the data structure.  Usually Bip01
        public int NumBones;                // Number of bones in the chunk
        // Bones are a bit different than Node Chunks, since there is only one CompiledBones Chunk, and it contains all the bones in the model.
        public Dictionary<int, CompiledBone> BoneDictionary = new Dictionary<int, CompiledBone>();  // Dictionary of all the CompiledBone objects based on parent offset(?).
        public List<CompiledBone> BoneList = new List<CompiledBone>();

        public CompiledBone GetParentBone(CompiledBone bone, int boneIndex) => bone.offsetParent != 0 ? BoneDictionary[boneIndex + bone.offsetParent] : null; // Should only be one parent.

        public List<CompiledBone> GetAllChildBones(CompiledBone bone) => bone.numChildren > 0 ? BoneList.Where(a => bone.childIDs.Contains(a.ControllerID)).ToList() : null;

        public List<string> GetBoneNames() => BoneList.Select(a => a.boneName).ToList();  // May need to replace space in bone names with _.

        protected void AddChildIDToParent(CompiledBone bone)
        {
            // Root bone parent ID will be zero.
            if (bone.parentID != 0)
                BoneList.Where(a => a.ControllerID == bone.parentID).FirstOrDefault()?.childIDs.Add(bone.ControllerID); // Should only be one parent.
        }

        protected Matrix44 GetTransformFromParts(Vector3 localTranslation, Matrix33 localRotation) => new Matrix44
        {
            // Translation part
            m14 = localTranslation.x,
            m24 = localTranslation.y,
            m34 = localTranslation.z,
            // Rotation part
            m11 = localRotation.m11,
            m12 = localRotation.m12,
            m13 = localRotation.m13,
            m21 = localRotation.m21,
            m22 = localRotation.m22,
            m23 = localRotation.m23,
            m31 = localRotation.m31,
            m32 = localRotation.m32,
            m33 = localRotation.m33,
            // Set final row
            m41 = 0,
            m42 = 0,
            m43 = 0,
            m44 = 1
        };

        protected void SetRootBoneLocalTransformMatrix()
        {
            RootBone.LocalTransform.m11 = RootBone.boneToWorld.boneToWorld[0, 0];
            RootBone.LocalTransform.m12 = RootBone.boneToWorld.boneToWorld[0, 1];
            RootBone.LocalTransform.m13 = RootBone.boneToWorld.boneToWorld[0, 2];
            RootBone.LocalTransform.m14 = RootBone.boneToWorld.boneToWorld[0, 3];
            RootBone.LocalTransform.m21 = RootBone.boneToWorld.boneToWorld[1, 0];
            RootBone.LocalTransform.m22 = RootBone.boneToWorld.boneToWorld[1, 1];
            RootBone.LocalTransform.m23 = RootBone.boneToWorld.boneToWorld[1, 2];
            RootBone.LocalTransform.m24 = RootBone.boneToWorld.boneToWorld[1, 3];
            RootBone.LocalTransform.m31 = RootBone.boneToWorld.boneToWorld[2, 0];
            RootBone.LocalTransform.m32 = RootBone.boneToWorld.boneToWorld[2, 1];
            RootBone.LocalTransform.m33 = RootBone.boneToWorld.boneToWorld[2, 2];
            RootBone.LocalTransform.m34 = RootBone.boneToWorld.boneToWorld[2, 3];
            RootBone.LocalTransform.m41 = 0;
            RootBone.LocalTransform.m42 = 0;
            RootBone.LocalTransform.m43 = 0;
            RootBone.LocalTransform.m44 = 1;
        }

        public override void WriteChunk()
        {
            Log($"*** START CompiledBone Chunk ***");
            Log($"    ChunkType:           {ChunkType}");
            Log($"    Node ID:             {ID:X}");
        }
    }

}
