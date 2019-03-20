using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.Structure;

namespace _25_Face_Detection
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> imgInput;
        public Form1()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imgInput = new Image<Bgr, byte>(ofd.FileName);
                    pictureBox1.Image = imgInput.Bitmap;
                }
                else
                {
                    throw new Exception("No file selected.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void detectFaceHaarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput == null)
                {
                    throw new Exception("Please select an Image.");
                }
                DetectFaceHaar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DetectFaceHaar()
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../Harcascade classifiers/haarcascade_frontalface_default.xml");
                string eyePath = Path.GetFullPath(@"../../Harcascade classifiers/haarcascade_eye.xml");

                CascadeClassifier classifierFace = new CascadeClassifier(facePath); // Aspects a grayscale Image
                CascadeClassifier classifierEye = new CascadeClassifier(eyePath);

                var imgGray = imgInput.Convert<Gray, byte>().Clone();

                Rectangle[] faces = classifierFace.DetectMultiScale(imgGray, 1.1, 4);

                foreach (var face in faces)
                {
                    imgInput.Draw(face, new Bgr(0, 0, 255), 2);
                    // Find eyes inside faces, limit the ROI
                    imgGray.ROI = face;
                    Rectangle[] eyes = classifierEye.DetectMultiScale(imgGray, 1.1, 4);
                    foreach (var eye in eyes)
                    {
                        // Retrieve the exact location for the full image
                        var e = eye;
                        e.X += face.X;
                        e.Y += face.Y;
                        imgInput.Draw(e, new Bgr(255, 0, 0), 2);
                    }
                }
                pictureBox1.Image = imgInput.Bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void detectFaceLBPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (imgInput == null)
                {
                    throw new Exception("Please select an image.");
                }
                DetectFaceLBP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// local binary patterns method
        /// </summary>
        private void DetectFaceLBP()
        {
            try
            {
                string facePath = Path.GetFullPath(@"../../LBPcascade classifiers/lbpcascade_frontalface.xml");

                // the following class is smart enough to know the type of classifier based from XML files
                CascadeClassifier classifierFace = new CascadeClassifier(facePath);

                var imgGray = imgInput.Convert<Gray, byte>().Clone();
                Rectangle[] faces = classifierFace.DetectMultiScale(imgGray, 1.1, 2);
                foreach (var face in faces)
                {
                    imgInput.Draw(face, new Bgr(255, 0, 0));
                }

                pictureBox1.Image = imgInput.Bitmap;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
        }
    }
}
