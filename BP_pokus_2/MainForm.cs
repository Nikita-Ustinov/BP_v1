using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace BP_pokus_2
{
	
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			String file = "Arnot_Vesel_30x39.bmp";
			String vysledek = ImgToRightString(file);
			textBox1.Text = vysledek;
			
		}
		
		string ImgToRightString(String file) {
			Image img = Image.FromFile(file);
			Bitmap bm = new Bitmap(img);
			Color color =new Color();
			String vysledek= null;
			
			for (int i=0;i<39; i++) {
				for (int j=0; j<30; j++) {
					color = bm.GetPixel(j,i);
					vysledek += color.GetBrightness().ToString();
				}
				vysledek+="\r\n";
			}
			return vysledek;
		}
		
		
		string ImgToStr(string file) {
			MemoryStream stream = new MemoryStream();
			Image img = Image.FromFile(file);
			Bitmap bm = new Bitmap(img);
			Color color =bm.GetPixel(15,0);
			
//			img.Save(stream, img.RawFormat);
//			byte[] arrayImg = stream.ToArray();
			return color.GetBrightness().ToString();
				// Convert.ToBase64String(arrayImg);
		}
	}
}
