data_energy1 = csvread("energy_0.csv");
data_energy2 = csvread("energy_1.csv");
data_energy3 = csvread("energy_2.csv");
data_energy4 = csvread("energy_3.csv");
data_energy5 = csvread("energy_4.csv");
data_energy6 = csvread("energy_5.csv");
data_energy7 = csvread("energy_6.csv");
data_energy8 = csvread("energy_7.csv");
data_energy9 = csvread("energy_8.csv");
data_energy10 = csvread("energy_9.csv");

y_avg = [];
err = [];
x = [];
len = max([length(data_energy1) length(data_energy2) length(data_energy3) length(data_energy4) length(data_energy5) length(data_energy6) length(data_energy7) length(data_energy8) length(data_energy9) length(data_energy10)]);
for i = 1:len
  x = [x i - 1];
  values = [];
  if(length(data_energy1(:,1)) >= i)
    values = [values data_energy1(i,2)];
  endif
  if(length(data_energy2(:,1)) >= i)
    values = [values data_energy2(i,2)];
  endif
  if(length(data_energy3(:,1)) >= i)
    values = [values data_energy3(i,2)];
  endif
  if(length(data_energy4(:,1)) >= i)
    values = [values data_energy4(i,2)];
  endif
  if(length(data_energy5(:,1)) >= i)
    values = [values data_energy5(i,2)];
  endif
  if(length(data_energy6(:,1)) >= i)
    values = [values data_energy6(i,2)];
  endif
  if(length(data_energy7(:,1)) >= i)
    values = [values data_energy7(i,2)];
  endif
  if(length(data_energy8(:,1)) >= i)
    values = [values data_energy8(i,2)];
  endif
  if(length(data_energy9(:,1)) >= i)
    values = [values data_energy9(i,2)];
  endif
  if(length(data_energy10(:,1)) >= i)
    values = [values data_energy10(i,2)];
  endif
  y_avg = [y_avg mean(values)];
  err = [err std(values)];
end

errorbar(x,y_avg,err,"o-");
hold on;
title("Energía");
xlabel("Iteración");
ylabel("Energía");
hold off;