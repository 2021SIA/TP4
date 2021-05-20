data_accuracy = csvread("accuracy.csv");

plot(data_accuracy(:,1),data_accuracy(:,2),"o-","linewidth",1.5);
hold on;
title("Ruido vs Accuracy");
xlabel("Ruido");
ylabel("Accuracy");
hold off;
