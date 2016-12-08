function draw_line(p1,p2)
    

hold on
% Their vertial concatenation is what you want
pts = [p1; p2];

% Because that's what line() wants to see    
line(pts(:,1), pts(:,2), pts(:,3))

% Alternatively, you could use plot3:
%plot3(pts(:,1), pts(:,2), pts(:,3))
hold off

end