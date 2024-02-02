using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using Sandbox.ModAPI;
using VRage.Collections;
using VRage.Game;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRage.Utils;
using VRage.Voxels;
using VRageMath;

namespace AtmosphericDamage
{
    public static class Utilities
    {
        private static readonly Random Random = new Random();
        public static List<LineD> Lines = new List<LineD>();
        public static bool Debug;

        /// <summary>
        ///     Gets a block on the exterior hull of a ship
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="blocks"></param>
        /// <returns></returns>
        public static IMySlimBlock GetRandomExteriorBlock(IMyCubeGrid grid, List<IMySlimBlock> blocks)
        {
            Vector3D posInt = grid.GridIntegerToWorld(blocks.GetRandomItemFromList().Position);
            Vector3D posExt = RandomPositionFromPoint(ref posInt, grid.WorldAABB.HalfExtents.Length());

            if (Debug)
                Lines.Add(new LineD {From = posExt, To = posInt});

            Vector3I? blockPos = grid.RayCastBlocks(posExt, posInt);
            return blockPos.HasValue ? grid.GetCubeBlock(blockPos.Value) : null;
        }

        public static IMySlimBlock GetRandomSkyFacingBlock(IMyCubeGrid grid, List<IMySlimBlock> blocks, Vector3D gravityDirection, bool physicsCast = false)
        {
            LineD line;
            return GetRandomSkyFacingBlock(grid, blocks, gravityDirection, out line, physicsCast);
        }

        /// <summary>
        ///     Finds a random block facing the sky. Raycast is slightly randomized to simulate wind blowing rain
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="blocks"></param>
        /// <param name="gravityDirection"></param>
        /// <param name="physicsCast"></param>
        /// <returns></returns>
        public static IMySlimBlock GetRandomSkyFacingBlock(IMyCubeGrid grid, List<IMySlimBlock> blocks, Vector3D gravityDirection, out LineD line, bool physicsCast = false)
        {
            Vector3D posInt = grid.GridIntegerToWorld(blocks.GetRandomItemFromList().Position) + -gravityDirection * 2;
            Vector3D posExt = posInt + gravityDirection * Math.Max(25, grid.WorldVolume.Radius * 2);
            posExt = RandomPositionFromPoint(ref posExt, grid.WorldVolume.Radius);
            line = new LineD();

            Vector3I? blockPos = grid.RayCastBlocks(posExt, posInt);
            if (!blockPos.HasValue)
                return null;

            IMySlimBlock block = grid.GetCubeBlock(blockPos.Value);

            if (block == null)
                return null;

            if (physicsCast)
            {
                var hits = new List<IHitInfo>();
                MyAPIGateway.Physics.CastRay(posExt, grid.GridIntegerToWorld(blockPos.Value), hits);
                foreach (IHitInfo hit in hits)
                {
                    if (hit?.HitEntity?.GetTopMostParent() == null)
                        continue;

                    if (hit.HitEntity.GetTopMostParent().EntityId != grid.EntityId)
                        return null;
                }
            }

            if (Debug)
                Lines.Add(new LineD {From = posExt, To = grid.GridIntegerToWorld(blockPos.Value)});
            line = new LineD {From = posExt, To = grid.GridIntegerToWorld(blockPos.Value)};

            return block;
        }

        public static List<IMySlimBlock> GetBlocksContactingVoxel(IMyCubeGrid grid, MyPlanet planet, out byte voxelType)
        {
            Vector3D[] corners = grid.WorldAABB.GetCorners();
            if (corners.All(c => Vector3D.DistanceSquared(c, planet.GetClosestSurfacePointGlobal(c)) > 2500))
            {
                voxelType = 0;
                return new List<IMySlimBlock>();
            }

            Vector3D planetPos = planet.PositionComp.GetPosition();

            var result = new MyConcurrentHashSet<IMySlimBlock>();
            var blocks = new List<IMySlimBlock>();
            grid.GetBlocks(blocks);
            byte id = 0;
            MyAPIGateway.Parallel.ForEach(blocks, block =>
                                                  {
                                                      try
                                                      {
                                                          var b = block.GetPosition();
                                                          Vector3D s = planet.GetClosestSurfacePointGlobal(ref b);

                                                          if (Vector3D.DistanceSquared(planetPos, s) > Vector3D.DistanceSquared(planetPos, b))
                                                          {
                                                              var hits = new List<IHitInfo>();
                                                              MyAPIGateway.Physics.CastRay(b, s, hits);
                                                              foreach (IHitInfo hit in hits)
                                                              {
                                                                  if (hit.HitEntity is IMyVoxelBase)
                                                                      s = hit.Position;
                                                              }
                                                          }

                                                          double cd = Vector3D.DistanceSquared(b, s);
                                                          if (cd > 200)
                                                              return;

                                                          if (cd > 6.25)
                                                          {
                                                              BoundingBoxD box;
                                                              block.GetWorldBoundingBox(out box, true);
                                                              Vector3D[] bc = box.GetCorners();
                                                              if (bc.All(c => Vector3D.DistanceSquared(c, planet.GetClosestSurfacePointGlobal(c)) > 6.25))
                                                                  return;
                                                          }

                                                          MyVoxelMaterialDefinition mat = planet.GetMaterialAt_R(ref s);
                                                          if (Config.VOXEL_IDS.ContainsKey(mat.Index))
                                                              result.Add(block);
                                                          id = mat.Index;
                                                      }
                                                      catch
                                                      {
                                                          //meh!
                                                      }
                                                  });
            voxelType = id;
            return result.ToList();

            var chunks = new HashSet<Vector3I[]>();

            int chunkSize = grid.GridSizeEnum == MyCubeSize.Large ? 3 : 5;
            for (int x = grid.Min.X; x < grid.Max.X + chunkSize; x += chunkSize)
            {
                for (int y = grid.Min.Y; y < grid.Max.Y + chunkSize; y += chunkSize)
                {
                    for (int z = grid.Min.Z; z < grid.Max.Z + chunkSize; z += chunkSize)
                    {
                        chunks.Add(new[]
                                   {
                                       new Vector3I(x, y, z),
                                       new Vector3I(x + chunkSize, y, z),
                                       new Vector3I(x, y + chunkSize, z),
                                       new Vector3I(x, y, z + chunkSize),
                                       new Vector3I(x + chunkSize, y + chunkSize, z),
                                       new Vector3I(x + chunkSize, y, z + chunkSize),
                                       new Vector3I(x, y + chunkSize, z + chunkSize),
                                       new Vector3I(x + chunkSize, y + chunkSize, z + chunkSize),
                                   });
                    }
                }
            }

            MyAPIGateway.Parallel.ForEach(chunks, chunk =>
                                                  {
                                                      var success = false;
                                                      foreach (Vector3I pos in chunk)
                                                      {
                                                          Vector3D d = grid.GridIntegerToWorld(pos);
                                                          Vector3D s = planet.GetClosestSurfacePointGlobal(ref d);
                                                          if (Vector3D.DistanceSquared(d, s) > 6.25)
                                                              continue;

                                                          if (!Config.VOXEL_IDS.ContainsKey(planet.GetMaterialAt_R(ref s).Index))
                                                              continue;
                                                          success = true;
                                                          break;
                                                      }

                                                      if (!success)
                                                          return;

                                                      foreach (Vector3I pos in chunk)
                                                      {
                                                          IMySlimBlock block = grid.GetCubeBlock(pos);
                                                          if (block != null)
                                                              result.Add(block);
                                                      }
                                                  });

            return result.ToList();
        }

        /// <summary>
        ///     Randomizes a vector by the given amount
        /// </summary>
        /// <param name="start"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        public static Vector3D RandomPositionFromPoint(ref Vector3D start, double distance)
        {
            double z = Random.NextDouble() * 2 - 1;
            double piVal = Random.NextDouble() * 2 * Math.PI;
            double zSqrt = Math.Sqrt(1 - z * z);
            var direction = new Vector3D(zSqrt * Math.Cos(piVal), zSqrt * Math.Sin(piVal), z);

            //var mat = MatrixD.CreateFromYawPitchRoll(RandomRadian, RandomRadian, RandomRadian);
            //Vector3D direction = Vector3D.Transform(start, mat);
            direction.Normalize();
            start += direction * -2;
            return start + direction * distance;
        }

        public static double RandomRadian()
        {
            return Random.NextDouble() * MathHelper.TwoPi;
        }

        public static bool IsEntityCovered(IMyEntity entity, Vector3D planetPos)
        {
            if (entity?.Physics == null)
                return false;

            Vector3D direction = entity.WorldVolume.Center - planetPos;
            direction.Normalize();

            var hits = new List<IHitInfo>();
            MyAPIGateway.Physics.CastRay(entity.WorldVolume.Center, entity.WorldVolume.Center + direction * 50, hits);

            foreach (IHitInfo hit in hits)
            {
                if (hit?.HitEntity == null)
                    continue;

                if (hit.HitEntity.EntityId == entity.EntityId)
                    continue;

                return true;
            }

            return false;
        }

        public static bool IsFullyInsideVoxel(IMyEntity entity, MyPlanet planet)
        {
            MatrixD transform = entity.WorldMatrix;
            return planet.AreAllAabbCornersInside(ref transform, (BoundingBoxD)entity.LocalAABB);
        }

        public static bool IsEntityInsideGrid(IMyEntity grid)
        {
            if (grid?.Physics == null)
                return false;

            BoundingSphereD sphere = grid.WorldVolume;
            sphere.Radius *= 10;
            List<IMyEntity> entities = MyAPIGateway.Entities.GetTopMostEntitiesInSphere(ref sphere);

            foreach (IMyEntity entity in entities)
            {
                if (entity == null)
                    continue;

                if (entity.EntityId == grid.EntityId)
                    continue;

                var parentGrid = entity as IMyCubeGrid;
                if (parentGrid == null)
                    continue;

                var hitCount = 0;
                if (parentGrid.RayCastBlocks(sphere.Center, sphere.Center + grid.WorldMatrix.Forward * sphere.Radius * 10) != null)
                    hitCount++;

                if (parentGrid.RayCastBlocks(sphere.Center, sphere.Center + -grid.WorldMatrix.Forward * sphere.Radius * 10) != null)
                    hitCount++;

                if (parentGrid.RayCastBlocks(sphere.Center, sphere.Center + grid.WorldMatrix.Left * sphere.Radius * 10) != null)
                    hitCount++;

                if (parentGrid.RayCastBlocks(sphere.Center, sphere.Center + -grid.WorldMatrix.Left * sphere.Radius * 10) != null)
                    hitCount++;

                if (parentGrid.RayCastBlocks(sphere.Center, sphere.Center + grid.WorldMatrix.Up * sphere.Radius * 10) != null)
                    hitCount++;

                if (parentGrid.RayCastBlocks(sphere.Center, sphere.Center + -grid.WorldMatrix.Up * sphere.Radius * 10) != null)
                    hitCount++;

                if (hitCount > 3)
                    return true;
            }

            return false;
        }
    }

    public static class Extensions
    {
        private static readonly Random rng = new Random();

        public static bool Closed(this IMySlimBlock block)
        {
            if (block.FatBlock != null)
                return block.FatBlock.Closed;
            return block.CubeGrid.GetCubeBlock(block.Position) == null;
        }

        public static Vector3D GetPosition(this IMySlimBlock block)
        {
            return block.CubeGrid.GridIntegerToWorld(block.Position);
        }

        public static void Log(this object ob)
        {
            MyLog.Default.WriteLine(ob.ToString());
        }

        public static void AddOrUpdate<T>(this MyConcurrentDictionary<T, float> dic, T key, float value)
        {
            if (dic.ContainsKey(key))
                dic[key] += value;
            else
                dic[key] = value;
        }

        public static void AddOrUpdate<T>(this Dictionary<T, float> dic, T key, float value)
        {
            if (dic.ContainsKey(key))
                dic[key] += value;
            else
                dic[key] = value;
        }

        public static void AddOrUpdate<T>(this Dictionary<T, int> dic, T key, int value)
        {
            if (dic.ContainsKey(key))
                dic[key] += value;
            else
                dic[key] = value;
        }

        public static MyVoxelMaterialDefinition GetMaterialAt_R(this MyVoxelBase self, ref Vector3D worldPosition)
        {
            Vector3 localHitPos;
            MyVoxelCoordSystems.WorldPositionToLocalPosition(worldPosition, self.PositionComp.WorldMatrix, self.PositionComp.WorldMatrixInvScaled, self.SizeInMetres / 2f, out localHitPos);
            Vector3I voxelPosition = new Vector3I(localHitPos / MyVoxelConstants.VOXEL_SIZE_IN_METRES) + self.StorageMin;

            return self.Storage.GetMaterialAt_R(ref voxelPosition);
        }

        public static MyVoxelMaterialDefinition GetMaterialAt_R(this IMyStorage self, ref Vector3I voxelCoords)
        {
            MyVoxelMaterialDefinition def;

            var cache = new MyStorageData();
            cache.Resize(Vector3I.One);
            cache.ClearMaterials(0);

            self.ReadRange(cache, MyStorageDataTypeFlags.Material, 0, voxelCoords, voxelCoords);

            def = MyDefinitionManager.Static.GetVoxelMaterialDefinition(cache.Material(0));

            return def;
        }

        public static Vector3D GetClosestSurfacePointGlobal(this MyPlanet planet, Vector3D pos)
        {
            return planet.GetClosestSurfacePointGlobal(ref pos);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static bool IsZero(this Vector3D vec)
        {
            return Vector3D.IsZero(vec);
        }

        public static void GetIntersect(Vector3D center, double radius, IMyCubeGrid query, List<IMySlimBlock> result)
        {
            Vector3I gc = query.WorldToGridInteger(center);
            double rc = radius / query.GridSize;
            query.GetBlocks(result, s => DistanceSquared(gc, s.Position) < rc);
        }

        public static double DistanceSquared(Vector3I value1, Vector3I value2)
        {
            int num1 = value1.X - value2.X;
            int num2 = value1.Y - value2.Y;
            int num3 = value1.Z - value2.Z;
            return num1 * num1 + num2 * num2 + num3 * num3;
        }
    }
}
