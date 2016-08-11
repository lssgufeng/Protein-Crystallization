﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Features2D;
using System.Diagnostics;
using System.Drawing;
namespace Protein_Crystallization
{
    class Matching
    {
        public Matching()
        {
        }
        public void FindMatch(Image<Gray, Byte> modelImage, Image<Gray,byte> observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, out Matrix<int>indices, out Matrix<byte> mask, out HomographyMatrix homography)
        {
            int k = 2;
            double uniquenessThreshold = 0.8;
            SURFDetector surfCPU = new SURFDetector(500, false);
            Stopwatch watch;
            homography = null;
            
            //extract features from the objectimage
            modelKeyPoints = new VectorOfKeyPoint();
            Matrix<float>modelDescriptors = surfCPU.DetectAndCompute(modelImage, null, modelKeyPoints);
            watch = Stopwatch.StartNew();
            // extract features from theobserved image
            observedKeyPoints = new VectorOfKeyPoint();
            Matrix<float>observedDescriptors = surfCPU.DetectAndCompute(observedImage, null,observedKeyPoints);
            BruteForceMatcher<float>matcher = new BruteForceMatcher<float>(DistanceType.L2);
            matcher.Add(modelDescriptors);
            indices = new Matrix<int>(observedDescriptors.Rows, k);
            using (Matrix<float> dist =new Matrix<float>(observedDescriptors.Rows, k))
            {
               matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                mask = new Matrix<byte>(dist.Rows, 1);
                mask.SetValue(255);
               Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            }
            int nonZeroCount =CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount >= 4)
            {
                nonZeroCount =Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,indices, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                    homography =Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,observedKeyPoints, indices, mask, 2);
            }
            watch.Stop();
            matchTime = watch.ElapsedMilliseconds;               
        }
        public Image<Bgr, Byte>Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage,out long matchTime)
        {
            HomographyMatrix homography;
            VectorOfKeyPoint modelKeyPoints;
            VectorOfKeyPoint observedKeyPoints;
            Matrix<int> indices;
            Matrix<byte> mask;
            FindMatch(modelImage,observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, out indices, out mask, out homography);
            //Draw the matched keypoints
            Image<Bgr, Byte> result =Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage,observedKeyPoints,
               indices, new Bgr(255, 255, 255),new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);
            #region draw the projected regionon the image
            if (homography != null)
            { //draw a rectangle along the projected model
                Rectangle rect =modelImage.ROI;
                PointF[] pts = new PointF[] {
               new PointF(rect.Left,rect.Bottom),
               new PointF(rect.Right,rect.Bottom),
               new PointF(rect.Right,rect.Top),
               new PointF(rect.Left,rect.Top)};
                homography.ProjectPoints(pts);
               result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts,Point.Round), true, new Bgr(Color.Red), 5);
            }
            #endregion
            return result;
        }
    }
}
