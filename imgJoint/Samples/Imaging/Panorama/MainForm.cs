﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Accord.Imaging;
using Accord.Imaging.Filters;
using Accord.Math;
using AForge;

namespace Panorama
{
    public partial class MainForm : Form
    {
        private Bitmap img1 = Panorama.Properties.Resources.desk3;
        private Bitmap img2 = Panorama.Properties.Resources.desk4;

        private IntPoint[] harrisPoints1;
        private IntPoint[] harrisPoints2;

        private IntPoint[] correlationPoints1;
        private IntPoint[] correlationPoints2;

        private MatrixH homography;


        public MainForm()
        {
            InitializeComponent();            

            // Concatenate and show entire image at start
            Concatenate concatenate = new Concatenate(img1);
            pictureBox.Image = concatenate.Apply(img2);
        }


        private void btnHarris_Click(object sender, EventArgs e)
        {
            // Step 1: Detect feature points using Harris Corners Detector
            HarrisCornersDetector harris = new HarrisCornersDetector(0.04f, 1000f);
            harrisPoints1 = harris.ProcessImage(img1).ToArray();
            harrisPoints2 = harris.ProcessImage(img2).ToArray();

            // Show the marked points in the original images
            Bitmap img1mark = new PointsMarker(harrisPoints1).Apply(img1);
            Bitmap img2mark = new PointsMarker(harrisPoints2).Apply(img2);

            // Concatenate the two images together in a single image (just to show on screen)
            Concatenate concatenate = new Concatenate(img1mark);
            pictureBox.Image = concatenate.Apply(img2mark);
        }


        private void btnCorrelation_Click(object sender, EventArgs e)
        {
            // Step 2: Match feature points using a correlation measure
            CorrelationMatching matcher = new CorrelationMatching(9);
            IntPoint[][] matches = matcher.Match(img1, img2, harrisPoints1, harrisPoints2);

            // Get the two sets of points
            correlationPoints1 = matches[0];
            correlationPoints2 = matches[1];

            // Concatenate the two images in a single image (just to show on screen)
            Concatenate concat = new Concatenate(img1);
            Bitmap img3 = concat.Apply(img2);

            // Show the marked correlations in the concatenated image
            PairsMarker pairs = new PairsMarker(
                correlationPoints1, // Add image1's width to the X points to show the markings correctly
                correlationPoints2.Apply(p => new IntPoint(p.X + img1.Width, p.Y)));

            pictureBox.Image = pairs.Apply(img3);
        }

        private void btnRansac_Click(object sender, EventArgs e)
        {
            // Step 3: Create the homography matrix using a robust estimator
            RansacHomographyEstimator ransac = new RansacHomographyEstimator(0.001, 0.99);
            homography = ransac.Estimate(correlationPoints1, correlationPoints2);

            // Plot RANSAC results against correlation results
            IntPoint[] inliers1 = correlationPoints1.Submatrix(ransac.Inliers);
            IntPoint[] inliers2 = correlationPoints2.Submatrix(ransac.Inliers);

            // Concatenate the two images in a single image (just to show on screen)
            Concatenate concat = new Concatenate(img1);
            Bitmap img3 = concat.Apply(img2);

            // Show the marked correlations in the concatenated image
            PairsMarker pairs = new PairsMarker(
                inliers1, // Add image1's width to the X points to show the markings correctly
                inliers2.Apply(p => new IntPoint(p.X + img1.Width, p.Y)));

            pictureBox.Image = pairs.Apply(img3);
        }

        private void btnBlend_Click(object sender, EventArgs e)
        {
            // Step 4: Project and blend the second image using the homography
            Blend blend = new Blend(homography, img1);
            pictureBox.Image = blend.Apply(img2);
        }

        private void btnDoItAll_Click(object sender, EventArgs e)
        {
            // Do it all
            btnHarris_Click(sender, e);
            btnCorrelation_Click(sender, e);
            btnRansac_Click(sender, e);
            btnBlend_Click(sender, e);
        }

        private void Sift_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox_Click_1(object sender, EventArgs e)
        {

        }

        private void Sift_Click_1(object sender, EventArgs e)
        {

        }



    }
}
