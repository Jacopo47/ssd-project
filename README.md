# ssd-project
## Progetto per corso di 'Sistemi di supporto alle decisioni' - anno 2019/2020


Il progetto si suddivide in 3 progetti comunicanti tra loro. 

### Progetto 1 - View (ReactJS)
All'interno della cartella **ClientApp** è possibile trovate un progetto npm in ReactJS che si occupa di servire la parte client. 
E' possibile lanciarlo singolarmente eseguendo il comando: 

> npm start


### Progetto 2 - Server (dotnet)
Alla root del progetto è possibile trovare il progetto che include tutti gli altri e che si occupa di 
inserirsi come middleware di comunicazione. 
Si tratta di un server che si basa su dotnet core e si occupa anche di fornire la parte client. 
Infatti con il seguente comando viene servito anche il client oltre che eseguito il server. 

> dotnet run

### Progetto 3 - Server per la previsione (python)
Si tratta di un piccolo server HTTP scritto in python e servito da flask che risponde a determinati endpoint e si occupa di eseguire algoritmi 
di previsione / ML sui dati. Il server principale contatterà le API fornite da questo server per ottenere i dati da passare alla GUI. 
Per eseguire il progetto su Linux è possibile usare lo script:

> ./runFlask.sh