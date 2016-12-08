function sub_point = subtrajectories(T,num_segments,total_length)

%% Create sub-trajectories/segments from the original trajectory

k = length(T);
length_segment = total_length/num_segments;
px = T(1,1);
py = T(1,2);
pz = T(1,3);
p = 0;

for i=2:k
    pxi = T(i,1);
    pyi = T(i,2);
    pzi = T(i,3);
    distance = sqrt((px-pxi)^2+(py-pyi)^2+(pz-pzi)^2);
    if distance >= length_segment
        plot3(T(i,1), T(i,2), T(i,3), 'b')
        p = p + 1;
        sub_point{p} = i;
        px = T(i-1,1);
        py = T(i-1,2);
        pz = T(i-1,3);
        T(i-1,4)=1;
    end
end

end

