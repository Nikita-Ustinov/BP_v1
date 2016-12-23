using System;
using System.Windows.Forms;

namespace BP_pokus_2
{
	
	internal sealed class Program
	{	
		public double MAX=0;
		public int inputLength=441;
		public int prvniVrstva=186;
		public int druhaVrstva=184;
		public int tretiVrstva=10;
		
		public List l0,l1,l2;
		
		public int counterAddNeuron=0;
		public int epochaWithoutNewNeuron=10;
		
		double speedL=0.1;
		
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
		
		
		int calculateResult(String line)
		{
			l0.writeInput(line);												//присваивание входного вектора нейроной сети
			l0.countOutputs();
			
			Neuron templ0;
			Neuron templ1=l1.head;
			for (int i=0; i<druhaVrstva; i++){									// присваивание второму слою нейронов входных сигналов
				templ0=l0.head;
				for (int j=0; j<prvniVrstva; j++){
					templ1.input[j]=l0.outputs[j];
				}
				templ1=templ1.next;
			}
	
			l1.countOutputs();												
			
		    templ1=l2.head;
			for (int i=0; i<tretiVrstva; i++){	
				for (int j=0; j<druhaVrstva; j++){
					templ1.input[j]=l1.outputs[j];
				}
				templ1=templ1.next;
			}
			
		    l2.countOutputs();
		    
		    index=0;
		    max=l2.outputs[0];
		   
			for (int i=0; i<tretiVrstva;i++){									//выбор большего выхода НС
				if (max<l2.outputs[i])
				{
		    		max=l2.outputs[i];
					index=i;													// index показывает какой нейрон(цифра) выйграл/а
				}
			}

      		return index;
		}
		
		void study()
		{	double ErrorMin=100;										// сигналы ошибки для нижних слоев 
		 	double [] err2 = new double[tretiVrstva];
		 	double lokError=0;
		 	double lokResult=0;
		 	int counter=0;
		 	int iteration=0;
			int count=0;
		  	int epocha=1;
		  	double lastError=100;
		  	
		  	bool flag=true;
		  	
		  	Console.WriteLine ("итерация:");
		  	
		  	
		  	
		while(test()<90)  {
		  		
		  	lokError=0;
		  	lokResult=0;
		  	iteration=0;
		  	
		  	do
		  	{														
		  		if (counter==100)
		  		{
		  			counter=0;
		  			Console.WriteLine("  эпоха-"+epocha+"      итерация-"+iteration);
		  			
		  			writeWeights(1);
		  		}
		  		if (count==velikostVyukovyMnoziny*10-1){
		  			Console.WriteLine("моментальная ошибка составляет: "+lokError/iteration*100);
		  			if (lokError/iteration*100<1){
		  				flag=false;
		  			}
		  			count=0;
		  		}
		  		count++;
		  		counter++;
		  		doLine();
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
		
						if(templ2.grad==0){
							
						}
						
		 			}
		 			templ2 = templ2.next;
		 		}
		 		
		 		double grad=0;
		 		Neuron templ0;
		 		templ1=l1.head;
		 		for (int i=0; i<druhaVrstva; i++)									
		 		{	grad=0;
		 			templ2=l2.head;
		 			for(int u=0; u<tretiVrstva; u++)
		 				 {
		 				grad+=templ2.grad*templ2.weights[i];							// суммирование градиента предыдущего слоя (дельта правило для втрого слоя)
		 				templ2=templ2.next;
		 				 }
		 			templ1.grad= grad*0.388*(1.7159-templ1.output)*(1.7159+ templ1.output);	
		 			templ0=l0.head;
		 			
		 			if(grad==0){
		 				
		 			}
		 			
		 			for (int j=0; j<prvniVrstva; j++)
		 			{		
		 				templ1.weights[j]+=speedL*templ0.output*templ1.grad;		
		 				templ0=templ0.next;
		 				templ1.grad=grad;
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
		 			if(grad==0){
		 				
		 			}
		 			for (int j=0; j<inputLength; j++)
		 			{		
		 				templ0.weights[j]+=speedL*templ0.input[j]*grad;		
		 				templ0.grad=grad;
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
		    		Console.WriteLine("gErrorMin="+ ErrorMin);
		    		//writeWeights();
		    		
		    	}
		 		
		 		
		    	
		 	}while (iteration<velikostVyukovyMnoziny*10);
		  	
		  	if (speedL>0.002){
		  		if (Math.Abs(lastError-(lokError/iteration*100))<=2){
		 			speedL+=-0.001;
		  		}
		  	}
		  	
//		  	if (epochaWithoutNewNeuron>=5){
//		  		if (Math.Abs(lastError-lokError/iteration*100)<1){
//		  			epochaWithoutNewNeuron=0;
//		  			speedL=0.01;
//		  		}
//		  		}
//		  	
		  	
		  	
		  	if (epochaWithoutNewNeuron>=6){
		  	if (Math.Abs(lastError-lokError/iteration*100)<1)
		 		{	epochaWithoutNewNeuron=0;
		  	//	addNeuron();
		 			
		 			using (StreamWriter sr1 = new StreamWriter(@"Структура НС.txt")){
		 				sr1.WriteLine("Первый слой-"+prvniVrstva);
		 				sr1.WriteLine("Второй слой-"+druhaVrstva);
		 			}
		  	}
		  	
		  	}
		  	else 
		  		epochaWithoutNewNeuron++;
		  	
		  	
		  	
		  	
		  	lastError =lokError/iteration*100;
		  //	count0=0;count1=0;count2=0;count3=0;count4=0;count5=0;count6=0;count7=0;count8=0;count9=0;
		  	test(1);
		  //	count0=0;count1=0;count2=0;count3=0;count4=0;count5=0;count6=0;count7=0;count8=0;count9=0;
		  	epocha++;
		  	 writeWeights(3,2);
		  	 
        }
		 
		    writeWeights(1);
		  	Console.WriteLine("gError="+lokError/iteration*100);
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
			writeWeights(2);
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
		
	}
}
