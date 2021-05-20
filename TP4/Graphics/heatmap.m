out = zeros(8,8);

for i = classification'
x=i(1)+1;
y=i(2)+1;
out(x,y)=out(x,y)+1;
endfor

clf
imagesc(out)
colormap(jet(3))
colorbar