function total_length = calculate_length(T)

%% Calculate the length of the trajectory

total_length = 0;
k = length(T);
for i=1:k-1
    px1 = T(i,1);
    px2 = T(i+1,1);
    py1 = T(i,2);
    py2 = T(i+1,2);
    pz1 = T(i,3);
    pz2 = T(i+1,3);
    distance = sqrt((px1-px2)^2+(py1-py2)^2+(pz1-pz2)^2);
    total_length = total_length + distance;
end



end

