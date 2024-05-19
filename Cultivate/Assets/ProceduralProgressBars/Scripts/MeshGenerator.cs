using System.Collections.Generic;
using UnityEngine;

namespace Renge.PPB {
    public class MeshGenerator {
        public static void GenerateSquareMesh(Mesh mesh, int subdivisions, bool leftToRightFill, bool use16BitIndices = false) {
            if (use16BitIndices) {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
            } else {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

            }

            mesh.name = "SquareMesh";
            mesh.triangles = null;
            mesh.vertices = null;
            mesh.uv = null;


            //clamp subdivisions to reasonable values
            if (subdivisions < 0) subdivisions = 0;
            if (subdivisions > 10) subdivisions = 10;
            int sides = (int)Mathf.Pow(2, subdivisions) + 1;
            Vector3[] vertices = new Vector3[sides * sides];
            Vector2[] uv = new Vector2[sides * sides];
            int[] tris = new int[(sides - 1) * (sides - 1) * 6];
            int triCounter = 0;
            for (int x = 0; x < sides; x++) {
                for (int y = 0; y < sides; y++) {
                    var vertex01 = new Vector2(x / (float)(sides - 1), y / (float)(sides - 1));
                    vertices[x * sides + y] = new Vector3(vertex01.x - .5f, vertex01.y - .5f, 0);
                    //uv[x * sides + y] = vertex01;

                    if (x < sides - 1 && y < sides - 1 && triCounter < tris.Length) {
                        int curr = x * sides + y;
                        tris[triCounter] = curr;
                        tris[triCounter + 1] = curr + sides + 1;
                        tris[triCounter + 2] = curr + sides;

                        tris[triCounter + 3] = curr;
                        tris[triCounter + 4] = curr + 1;
                        tris[triCounter + 5] = curr + sides + 1;
                        triCounter += 6;
                    }
                }
            }
            int dir = leftToRightFill ? 1 : -1;
            int xSideIndex = 0;
            for (int x = leftToRightFill ? 0 : sides - 1; leftToRightFill ? x < sides : x >= 0; x += dir) {
                for (int y = 0; y < sides; y++) {
                    uv[x * sides + y] = vertices[xSideIndex * sides + y] + new Vector3(0.5f, 0.5f, 0);
                }
                xSideIndex++;
            }
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = tris;
            //mesh.RecalculateNormals();
        }

        public static void GenerateRingMesh(Mesh mesh, int sides, float radius, float width, float arc, bool autoArcOffset, float autoArcOffsetThreshold, float circleLength, int loops, float subdivisionDetail, float corkScrew, float faceRotation, bool clockwiseFill, bool use16BitIndices = false) {
            if (use16BitIndices) {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt16;
            } else {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }
            mesh.name = "CircleMesh";
            mesh.triangles = null;
            mesh.vertices = null;
            mesh.uv = null;

            List<Vector3> vertices = new List<Vector3>();
            List<Vector2> uv1 = new List<Vector2>();
            List<Vector2> uv2 = new List<Vector2>();
            List<int> tris = new List<int>();

            #region vertex generation
            float tau = Mathf.PI * 2;
            float pi = Mathf.PI;
            float lengthAdjustedSides = circleLength * sides;
            float radianStep = circleLength * tau * (1 - arc) / lengthAdjustedSides;
            float radianOffset = -pi / 2;
            float outerRadius = radius + width / 2;
            Vector3 offset = new Vector3();
            if (autoArcOffset && arc > autoArcOffsetThreshold) {
                offset = new Vector3(0, -radius);
            }

            //generate initial vertices
            //+ 1, since the circle isn't closed together at the ends.
            for (int i = 0; i <= (int)lengthAdjustedSides; i++) {
                float currentRadian = radianStep * i + radianOffset + pi * arc;
                //all vertices for one side
                for (int j = 0; j < loops; j++) {
                    var vertex = new Vector3(Mathf.Cos(currentRadian), Mathf.Sin(currentRadian)) * (outerRadius - (width / (loops - 1)) * j) + offset;
                    vertex.z = ((radianStep * i) / tau) * corkScrew;
                    //Vector3 centerPoint = new Vector3(radius, 0.1f, vertex.z);
                    //vertex = faceRotation * (vertex - new Vector3(0.1f, 0.1f, 0.1f)) + new Vector3();
                    vertices.Add(vertex);
                }
            }


            #endregion

            #region subdivision
            //subdivide vertices
            int subdivisions = 0;
            if (radianStep >= tau / 5)
                subdivisions = (int)(4 * subdivisionDetail);
            else if (radianStep >= tau / 12)
                subdivisions = (int)(3 * subdivisionDetail);
            else if (radianStep >= tau / 24)
                subdivisions = (int)(2 * subdivisionDetail);
            else if (radianStep >= tau / 49)
                subdivisions = (int)(1 * subdivisionDetail);
            else
                subdivisions = 0;

            subdivisions -= (int)(circleLength / Mathf.Max(4.01f * subdivisionDetail, 1.0f));
            //subdivisions = 0;

            var vc = vertices.Count;
            while (vc * Mathf.Pow(2, subdivisions) > 50000) {
                subdivisions--;
                if (subdivisions == 0) {
                    break;
                }
            }


            for (int i = 0; i < subdivisions; i++) {
                int sidesAfterSubdivide = (vertices.Count / loops) * 2 - 1;
                for (int j = 0; j < sidesAfterSubdivide - 2; j += 2) {
                    int currentIndex = j * loops;
                    for (int k = 0; k < loops; k++) {
                        vertices.Insert(currentIndex + loops + k, (vertices[currentIndex + k] + vertices[currentIndex + loops + k + k]) / 2);
                    }
                }
            }
            #endregion

            #region uv generation
            int c = vertices.Count;
            int totalSides = c / loops;
            //uvs
            Vector2 size = mesh.bounds.size;
            for (int i = 0; i < c; i++) {
                int loop = i % loops;
                int side = i / loops;

                uv1.Add(new Vector2(clockwiseFill ? 1 - (float)(side * loops) / (c - loops) : (float)(side * loops) / (c - loops), 1.0f - (float)loop / (loops - 1)));
                //uv1.Add(new Vector2(vertices[i].x / (size.x) + 0.5f, vertices[i].y / (size.y) + 0.5f));
                //uv1.Add(vertices[i]);
            }
            #endregion

            #region triangle generation
            //triangles
            for (int i = 0; i < c - loops; i++) {
                //no triangles for inner most vertex loop
                if (i % loops == loops - 1) continue;

                tris.Add(i);
                tris.Add(i + loops + 1);
                tris.Add(i + loops);

                tris.Add(i);
                tris.Add(i + 1);
                tris.Add(i + loops + 1);
            }
            #endregion

            mesh.vertices = vertices.ToArray();
            mesh.triangles = tris.ToArray();

            mesh.uv = uv1.ToArray();
            //mesh.RecalculateNormals();
        }
    }
}