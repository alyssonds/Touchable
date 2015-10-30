using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Framework.TokenEngine;
using Assets.Framework.MultiTouchManager;
using MathNet.Numerics.LinearAlgebra;

using UnityEngine;

namespace Assets.Framework.Utils
{
    internal static class TokenUtils
    {

        public static Dictionary<int,TokenMarker> ConvertTouchInputToMarkers(int[] orderedIndexes, Dictionary<int,TouchInput> clusterPoints)
        {
            Dictionary<int, TokenMarker> result = new Dictionary<int, TokenMarker>();

            for (int i = 0; i < orderedIndexes.Length; i++)
            {
                TouchInput ti = clusterPoints[orderedIndexes[i]];

                switch (i)
                {
                    case 0:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.Origin);
                            result.Add(tm.Id, tm);
                            break;
                        }
                    case 1:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.XAxis);
                            result.Add(tm.Id, tm);
                            break;
                        }
                    case 2:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.YAxis);
                            result.Add(tm.Id, tm);
                            break;
                        }
                    case 3:
                        {
                            TokenMarker tm = new TokenMarker(ti.Id, ti.Position, ti.State, MarkerType.Data);
                            result.Add(tm.Id, tm);
                            break;
                        }
                }
            }

            return result;
        }

        public static TokenMarker[] MeanSquareOrthogonalReferenceSystem(TokenMarker originalOrigin, TokenMarker originalXAxis, TokenMarker originalYAxis, float tokenSize)
        {
            TokenMarker[] result = new TokenMarker[3];
            var M= Matrix<float>.Build;


            float[,] arrayA = { { 0.0f, 0.0f, 1.0f, 0.0f },
                              { 0.0f, 0.0f, 0.0f, 1.0f },
                              { tokenSize, 0.0f, 1.0f, 0.0f },
                              { 0.0f, tokenSize, 0.0f, 1.0f },
                              { 0.0f, -tokenSize, 1.0f, 0.0f},
                              { tokenSize, 0.0f, 0.0f, 1.0f } };

            float[,] arrayB = { { originalOrigin.Position.x },
                                { originalOrigin.Position.y },
                                { originalXAxis.Position.x },
                                { originalXAxis.Position.y },
                                { originalYAxis.Position.x },
                                { originalYAxis.Position.y} };

            var A = M.DenseOfArray(arrayA);
            var b = M.DenseOfArray(arrayB);

            var x = A.TransposeThisAndMultiply(A).Inverse() * A.TransposeThisAndMultiply(b);

            float[,] transformationMatrix = x.ToArray();

            Vector2 newOrigin = new Vector2( transformationMatrix[2, 0], 
                                             transformationMatrix[3, 0]);
            Vector2 newXAxis = new Vector2( tokenSize * transformationMatrix[0,0] + transformationMatrix[2,0],
                                            tokenSize * transformationMatrix[1,0] + transformationMatrix[3,0]);
            Vector2 newYAxis = new Vector2(-tokenSize * transformationMatrix[1, 0] + transformationMatrix[2, 0],
                                             tokenSize * transformationMatrix[0, 0] + transformationMatrix[3, 0]);

            result[0] = new TokenMarker(originalOrigin.Id, newOrigin, originalOrigin.State, MarkerType.Origin);
            result[1] = new TokenMarker(originalXAxis.Id, newXAxis, originalXAxis.State, MarkerType.XAxis);
            result[2] = new TokenMarker(originalYAxis.Id, newYAxis, originalYAxis.State, MarkerType.YAxis);

            return result;
        }

        public static Vector2 ComputeRotoTranslation(Vector2 originalPosition, Vector2 newOriginPosition, float angleRad)
        {
            var M = Matrix<float>.Build;
            var v = Vector<float>.Build;
            float angleCos = Mathf.Cos(angleRad);
            float angleSin = Mathf.Sin(angleRad);

            float[,] rotation = { { angleCos , angleSin },
                                  { -angleSin, angleCos }};

            float[] translation = { originalPosition.x - newOriginPosition.x, originalPosition.y - newOriginPosition.y };

            var R = M.DenseOfArray(rotation);
            var T = v.DenseOfArray(translation);

            float[] result = (R * T).ToArray();

            return new Vector2(result[0], result[1]);

        }
    }
}
