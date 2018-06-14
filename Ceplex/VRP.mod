int QuantityOfClients = ...;
range Locations = 1..(QuantityOfClients + 1);

float Distance[Locations][Locations] = ...;
float ClientsDemand[Locations] = ...;

float wtot = ...;

dvar boolean x[Locations][Locations];
dvar int u[Locations];
dvar float d[Locations][Locations];

minimize sum(i in Locations, j in Locations) d[i][j];

subject to{
	UmEApenasUmCaminhoDeveSairDeTodoLocal:
	forall(i in Locations) {
		sum(j in Locations: i != j) x[i][j] == 1;
	}
	
	UmEApenasUmCaminhoDeveChegarEmTodoLocal:
	forall(j in Locations: j > 1){
		sum(i in Locations: i != j) x[i][j] == 1;	
	}
	
	R1:
	forall(i in Locations, j in Locations){
		2*wtot*(1 - x[i][j]) >= u[i] - u[j] + ClientsDemand[i];	
	}
	
	R2:
	forall(i in Locations, j in Locations){
		2*wtot*(1 - x[i][j]) >= (wtot - u[j])*Distance[i][j] - d[i][j];	
	}
	
	R3:
	forall(j in Locations){
		u[j] <= wtot;
	}
	
	R4: 
	forall(j in Locations){
		u[j] >= 0;	
	}
	
	
}

execute {
	var f = new IloOplOutputFile("C:\\Users\\Richard Sobreiro\\Desktop\\PFCCodigos\\Backend\\VRPTW_Server\\Ceplex\\SolutionTSP.txt");

	for(var i in Locations) {
		f.writeln(u[i]);		
	}
		
	f.close();
}