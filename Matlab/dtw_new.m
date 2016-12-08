function error = dtw_new(csv_file1, csv_file2, num_segments)

%% DTW implementation for 3D trajectories

%% PLOT INFORMATION
figure(1)
title('Trajectory segmentation and sub-projection');
xlabel('x')
ylabel('y')
zlabel('z')
axis ([-1 1 -1 1 -1 1])
view(-40,50);
hold on

%%%%%%%%%%%%%%%%%%%%%%%%%% First Trajectory %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
M = csvread(csv_file1);
%% Calculate the length of the trajectory
total_length = calculate_length(M);
%% Create sub-trajectories/segments from the original trajectory
sub_point1 = subtrajectories(M,num_segments,total_length);
t = ThreeDpoints(num_segments,sub_point1,M);
figure(1);
plot3(t(:,1),t(:,2),t(:,3),'r');
hold on;
M1 = M;
clear M;
%%%%%%%%%%%%%%%%%%%%%%%%%% Second Trajectory %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
M = csvread(csv_file2);
%% Calculate the length of the trajectory
total_length = calculate_length(M);
%% Create sub-trajectories/segments from the original trajectory
sub_point2 = subtrajectories(M,num_segments,total_length);
r = ThreeDpoints(num_segments,sub_point2,M);
figure(1);
plot3(r(:,1),r(:,2),r(:,3),'g');
hold on;
M2 = M;
%%%%%%%%%%%%%%%%%%%%%%%%%%%% START DTW %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
n = size(t,1) ;
m = size(r,1);

% frame match distance matrix
d = zeros(n,m);

for i = 1:n
    for j = 1:m
        % euclidean distance
        d(i,j) = sum((t(i,:)-r(j,:)).^2);
    end
end

% accumulate distance matrix
D =  ones(n,m) * realmax;
D(1,1) = d(1,1);
% dynamic programming
for i = 2:n
    for j = 1:m
        D1 = D(i-1,j);
        
        if j>1
            D2 = D(i-1,j-1);
        else
            D2 = realmax;
        end
        
        if j>1
            D3 = D(i,j-1);
        else
            D3 = realmax;
        end
        [elem pos]=min([D1,D2,D3]);
        direc(i,j) = pos;
        
        D(i,j) = d(i,j) + elem;
    end
end

error = 100*D(n,m);

% the last items are always aligned
warp(n,m) = 1;

% trace back
trackback = direc(n,m);
coordinate = [n, m];

while trackback~=0
    switch trackback
         case 1
            trackback = direc(coordinate(1)-1, coordinate(2));
            coordinate = [coordinate(1)-1, coordinate(2)];
        case 3
            trackback = direc(coordinate(1), coordinate(2)-1);
            coordinate = [coordinate(1), coordinate(2)-1];
        case 2
            trackback = direc(coordinate(1)-1, coordinate(2)-1);
            coordinate = [coordinate(1)-1, coordinate(2)-1];
    end
    warp(coordinate(1), coordinate(2)) = 1;
end

figure(1)
for i = 1:n
    for j = 1:m
        if(warp(i,j)==1)
            a = ThreeDpoint(i,sub_point1,M1);
            b = ThreeDpoint(j,sub_point2,M2);
            draw_line(b,a);
            hold on;
            
        end
        
        
    end
end

figure(2)
plotWarp(warp);
