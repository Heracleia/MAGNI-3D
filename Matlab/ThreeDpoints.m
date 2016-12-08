function trajectory = ThreeDpoints(num_segments,sub_point,M)
%UNTITLED Summary of this function goes here
%   Detailed explanation goes here

trajectory = [];

for i=1:length(sub_point)
    
    x = M(sub_point{i},1);
    y = M(sub_point{i},2);
    z = M(sub_point{i},3);
    
    trajectory = [trajectory; x y z];
end



