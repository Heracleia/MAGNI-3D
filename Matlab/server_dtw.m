function error = server_dtw(csv_file1, csv_file2, C)

%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
%% DTW EXECUTION

patientID = C{1};
sessionID = C{2};
playlistID = C{3};
exerciseID = C{4};

csv_file1 = strcat(csv_file1, '.csv')
csv_file2 = strcat(csv_file2, '.csv')

num_segments = 50;

%% DTW implementation for 3D trajectories

%% PLOT INFORMATION
figure(1)
title('Trajectory error deviations');
xlabel('x')
ylabel('y')
zlabel('z')
axis ([-10 10 -10 10 -10 10])
view(-40,50);
hold on

%%%%%%%%%%%%%%%%%%%%%%%%%% Therapist Trajectory %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
cd('C:\Users\Public\Exercise');
M = csvread(csv_file1);
%% Calculate the length of the trajectory
total_length = calculate_length(M);
%% Create sub-trajectories/segments from the original trajectory
sub_point1 = subtrajectories(M,num_segments,total_length);
t = ThreeDpoints(num_segments,sub_point1,M);
figure(1);

h(:,1) = plot3(t(:,1),t(:,2),t(:,3),'r');

hold on;
M1 = M;
clear M;

%%%%%%%%%%%%%%%%%%%%%%%%%% Patient Trajectory %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
file_name = strcat('C:\Users\Public\Patient', '\PID_', patientID, '\Session_', sessionID);
cd(file_name);
M = csvread(csv_file2);

%% Calculate the length of the trajectory
total_length = calculate_length(M);
%% Create sub-trajectories/segments from the original trajectory
sub_point2 = subtrajectories(M,num_segments,total_length);
r = ThreeDpoints(num_segments,sub_point2,M);
figure(1);
h(:,2)= plot3(r(:,1),r(:,2),r(:,3),'g');
hold on;
M2 = M;
%%%%%%%%%%%%%%%%%%%%%%%%%%%% START DTW %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
n = size(t,1);
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

error = D(n,m);

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

xlim([-7 7])
ylim([-7 7])
zlim([-8.5 0])


%set(h(:,1), 'Color', 'r');
%set(h(:,2), 'Color', 'g');
%legend(h(1,:), {'Therapist Trajectory', 'Patient Trajectory'});

message2 = ['error =' num2str(error)];
handleAnnotation = annotation ( 'textbox' , [0.05 0.05 0.2 0.07] , 'string' , message2 );

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



%% Save the DTW analysis

%Save dtw figure in folder
cd(file_name);
csv_figure_name = strcat('dtw_', 'session', sessionID, '_playlist', playlistID, '_exercise', exerciseID, '.png');
print(csv_figure_name,'-dpng','-r200');
close all;



    
    
    
end