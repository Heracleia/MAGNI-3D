function plotWarp(warp)

[i,j] = find(warp);
title('Dynamic Time Warping');
xlabel('sequence 2');
ylabel('sequence 1');
line(i,j);

end