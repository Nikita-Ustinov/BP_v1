using System;
using System.IO;

namespace BP_pokus_2
{
	[Serializable]
	public class Neuron
	{
		public	double [] weights, input;
		public	double output, grad, sum ;
	    public	Neuron next;
		Program p=new Program();
	
			
		public Neuron(int vrstva)
		{	
			if (vrstva==0){
				weights = new double[p.inputLength];
				input = new double[p.inputLength];}
			if (vrstva==1){
				weights = new double[p.prvniVrstva];
				input=new double[p.prvniVrstva];}
			if (vrstva==2){
				weights = new double[p.druhaVrstva];
				input=new double[p.druhaVrstva];}
			
			doRandomWeights();
			
		}		
		
		
		public void doRandomWeights()							// для первого нейрона
		{  
			Random rand = new Random();
			for (int i=0;i<weights.Length; i++){
				do{
			 	weights[i] =rand.Next(-6,5)*0.1 + 0.1;
				} while (weights[i]==0);
				if(weights[i]==0){
					weights[i]=1;
				}
			}
		}
		
		
		public void doZeroWeights(){
			for (int i=0;i<weights.Length; i++){
				weights[i] = 0;
			}
		}
		
		
		public	double countOut()
		{
			sum=0;
			output=0;
			for (int i=0; i<input.Length; i++)
			{
				sum+=weights[i]*input[i];
			}
			output=1.7159*Math.Tanh(0.66*sum);
		    return output;
		}
		
	}
}

