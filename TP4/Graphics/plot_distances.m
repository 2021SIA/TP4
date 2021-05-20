data = csvread("distances.csv");

map = [];
row = [];
for i = 1:length(data)
  if i > 1 && mod(i - 1,8) == 0
    map = [map;row];
    row = [];
  endif
  row = [row data(i,1)];
endfor
map = [map;row];
x = y = 1:8;
colormap("gray");
imagesc(x,y,map);
colorbar();
hold on;
xticks(1:8);
yticks(1:8);
hold off;
