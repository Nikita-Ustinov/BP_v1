using System;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BP_pokus_2
{
	[Serializable]
	public   class Program
	{	
		public double MAX=0;
		public int inputLength=1170; 	
		public int prvniVrstva=100;
		public int druhaVrstva=50;
		public int tretiVrstva=10;
		
		public List l0,l1,l2;
		
		public int counterAddNeuron=0;
		public int epochWithoutNewNeuron=0;
		
		double speedL=0.01;
		
		public String line= null;
		public double answer;
		public double max;
		public int index;
		
		public int counterNeuronNew=0;
	
		
		public	Program(){}																//для передачи компонентов другим клаасов
		
		
		public Program (int q) 
		{
		 l0= new List(0);
		 l1= new List(1);
		 l2= new List(2);
		}
		
		
		public int calculateResult(String line)
		{	
			l0.writeInput(line);												//присваивание входного вектора нейроной сети
			l0.countOutputs();
			
			Neuron templ1=l1.head;
			for (int i=0; i<druhaVrstva; i++){									// присваивание второму слою нейронов входных сигналов
				for (int j=0; j<prvniVrstva; j++){
					templ1.input[j]=l0.outputs[j];
				}
				templ1=templ1.next;
			}
	
			l1.countOutputs();												
			
		    templ1=l2.head;
			for (int i=0; i<tretiVrstva; i++){									// присваивание третьему слою нейронов входных сигналов
				for (int j=0; j<druhaVrstva; j++){
					templ1.input[j]=l1.outputs[j];
				}
				templ1=templ1.next;
			}
		    
		    
			l2.countOutputs();
			index=0;
		    max=l2.outputs[0];
		    for (int i=0; i<l2.outputs.Length; i++) {
			    if (max<l2.outputs[i])
					{
			    		max=l2.outputs[i];
						index=i;													// index показывает какой нейрон(цифра) выйграл/а
					}
			}
		    
      		return index;
		}
		
		public void study(Program p)
		{
			int testValue = 0;
			MainForm form = new MainForm("");
			double ErrorMin=100;										
		 	double [] err2 = new double[tretiVrstva];
		 	double lokError=4;
		 	double lokResult=0;										
		 	int iteration=0;
			int count=0;
		  	int epocha=1;
		  	double lastError=100;
		  	int maxTestValue=0;							//для сохранения самых обученых весов
			
		  	while (testValue< 100) {				
			  	lokError=0;
			  	lokResult=0;
			  	iteration=0;
			  	form.newEpoch();
			  	  	
			  	do
			  	{			  		
			  		count++;
			  		line = form.getLine(p);
			 		lokResult = calculateResult(line);
			 		
			 		
			 		Neuron templ2=l2.head;
			 		for  (int i=0; i<tretiVrstva; i++){
			 			if (answer==i)
			 				err2[i]= max - templ2.output ;									// присваивание сигнала ошибки выходному слою
			 			else
			 				err2[i] =0-templ2.output;
			 			templ2=templ2.next;	
			 		}
			 		
			 		
			 		Neuron templ1;
			 		templ2=l2.head;
			 		for (int i=0; i<tretiVrstva; i++)
			 		{	
			 			templ1=l1.head;
			 			for (int j=0; j<druhaVrstva; j++)
			 			{	templ2.grad=0.388*(1.7159-templ2.output)*(1.7159+ templ2.output)*err2[i];				// вычисление градиента для верхнего слоя
			 				templ2.weights[j]+=speedL*templ1.output*templ2.grad;									// вычисление весов для верхнего слоя
							templ1= templ1.next;		 				
			 			}
			 			templ2 = templ2.next;
			 		}
			 		
			 		double grad=0;
			 		Neuron templ0;
			 		templ1=l1.head;
			 		for (int i=0; i<druhaVrstva; i++)									
			 		{	
			 			grad=0;
			 			templ2=l2.head;
			 			for(int u=0; u<tretiVrstva; u++)							// суммирование градиента предыдущего слоя (дельта правило для втрого слоя)
			 				 {
				 				grad+=templ2.grad*templ2.weights[i];
				 				templ2=templ2.next;
			 				 }
			 			templ1.grad= grad*0.388*(1.7159-templ1.output)*(1.7159+ templ1.output);	
			 			templ0=l0.head;
			 			for (int j=0; j<prvniVrstva; j++)
			 			{		
			 				templ1.weights[j]+=speedL*templ0.output*templ1.grad;		
			 				templ0=templ0.next;
			 			}
			 			templ1=templ1.next;
			 		}
			 		
			 		templ0=l0.head;
			 		for (int i=0; i<prvniVrstva; i++)									
			 		{	grad=0;
			 			templ1=l1.head;
			 			for(int u=0; u<druhaVrstva; u++)							// суммирование градиента предыдущего слоя (дельта правило для первого слоя)
			 				 {
			 				grad+=templ1.grad*templ1.weights[i];
			 				templ1=templ1.next;
			 				 }
			 			grad= grad*0.388*(1.7159-templ0.output)*(1.7159+ templ0.output);	
			 			
			 			for (int j=0; j<inputLength; j++)
			 			{		
			 				templ0.weights[j]+=speedL*templ0.input[j]*grad;		
			 			}
			 			templ0= templ0.next;
			 		}
			 		iteration++;
			 		
			 		
			    	if (answer!=index){
			    		lokError++;
			    	}
			 		
			 		
			 		
			 		if ((lokError/iteration*100)<ErrorMin)
			    	{
			    		ErrorMin=lokError/iteration*100;
			    	}
			  	} while (iteration < 10);
			  	 testValue  = test(p);
			  	 if (testValue > maxTestValue) {
			  	 	maxTestValue = testValue;
			  	 	serializaceBetterResult(p);
			  	 }
			  	form.writeInfo(epocha, iteration, ErrorMin, testValue, counterNeuronNew);
			  	serializace(p);
			 
			  	if (speedL>0.002){
			  		if (Math.Abs(lastError-(lokError/iteration*100))<=2){
			 			speedL += -0.0001;
			  		}
			  	}
//			  	
//		  		if ((Math.Abs(lastError-lokError/iteration*100)<2)&&(epochWithoutNewNeuron>=6)) {
//		  			epochWithoutNewNeuron=0;
//		  			addNeuron();
//		  			String structure= "Первый слой-"+prvniVrstva.ToString()+ "Второй слой-"+druhaVrstva.ToString();
//		  			File.WriteAllText("Структура.txt", structure);
//		 		}
//			  	else 
//			  		epochWithoutNewNeuron++;
			  	
			  	lastError = lokError/iteration*100;
				epocha++;
		 }
		  	form.writeHotovo();
		}
		
		
		
		[STAThread]
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
		
		
		Boolean addNeuron(){
		counterNeuronNew++;
		Console.WriteLine("---------------------------Добавлен новый нерон!!!---------------------------------");
			if (counterNeuronNew>=50){
			
		}
//			writeWeights(2);
			if (counterAddNeuron==1){
				counterAddNeuron=0;
				l1.addNeuron();
				druhaVrstva++;
				l1.length=druhaVrstva;
				l1.outputs=new double[druhaVrstva];
				
				Neuron templ=l2.head;
				for(int i=0;i<tretiVrstva;i++){					
					templ.weights=new double[druhaVrstva];
					templ.doRandomWeights();
					templ.input= new double[druhaVrstva];
					templ=templ.next;
				}
				     
				templ=l1.head;
				while(templ.next!=null)
					templ=templ.next;
				templ.weights=new double[prvniVrstva];
				templ.doRandomWeights();
				templ.input=new double[prvniVrstva];
				
//				initialization("для второго слоя");
				return false;
			}
			else {
				counterAddNeuron++;
				l0.addNeuron();
				prvniVrstva++;
				l0.length=prvniVrstva;
				l0.outputs=new double[prvniVrstva];
				
				Neuron templ1=l1.head;
				for(int i=0;i<druhaVrstva;i++){
					templ1.weights=new double[prvniVrstva];
					templ1.input= new double[prvniVrstva];
					templ1=templ1.next;
				}
				
//				initialization(1);
				return true;
			}
		}
		
		
		
		void serializace(Program p) {
			BinaryFormatter formatter = new BinaryFormatter();
			using ( var fSream = new FileStream("weights.dat", FileMode.Create, FileAccess.Write, FileShare.None)) {
				formatter.Serialize(fSream, p);
			}
		}
		
		void serializaceBetterResult (Program p) {
			BinaryFormatter formatter = new BinaryFormatter();
			using ( var fSream = new FileStream("weightsBetter.dat", FileMode.Create, FileAccess.Write, FileShare.None)) {
				formatter.Serialize(fSream, p);
			}
		}
		
		
		public  Program deseralizace() {
			try {
				using (var fStream = File.OpenRead("weights.dat")) {
					BinaryFormatter formatter = new BinaryFormatter();
					return  (Program)formatter.Deserialize(fStream);
				}
			}
			catch (Exception e) {
				return new Program(5);
			}
		}
		
		
		public String testOnDisplay(Program p) {
			String vysledek=null;
			MainForm form= new MainForm("");
			form.newEpoch();
			for (int i=0; i<14; i++) {
				int indexL = p.calculateResult(form.getLine(p)) ;
				if (answer == indexL)	{								//p.calculateResult("11")
					vysledek += 1;
				}
				else 
					vysledek += 0;
				vysledek += "\r\n";
			}
		return vysledek;	
		}
		
	public	int test(Program p) {
			MainForm form = new MainForm("");
			
			int vysledek=0;
//			int countNewEpoch= 0;
			for (int j=0; j<10; j++) {
				form.newEpoch();
				for (int i=0; i<10; i++) {
//					countNewEpoch++;
					int vysOperace = p.calculateResult(form.getLine(p));
					if (answer == vysOperace) {			
						vysledek += 1;
					}
					else 
						vysledek += 0;
				}
			}
		return vysledek;
		}
	}
}
