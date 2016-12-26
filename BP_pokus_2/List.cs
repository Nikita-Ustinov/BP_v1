using System;

namespace BP_pokus_2
{
	[Serializable]
	public class List
	{
		public	int length;
		public Neuron head;
	    public double [] outputs;
	    int vrstva;
		Program p = new Program();
		
		
		public List(int vrstva)
		{
			this.vrstva=vrstva;
			if (vrstva==0)
				length=p.prvniVrstva;
			if(vrstva==1)
				length=p.druhaVrstva;
			if(vrstva==2)
				length=p.tretiVrstva;
			Neuron templ;
			
			for (int i=0; i< length; i++){
				Neuron node=new Neuron(vrstva);
				if (i==0)
					head=node;
				else {
					 templ=head;
					 while(templ.next!=null){
					 	templ = templ.next;
					 }
					 
					do{
					   node= new Neuron(vrstva);
					 } while(templ.weights[0]==node.weights[0]);
					 
					 templ.next=node;
					 }
			}
			
			outputs=new double[length];
			
		}
		
			
		public void writeInput(String line){
			Neuron templ=head;
			for (int i=0; i<p.prvniVrstva; i++){
				templ.input = new double[p.inputLength];
				for (int j=0; j<p.inputLength; j++){															//ИЗМЕНИТЬ!!!! ??
					templ.input[j]=double.Parse(line[j].ToString());
				}
				templ=templ.next;
			}
		}
		

		
		public void countOutputs(){
			Neuron templ=head;
			int counter=0;
			while(templ!=null){
				outputs[counter]=templ.countOut();
				templ= templ.next;
				counter++;
			}
		}
		
		public void addNeuron(){
			Neuron templ;
			templ=head;
			while(templ.next!=null){
				templ= templ.next;
			}
			Neuron node= new Neuron(vrstva);
			templ.next=node;
		}
	}
}

