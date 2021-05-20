pc = [-0.196 0.48 -0.468 0.4905 -0.1302 0.4488 -0.2291];

data = csvread("europe.csv");
[v1,v2,v3,v4,v5,v6,v7,v8] = textread("europe.csv", '%s %d %d %d %d %d %d %d','delimiter' , ',' ,'headerlines',1);

m = [];
s = [];
for i = 2:8
  m = [m mean(data(2:end,i))];
  s = [s std(data(2:end,i))];
endfor
values = [];
for i = 2:length(data)
  values = [values;dot((data(i,2:end) - m) ./ s,pc)];
endfor

barh(values);
yticks([1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28]);
yticklabels({'Austria';'Belgium';'Bulgaria';'Croatia';'Czech';'Denmark';'Estonia';'Finland';'Germany';'Greece';'Hungary';'Iceland';'Ireland';'Italy';'Latvia';'Lithuania';'Luxembourg';'Netherlands';'Norway';'Poland';'Portugal';'Slovakia';'Slovenia';'Spain';'Sweden';'Switzerland';'Ukraine';'United Kingdom'});
hold off;
