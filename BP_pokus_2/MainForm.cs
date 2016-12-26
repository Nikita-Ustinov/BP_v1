using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace BP_pokus_2
{
	[Serializable]
	public partial class MainForm : Form
	{	String prubeh;
		int countwriteInfo=0;
		bool [] UzByli = new bool[] {false, false, false, false, false, false , false, false, false, false};				//, 
		
		public void newEpoch() {
			for (int i=0; i<10; i++) {
				UzByli[i] = false;
			}
		}
		public MainForm(String s){}					//pro pristup ostatnich trid
		
		public MainForm()
		{
			InitializeComponent();			
		}
		
		public String getLine(Program p) {
			Random rand = new Random();
			int a;
			
			do {
			  a =rand.Next(0,10);
			} while(UzByli[a] == true);
			
			String fileName = a.ToString()+".bmp";
			UzByli[a] = true;
			p.answer = a;
			return ImgToRightString(fileName);
		}
		
//		
//		public String getLine(Program p) {
//			String [] line = new String[]{"000000000000000","000000000000111"};
//			Random rand = new Random();
//			int vysledek;
//			do {
//			  vysledek = rand.Next(0,2);
//			} while(UzByli[vysledek] == true);
//			
//			UzByli[vysledek] = true;
//			
//			if (vysledek == 0)
//				p.answer=0;
//			else 
//				p.answer = 1;
//			return line[vysledek];
//		}
		//000000000000000 0
		//000000000000111 1
		
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
			}
			return vysledek;
		}
	
		
		public void writeInfo(int epocha, int  iteration, double ErrorMin, int testValue, int counterNeuronNew) {
			countwriteInfo++;
			if (countwriteInfo<=500) {
				prubeh += "Эпоха: "+ epocha+ " "+"Значание теста: "+testValue+" "+"Минимальная ошибка: " + ErrorMin+" "+"NN: "+counterNeuronNew+"\r\n";
				File.WriteAllText("Prubeh.txt", prubeh );
			}
			else {
				countwriteInfo=0;
				prubeh="Эпоха: "+ epocha+ " "+"Значание теста: "+testValue+" "+"Минимальная ошибка: " + ErrorMin+" "+"NN: "+counterNeuronNew+"\r\n";
			}
		}
		
		
		public void writeHotovo() {
//			label4.Text = "Обучение завершено";
			
		}
		
		
		void Button1Click(object sender, EventArgs e)
		{
			this.label4.Visible = true;
			Program p = new Program().deseralizace();
//			Program p = new Program(5);
//			p.study(p);	
			p.test(p);
//			textBox4.Text = p.testOnDisplay(p);
		}
	}
}
