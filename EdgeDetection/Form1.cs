using System;
using System.Drawing;
using System.Windows.Forms;
using ImageProcessor;
using ImageProcessor.Imaging.Filters.EdgeDetection;

namespace EdgeDetection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
            //pictureBox1.BackgroundImage = CleanBitmap(new Bitmap("skyline.png"), 2, 0.5f, 8);
            pictureBox1.BackgroundImage = new Bitmap("skyline.png");
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            ImageFactory imageFactory = new ImageFactory();
            imageFactory.Load(pictureBox1.BackgroundImage);
            IEdgeFilter edgeFilter = new SobelEdgeFilter();
            imageFactory.DetectEdges(edgeFilter, true);
            pictureBox1.BackgroundImage = imageFactory.Image;
        }

        private static Bitmap CleanBitmap(Bitmap raw, int thickness, float noiseThresh, int neighThresh)
        {
            Bitmap dirty = new Bitmap(raw);
            for (int x = 0; x < dirty.Width; x++)
            {
                for (int y = 0; y < dirty.Height; y++)
                {
                    Color pixCol = dirty.GetPixel(x, y);
                    if (pixCol.GetBrightness() > noiseThresh)
                    {
                        dirty.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        for (int i = -thickness; i < 1; i++)
                        {
                            for (int j = -thickness; j < 1; j++)
                            {
                                if ((x + i > dirty.Width) || (x + i < 0) || (y + j > dirty.Height) || (y + j < 0))
                                {
                                    continue;
                                }
                                else
                                {
                                    dirty.SetPixel(x + i, y + j, Color.Black);
                                }
                            }
                        }
                    }

                    if (x == 0 || x == dirty.Width - 1 || y == 0 || y == dirty.Height - 1)
                    {
                        dirty.SetPixel(x, y, Color.Black);
                    }
                }
            }

            for (int x = 1; x < dirty.Width - 1; x++)
            {
                for (int y = 1; y < dirty.Height - 1; y++)
                {
                    int neigh = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (dirty.GetPixel(x + i, y + j).GetBrightness() < noiseThresh)
                            {
                                neigh++;
                            }
                        }
                    }

                    if (neigh > neighThresh)
                    {
                        dirty.SetPixel(x, y, Color.Black);
                    }
                }
            }

            return new Bitmap(dirty);
        }
    }


}
